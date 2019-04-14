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
using System.Windows.Input;
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
        public CopyEditToolProvider([ImportMany]IEnumerable<IDrawObjectMoveTool> drawObjectMoveTools) {
            CopyEditTool.DrawObjectMoveTools.Clear();
            CopyEditTool.DrawObjectMoveTools.AddRange(drawObjectMoveTools);
        }

        protected override CopyEditTool OnCreateEditTool() => new CopyEditTool();
    }
}
