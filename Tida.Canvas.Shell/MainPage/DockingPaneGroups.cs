using Tida.Application.Contracts.Docking;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.MainPage.Constants;

namespace Tida.Canvas.Shell.MainPage {
    [ExportDockingGroup(ContainerGUID = DockingContainer_Left, GUID = DockingPaneGroup_Left)]
    class LeftDockingPaneGroup : IDockingGroup {
    }

    [ExportDockingGroup(ContainerGUID = DockingContainer_Right, GUID = DockingPaneGroup_Right)]
    class RightDockingPaneGroup : IDockingGroup {

    }

    [ExportDockingGroup(ContainerGUID = DockingContainer_Bottom, GUID = DockingPaneGroup_BottomLeft)]
    class BottomDockingPaneGroup : IDockingGroup {
    }

    [ExportDockingGroup(ContainerGUID = DockingContainer_Top, GUID = DockingPaneGroup_Top)]
    class TopDockingPaneGroup : IDockingGroup {

    }

    [ExportDockingGroup(ContainerGUID = DockingContainer_Bottom, GUID = DockingPaneGroup_BottomRight, Order = 123)]
    class BottomRightDockingGroup : IDockingGroup {

    }
    //[ExportDockingGroup(ContainerGUID = DockingContainer_Document, GUID = DockingPaneGroup_Document,NoStyle = true)]
    //class DocumentDockingPaneGroup : IDockingGroup {

    //}
}
