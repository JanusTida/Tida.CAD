using System;
using System.ComponentModel.Composition;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using Prism.Mef.Modularity;
using Prism.Modularity;

namespace Tida.Canvas.Shell.Ribbon {
    [Export(typeof(IShellInitializingEventHandler))]
    class ShellInitializingNavigateRibbonHandler : IShellInitializingEventHandler {
        public int Sort => 0;

        public bool IsEnabled => true;

        public void Handle() {
            ServiceProvider.GetInstance<IRibbonService>().Initialize();

            RegionHelper.RegisterViewWithRegion(
                Contracts.Shell.Constants.RegionName_Ribbon,
                typeof(Views.Ribbon)
            );
        }
    }
    
}
