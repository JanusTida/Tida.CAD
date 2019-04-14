using System.ComponentModel.Composition;
using Prism.Mvvm;
using System;
using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.App;
using Tida.Canvas.Shell.Contracts.MainPage.Events;

namespace Tida.Canvas.Shell.MainPage.ViewModels {
    [Export]
    public partial class MainPageViewModel : BindableBase {
        public MainPageViewModel() {
            
        }
        
        /// <summary>
        /// 主页被加载时命令;
        /// </summary>
        private Prism.Commands.DelegateCommand _loadedCommand;
        public Prism.Commands.DelegateCommand LoadedCommand => _loadedCommand ??
            (_loadedCommand = new Prism.Commands.DelegateCommand(
                () => {
                    try {
                        CommonEventHelper.Publish<MainPageLoadedEvent>();
                        CommonEventHelper.PublishEventToHandlers<IMainPageLoadedEventHandler>();
                    }
                    catch(Exception ex) {
                        LoggerService.WriteException(ex);
                        MsgBoxService.Show(ex.Message);
                    }
                }
            ));

      
    }
    
    
    
    
    
}
