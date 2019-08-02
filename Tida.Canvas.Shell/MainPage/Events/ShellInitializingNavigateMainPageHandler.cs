using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Controls;
using Tida.Canvas.Shell.Contracts.MainPage;
using Tida.Canvas.Shell.Contracts.Shell;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.MainPage.Events {
    /// <summary>
    /// Shell初始化时使用MainPage占领主视图;
    /// </summary>
    [Export(typeof(IShellInitializingEventHandler))]
    class ShellInitializingNavigateMainPageHandler : IShellInitializingEventHandler {
        public int Sort => 0;

        public bool IsEnabled => true;

        public void Handle() {
            //ServiceProvider.GetInstance<IRibbonService>().Initialize();

            RegionHelper.RegisterViewWithRegion(
                Contracts.Shell.Constants.RegionName_MainPage,
                typeof(Views.MainPage)
            );
            //var mainPageView = ServiceProvider.GetInstance<Views.MainPage>();
            ////画布加入导航;
            //ShellService.Current.StackGrid.AddChild(
            //    UIObjectProviderFactory.CreateNew(mainPageView),
            //    new GridChildLength(
            //        new System.Windows.GridLength(
            //            1, System.Windows.GridUnitType.Star
            //        )
            //    )
            //);

        }
    }
}
