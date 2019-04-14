using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.EditTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
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
