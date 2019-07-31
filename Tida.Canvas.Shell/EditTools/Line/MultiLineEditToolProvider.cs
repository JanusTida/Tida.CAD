using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Line {
    [ExportEditToolProvider(
      GroupGUID = EditToolGroup_Line,
      EditToolLanguageKey = Constants.EditToolName_MultiLine,
      GUID = EditTool_ArcStartAndCenterThenEnd,
      IconResource = Constants.EditToolGroupName_Arc,
      Order = 8
    )]
    class MultiLineEditToolProvider : EditToolProviderGenericBase<MultiLineEditTool>, IEditToolProvider {
        protected override MultiLineEditTool OnCreateEditTool() {
            return new MultiLineEditTool();
        }
    }
}
