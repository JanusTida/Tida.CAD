using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Infrastructure.ExtendTools;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Infrastructure.TrimTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.ExtendTools;
using Tida.Canvas.Shell.Contracts.Snaping;
using Tida.Canvas.Shell.Contracts.TrimTools;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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
            [ImportMany]IEnumerable<IIntersectRuleProvider> intersectRuleProviders,
            [ImportMany]IEnumerable<IDrawObjectTrimTool> drawObjectTrimTools,
            [ImportMany]IEnumerable<ITrimToolsProvider> trimToolsProviders,

            [ImportMany]IEnumerable<IDrawObjectExtendTool> drawObjectExtendTools,
            [ImportMany]IEnumerable<IExtendToolsProvider> extendToolsProviders
        ) {
            TrimEditTool.DrawObjectIntersectRules.Clear();
            TrimEditTool.DrawObjectIntersectRules.AddRange(drawObjectIntersectRules);
            TrimEditTool.DrawObjectIntersectRules.AddRange(intersectRuleProviders.SelectMany(p => p.Rules));

            TrimEditTool.DrawObjectTrimTools.Clear();
            TrimEditTool.DrawObjectTrimTools.AddRange(drawObjectTrimTools);
            TrimEditTool.DrawObjectTrimTools.AddRange(trimToolsProviders.SelectMany(p => p.Tools));

            TrimEditTool.DrawObjectExtendTools.Clear();
            TrimEditTool.DrawObjectExtendTools.AddRange(drawObjectExtendTools);
            TrimEditTool.DrawObjectExtendTools.AddRange(extendToolsProviders.SelectMany(p => p.Tools));
        }

        protected override TrimEditTool OnCreateEditTool() => new TrimEditTool();
    }
}
