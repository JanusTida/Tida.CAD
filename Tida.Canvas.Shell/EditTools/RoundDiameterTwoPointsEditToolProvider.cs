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
         EditToolLanguageKey = Constants.EditToolName_RoundDiameterTwoPoints,
         GUID = EditTool_RoundDiameterTwoPoints,
         IconResource = Constants.EditToolIcon_Round,
         Order = 16
     )]
    public class RoundDiameterTwoPointsEditToolProvider : EditToolProviderGenericBase<RoundDiameterTwoPointsEditTool>,IEditToolProvider {
        protected override RoundDiameterTwoPointsEditTool OnCreateEditTool() => new RoundDiameterTwoPointsEditTool();
    }
}
