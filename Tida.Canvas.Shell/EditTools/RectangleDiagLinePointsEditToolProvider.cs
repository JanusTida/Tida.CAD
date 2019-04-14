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
        GroupGUID = EditToolGroup_Rectangle,
        EditToolLanguageKey = Constants.EditToolName_RectangleDiagLinePoints,
        GUID = EditTool_RectangleDiagLinePoints,
        IconResource = Constants.EditToolIcon_Rectangle,
        Order = 4
    )]
    public class RectangleDiagLinePointsEditToolProvider : EditToolProviderGenericBase<RectangleDiagLinePointsEditTool>,IEditToolProvider {
        protected override RectangleDiagLinePointsEditTool OnCreateEditTool() => new RectangleDiagLinePointsEditTool();
    }
}
