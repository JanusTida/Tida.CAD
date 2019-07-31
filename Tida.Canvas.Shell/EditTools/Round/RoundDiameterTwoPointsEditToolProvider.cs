using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Round {
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
