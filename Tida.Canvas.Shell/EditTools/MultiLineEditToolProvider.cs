using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
    [ExportEditToolProvider(
      GroupGUID = EditToolGroup_Line,
      EditToolLanguageKey = Constants.EditToolName_MultiLine,
      GUID = EditTool_MultiLine,
      IconResource = Constants.EditToolIcon_MultiLine,
      Order = 8
    )]
    class MultiLineEditToolProvider : EditToolProviderGenericBase<MultiLineEditTool>, IEditToolProvider {
        protected override MultiLineEditTool OnCreateEditTool() {
            return new MultiLineEditTool();
        }
    }
}
