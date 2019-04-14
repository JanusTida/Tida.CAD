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
       GroupGUID = EditToolGroup_Round,
       EditToolLanguageKey = Constants.EditToolName_RoundCenterRadiusPoints,
       GUID = EditTool_RoundCenterRadiusPoints,
       IconResource = Constants.EditToolIcon_Round,
       Order = 32,
       Key = System.Windows.Input.Key.E
   )]
    public class RoundCenterRadiusPointsEditToolProvider : EditToolProviderGenericBase<RoundCenterRadiusPointsEditTool>,IEditToolProvider {
        protected override RoundCenterRadiusPointsEditTool OnCreateEditTool() => new RoundCenterRadiusPointsEditTool();
    }
}
