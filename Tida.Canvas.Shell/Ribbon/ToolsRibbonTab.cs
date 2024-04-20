using Tida.Canvas.Shell.Contracts.Ribbon;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Ribbon {
    /// <summary>
    /// RibbonTab-工具;
    /// </summary>
    [ExportRibbonTab(
        GUID = Contracts.Ribbon.Constants.RibbonTab_Tool,
        Order = Contracts.Ribbon.Constants.RibbonTabOrder_Tools,
        TextLangaugeKey = Constants.RibbonTabName_Tools
    )]
    class ToolsRibbonTab : IRibbonTab {
        
    }


}
