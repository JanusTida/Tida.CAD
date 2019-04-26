using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.MoveTools;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
    [ExportEditToolProvider(
        GroupGUID = EditToolGroup_BasicEditor,
        GUID = EditTool_CopyTool,
        EditToolLanguageKey = Constants.EditToolName_CopyTool,
        IconResource = Constants.EditToolIcon_Copy,
        Order = 688
    )]
    class CopyEditToolProvider : EditToolProviderGenericBase<CopyEditTool>,IEditToolProvider {
        [ImportingConstructor]
        public CopyEditToolProvider(
            [ImportMany]IEnumerable<IDrawObjectMoveTool> drawObjectMoveTools,
            [ImportMany]IEnumerable<IMoveToolsProvider> moveToolsProviders
        ) {

            CopyEditTool.DrawObjectMoveTools.Clear();
            CopyEditTool.DrawObjectMoveTools.AddRange(drawObjectMoveTools);
            CopyEditTool.DrawObjectMoveTools.AddRange(moveToolsProviders.SelectMany(p => p.Tools));
        }

        protected override CopyEditTool OnCreateEditTool() => new CopyEditTool();
    }
}
