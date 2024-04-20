
using Tida.Canvas.Shell.Contracts.Docking;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Tida.Canvas.Shell.Contracts.MainPage.Constants;

namespace Tida.Canvas.Shell.MainPage {
    /// <summary>
    /// 停靠区域容器-左;
    /// </summary>
    [ExportDockingContainer(DockingServiceGUID = DockingService_Main,GUID = DockingContainer_Left,InitDockingPosition = DockingPosition.Left)]
    class LeftDockingPaneContainer : IDockingContainer {
    }

    /// <summary>
    /// 停靠区域容器-右;
    /// </summary>
    [ExportDockingContainer(DockingServiceGUID = DockingService_Main, GUID = DockingContainer_Right, InitDockingPosition = DockingPosition.Right)]
    class RightDockingPaneContainer : IDockingContainer {
    }

    /// <summary>
    /// 停靠区域容器-底部;
    /// </summary>
    [ExportDockingContainer(DockingServiceGUID = DockingService_Main, GUID = DockingContainer_Bottom, InitDockingPosition = DockingPosition.Bottom,Orientation = Orientation.Horizontal)]
    class BottomDockingPaneContainer : IDockingContainer {
    }

    /// <summary>
    /// 停靠区域容器-顶部;
    /// </summary>
    [ExportDockingContainer(DockingServiceGUID = DockingService_Main, GUID = DockingContainer_Top, InitDockingPosition = DockingPosition.Top)]
    class TopDockingPaneContainer : IDockingContainer {
    }

    ///// <summary>
    ///// 停靠区域容器-文档;
    ///// </summary>
    //[Export(typeof(IDockingContainer))]
    //class DocumentDockingPaneContainer : DockingContainerBase {
    //    public override string DockingServiceName => DockingService_Main;

    //    public override string GUID => DockingContainer_Document;

    //    public override DockingPosition InitDockingPosition { get; set; } = DockingPosition.Right;
    //}

    ///// <summary>
    ///// 停靠区域容器-底部;
    ///// </summary>
    //[Export(typeof(IDockingContainer))]
    //class BottomDockingPaneContainer : DockingContainerBase {
    //    public BottomDockingPaneContainer() {

    //    }

    //    public override string DockingServiceName => DockingService_Main;

    //    public override string GUID => DockingContainer_Document;

    //    public override DockingPosition InitDockingPosition { get; set; } = DockingPosition.Right;
    //}
}
