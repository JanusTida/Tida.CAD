using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.MirrorTools;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;
using System.Linq;

namespace Tida.Canvas.Shell.EditTools {
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
        public MirrorEditToolProvider(
            [ImportMany]IEnumerable<IDrawObjectMirrorTool> drawObjectMirrorTools,
            [ImportMany]IEnumerable<IMirrorToolProvider> mirrorToolProviders)
        {
            MirrorEditTool.DrawObjectMirrorTools.Clear();
            MirrorEditTool.DrawObjectMirrorTools.AddRange(drawObjectMirrorTools);
            MirrorEditTool.DrawObjectMirrorTools.AddRange(mirrorToolProviders.SelectMany(p => p.Tools));
        }
        protected override MirrorEditTool OnCreateEditTool() => new MirrorEditTool();
    }
}
