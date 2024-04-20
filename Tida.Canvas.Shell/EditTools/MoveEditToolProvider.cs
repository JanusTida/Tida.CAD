using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.MoveTools;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
    /// <summary>
    /// 编辑工具——移动;
    /// </summary>
    [ExportEditToolProvider(
        GroupGUID = EditToolGroup_BasicEditor,
        GUID = EditTool_MoveTool,
        EditToolLanguageKey = Constants.EditToolName_MoveTool,
        IconResource = Constants.EditToolIcon_Move,
        Order = 666,
        Key = System.Windows.Input.Key.M
    )]
    public class MoveEditToolProvider : EditToolProviderGenericBase<MoveEditTool>,IEditToolProvider {
        [ImportingConstructor]
        public MoveEditToolProvider(
            [ImportMany]IEnumerable<IDrawObjectMoveTool> drawObjectCloneTools,
            [ImportMany]IEnumerable<IMoveToolsProvider> moveToolsProviders
        ) {

            MoveEditTool.DrawObjectMoveTools.Clear();
            MoveEditTool.DrawObjectMoveTools.AddRange(drawObjectCloneTools);
            MoveEditTool.DrawObjectMoveTools.AddRange(moveToolsProviders.SelectMany(p => p.Tools));
        }

        protected override MoveEditTool OnCreateEditTool() => new MoveEditTool();
    }
}
