using System;
using System.ComponentModel.Composition;
using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using Tida.Canvas.Shell.Contracts.StatusBar;
using Prism.Mef.Modularity;
using Prism.Modularity;

namespace Tida.Canvas.Shell.StatusBar {
    [Export(typeof(IShellInitializingEventHandler))]
    class ShellInitializingNavigateStatusBarHandler : IShellInitializingEventHandler {
        public int Sort => 233;

        public bool IsEnabled => true;

        public void Handle() {
            StatusBarService.Current.Initialize();
            //导航默认文字状态栏为欢迎(龙骨编辑器——悉道);
            StatusBarService.Report(LanguageService.FindResourceString(Constants.StatusBarBrandText));
        }
    }

    /// <summary>
    /// 状态栏模块;
    /// </summary>
    [ModuleExport(typeof(StatusBarModule))]
    class StatusBarModule : IModule {
        public void Initialize() {
            RegionHelper.RegisterViewWithRegion(Contracts.Shell.Constants.RegionName_StatusBar, typeof(Views.StatusBarView));
            
            
        }
        
    }
}
