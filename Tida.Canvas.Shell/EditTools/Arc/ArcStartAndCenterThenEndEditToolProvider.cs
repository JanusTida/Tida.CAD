using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Arc {
    [ExportEditToolProvider(
      GroupGUID = EditToolGroup_Arc,
      EditToolLanguageKey = Constants.EditToolName_ArcStartAndCenterThenEnd,
      GUID = EditTool_MultiLine,
      IconResource = Constants.EditToolIcon_MultiLine,
      Order = 8
    )]
    class ArcStartAndCenterThenEndEditToolProvider : EditToolProviderGenericBase<ArcStartAndCenterThenEndEditTool> {
        protected override ArcStartAndCenterThenEndEditTool OnCreateEditTool() {
            return new ArcStartAndCenterThenEndEditTool();
        }
    }
}
