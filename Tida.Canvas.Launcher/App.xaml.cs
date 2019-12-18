using Tida.Canvas.Shell.Contracts.App;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFApplication = System.Windows.Application;
using Tida.Canvas.Shell.Contracts.Shell;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Launcher {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : WPFApplication {
        public App() {

            DispatcherUnhandledException += (sender, e) => {
                LoggerService.WriteCallerLine("主线程错误:" + e.Exception.Message);
                e.Handled = true;
            };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
                Console.WriteLine((e.ExceptionObject as Exception).Message);
                LoggerService.WriteCallerLine("工作线程错误:" + ((Exception)e.ExceptionObject).Message);
                LoggerService.WriteCallerLine("工作线程错误:" + ((Exception)e.ExceptionObject).StackTrace);
                if (e.ExceptionObject is Exception ex && ex.InnerException != null) {
                    LoggerService.WriteLine("工作线程错误:" + ex.InnerException.StackTrace);
                    LoggerService.WriteLine("工作线程错误: " + ex.InnerException.Message);
                }

                if (e.ExceptionObject is NullReferenceException nullex) {
                    LoggerService.WriteLine("Source:" + nullex.Source);
                    var enumrator = nullex.Data.GetEnumerator();
                    while (enumrator.MoveNext()) {
                        LoggerService.WriteLine("Object:" + enumrator.Current.ToString());
                    }
                }
            };
            InitializeComponent();

        }
        protected override void OnStartup(StartupEventArgs e) {

            
            new BootStrapper().Run();
            
            

            base.OnStartup(e);

            if (!ShellService.Current.Initialized) {
                ShellService.Current.Initialize();
            }

            ServiceProvider.GetInstance<IShellService>().Show();

            //主窗体关闭时,关闭应用程序对象;
            CommonEventHelper.GetEvent<ShellClosingEvent>().Subscribe(Shell_Closing);
        }

        private void Shell_Closing(CancelEventArgs obj) {
            ServiceProvider.GetInstance<IAppDomainService>()?.Dispose();
        }

        
    }
}
