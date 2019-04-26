using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Tida.Canvas.Shell.Shell.ViewModels;
using Tida.Canvas.Shell.Contracts.Shell;
using System;
using Tida.Application.Contracts.Controls;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using System.ComponentModel;
using Tida.Application.Contracts.App;

namespace Tida.Canvas.Shell.Shell {
    /// <summary>
    /// 主窗体服务;
    /// </summary>
    [Export(typeof(IShellService))]
    public class ShellServiceImpl : IShellService {
        [ImportingConstructor]
        public ShellServiceImpl(ShellViewModel shellVM, Views.Shell shell) {
            this._shellVM = shellVM;
            _shell = shell;
        }
        
        public void Initialize() {
            if (Initialized) {
                return;
            }

            _shell.Closing += Shell_Closing;
            CommonEventHelper.GetEvent<ShellInitializingEvent>().Publish();
            CommonEventHelper.PublishEventToHandlers<IShellInitializingEventHandler>();
            Initialized = true;
        }

        public bool Initialized { get; private set; }

        private void Shell_Closing(object sender, CancelEventArgs e) {
            CommonEventHelper.Publish<ShellClosingEvent, CancelEventArgs>(e);
            CommonEventHelper.PublishEventToHandlers<IShellClosingEventHandler,CancelEventArgs>(e);
        }

        private void ShellVM_ClosingRequest(object sender, CancelEventArgs e) {
            //if (ShellClosingEventHandlers == null) {
            //    return;
            //}

            //var args = new ShellClosingEventArgs(e);
            //foreach (var handler in ShellClosingEventHandlers) {
            //    handler.Handle(args);
            //    if (args.Handled) {
            //        break;
            //    }
            //}
        }

        private ShellViewModel _shellVM;
        
        //更改标题栏文字;
        public void SetTitle(string word, bool saveBrandName = true) {
            _shellVM.SetTitle(word, saveBrandName);

        }

        public void Focus() {
            (_shell as Window)?.Focus();
        }

        /// <summary>
        /// 改变等待状态;
        /// </summary>
        /// <param name="isLoading"></param>
        /// <param name="word"></param>
        public void ChangeLoadState(bool isLoading, string word = null) {
            //_shellVM.IsLoading = isLoading;
            //_shellVM.LoadingWord = word;
        }


        /// <summary>
        /// /// 添加快捷键绑定;
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="key">案件</param>
        /// <param name="modifier">修饰键</param>
        /// <param name="commandPara">命令参数</param>
        public void AddKeyBinding(ICommand command, Key key, ModifierKeys modifier = ModifierKeys.None) {
            if (command == null) {
                throw new ArgumentNullException(nameof(command));
            }

            if(key == Key.None) {
                return; 
            }
            
            if(modifier == ModifierKeys.None) {
                _shell.InputBindings.Add(new KeyBinding {
                    Command = command,
                    Key = key
                });
            }
            else {
                _shell.InputBindings.Add(new KeyBinding(command, key, modifier));
            }
            
        }
        
        public void Show() {
            _shell.ShowActivated = true;
            _shell.Show();
        }

        
        public void Close() => _shell.Close();

        
        public void ShowDialog(object owner = null) {
            if (!Initialized) {
                Initialize();
            }

            _shell.ShowDialog(owner);
        }

        public void Hide() {
            _shell.Hide();
        }

        private Views.Shell _shell;
        public object Shell => _shell;// ViewProvider.GetView(Contracts.Shell.Constants.ShellView);

        private IStackGrid<IUIObjectProvider> _stackGrid;
        public IStackGrid<IUIObjectProvider> StackGrid {
            get {
                if(_stackGrid == null) {
                    _stackGrid = StackGridFactory.CreateNew<IUIObjectProvider>(_shell.Grid);
                    _stackGrid.Orientation = System.Windows.Controls.Orientation.Vertical;
                    _stackGrid.SplitterLength = 0;
                }
                return _stackGrid;
            }
        }
    }
}
