using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.App {
    [Export(typeof(IAppDomainService))]
    class AppDomainServiceImpl : IAppDomainService {
        public string EnvironmentDirectory => Environment.CurrentDirectory;

        private string _executingAssemblyDirectory;
        public string ExecutingAssemblyDirectory {
            get {
                if (_executingAssemblyDirectory == null) {
                    RefreshExecutingAssemblyDirectory();
                }

                return _executingAssemblyDirectory;
            }
        }
        
        public string GetExecutingAssemblyDirectory() {
            RefreshExecutingAssemblyDirectory();
            return _executingAssemblyDirectory;
        }

        /// <summary>
        /// 重新查询当前所运行的程序集路径;
        /// </summary>
        private void RefreshExecutingAssemblyDirectory() {
            _executingAssemblyDirectory = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";
        }

        //当前应用程序实例;
        private System.Windows.Application _currentApplication;

        public void Initialize() {
            if (_currentApplication == null) {
                if (System.Windows.Application.Current == null) {
                    _currentApplication = new System.Windows.Application();
                }
                else {
                    _currentApplication = System.Windows.Application.Current;
                }
            }

            _currentApplication.DispatcherUnhandledException += (sender, e) => {
                LoggerService.WriteException(e.Exception);
                e.Handled = true;
            };

            _currentApplication.Exit += (sender, e) => {
                LoggerService.WriteCallerLine($"Application has been exited.exit code :{e.ApplicationExitCode}");
            };

            _currentApplication.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
                LoggerService.WriteCallerLine("工作线程错误:" + ((Exception)e.ExceptionObject).Message);
                LoggerService.WriteCallerLine("工作线程错误:" + ((Exception)e.ExceptionObject).StackTrace);
                var ex = e.ExceptionObject as Exception;
                if (ex != null && ex.InnerException != null) {
                    LoggerService.WriteLine("工作线程错误:" + ex.InnerException.StackTrace);
                    LoggerService.WriteLine("工作线程错误: " + ex.InnerException.Message);
                }
                
                var nullex = e.ExceptionObject as NullReferenceException;
                if (nullex != null) {
                    LoggerService.WriteLine("Source:" + nullex.Source);
                    var enumrator = nullex.Data.GetEnumerator();
                    while (enumrator.MoveNext()) {
                        LoggerService.WriteLine("Object:" + enumrator.Current.ToString());
                    }
                }
            };
            
        }

        public void Dispose() {
            _currentApplication?.Shutdown();
            _currentApplication = null;
        }

        public void AddResourceDictionary(Uri resourcePath) {

            if (resourcePath == null) {
                throw new ArgumentNullException(nameof(resourcePath));
            }

            if(_currentApplication == null) {
                throw new InvalidOperationException($"{nameof(_currentApplication)} is null,make sure that {nameof(Initialize)} has been invoked.");
            }

            var themeDict = _currentApplication.Resources.MergedDictionaries.FirstOrDefault(p => (p as ResourceDictionaryEx)?.Name == Constants.ThemeDict) as ResourceDictionaryEx;
            if(themeDict == null) {
                LoggerService.WriteCallerLine($"{nameof(themeDict)} is not found.");
                return;
            }

            themeDict.MergedDictionaries.Add(new ResourceDictionary { Source = resourcePath });
        }
    
    }
}
