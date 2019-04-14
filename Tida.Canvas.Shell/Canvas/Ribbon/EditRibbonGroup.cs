using Tida.Canvas.Shell.Contracts.Ribbon;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Canvas.Ribbon {
    [ExportRibbonGroup(
        GUID = Constants.RibbonGroup_Edit,
        HeaderLanguageKey = Constants.MenuTabGroupName_Edit,
        Order = 1, 
        ParentGUID = Contracts.Ribbon.Constants.RibbonTab_Tool
    )]
    class EditRibbonGroup : IRibbonGroup {
       
    }
}
