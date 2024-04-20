using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
    [ExportEditToolProvider(
        GroupGUID = EditToolGroup_BasicEditor,
        EditToolLanguageKey = Constants.EditToolName_Point,
        GUID = EditTool_Point,
        IconResource = Constants.EditToolIcon_Point,
        Key = System.Windows.Input.Key.P,
        Order = 8
    )]
    public class PointEditToolProvider : EditToolProviderGenericBase<PointEditTool>,IEditToolProvider {
        protected override PointEditTool OnCreateEditTool() => new PointEditTool();
    }
}
