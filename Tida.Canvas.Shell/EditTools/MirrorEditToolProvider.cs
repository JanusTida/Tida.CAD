using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools
{
    [ExportEditToolProvider(
        GroupGUID = EditToolGroup_BasicEditor,
        GUID = EditTool_MirrorTool,
         EditToolLanguageKey = Constants.EditToolName_MirrorTool,
        IconResource = Constants.EditToolIcon_Mirror,
        Order = 695
        )]
    class MirrorEditToolProvider : EditToolProviderGenericBase<MirrorEditTool>, IEditToolProvider
    {
        [ImportingConstructor]
        public MirrorEditToolProvider([ImportMany]IEnumerable<IDrawObjectMirrorTool> drawObjectMirrorTools )
        {
            MirrorEditTool.DrawObjectMirrorTools.Clear();
            MirrorEditTool.DrawObjectMirrorTools.AddRange(drawObjectMirrorTools);
        }
        protected override MirrorEditTool OnCreateEditTool() => new MirrorEditTool();
    }
}
