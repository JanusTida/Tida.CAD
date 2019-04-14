using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Infrastructure.ExtendTools;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Infrastructure.TrimTools;
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
    [ExportEditToolProvider(
        GroupGUID = EditToolGroup_BasicEditor,
        GUID = EditTool_TrimTool,
        EditToolLanguageKey = Constants.EditToolName_TrimTool,
        IconResource = Constants.EditToolIcon_Trim,
        Order = 699,
        Key = System.Windows.Input.Key.T
    )]
    public class TrimEditToolProvider : EditToolProviderGenericBase<TrimEditTool>,IEditToolProvider {
        [ImportingConstructor]
        public TrimEditToolProvider(
            [ImportMany]IEnumerable<IDrawObjectIntersectRule> drawObjectIntersectRules,
            [ImportMany]IEnumerable<IDrawObjectTrimTool> drawObjectTrimTools,
            [ImportMany]IEnumerable<IDrawObjectExtendTool> drawObjectExtendTools
        ) {
            TrimEditTool.DrawObjectIntersectRules.Clear();
            TrimEditTool.DrawObjectIntersectRules.AddRange(drawObjectIntersectRules);

            TrimEditTool.DrawObjectTrimTools.Clear();
            TrimEditTool.DrawObjectTrimTools.AddRange(drawObjectTrimTools);

            TrimEditTool.DrawObjectExtendTools.Clear();
            TrimEditTool.DrawObjectExtendTools.AddRange(drawObjectExtendTools);
        }

        protected override TrimEditTool OnCreateEditTool() => new TrimEditTool();
    }
}
