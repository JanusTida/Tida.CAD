using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Line {
    [ExportEditToolProvider(
       GroupGUID = EditToolGroup_Line,
       EditToolLanguageKey = Constants.EditToolName_SingleLine,
       GUID = EditTool_SingleLine,
       IconResource = Constants.EditToolIcon_SingleLine,
       Order = 4,
       Key = System.Windows.Input.Key.L
   )]
    class SingleLineEditToolProvider : EditToolProviderGenericBase<SingleLineEditTool>,IEditToolProvider {
        protected override SingleLineEditTool OnCreateEditTool() {
#if DEBUG
            for (int i = 0; i < 4; i++) {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
#endif

            return new SingleLineEditTool();
        }
    }
}
