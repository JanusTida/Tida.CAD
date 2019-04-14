using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.EditTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            [ImportMany]IEnumerable<IDrawObjectMoveTool> drawObjectCloneTools
        ) {

            MoveEditTool.DrawObjectMoveTools.Clear();
            MoveEditTool.DrawObjectMoveTools.AddRange(drawObjectCloneTools);
            
        }

        protected override MoveEditTool OnCreateEditTool() => new MoveEditTool();
    }
}
