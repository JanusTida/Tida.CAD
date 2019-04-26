using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Infrastructure.OffsetTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.NativePresentation;
using Tida.Canvas.Shell.Contracts.OffsetTools;
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
        GUID = EditTool_OffsetTool,
        EditToolLanguageKey = Constants.EditToolName_OffsetTool,
        IconResource = Constants.EditToolIcon_Offset,
        Order = 676
    )]
    class OffsetToolEditToolProvider : EditToolProviderGenericBase<OffsetEditTool2>, IEditToolProvider {
        [ImportingConstructor]
        public OffsetToolEditToolProvider(
            [ImportMany]IEnumerable<IDrawObjectOffsetTool> drawObjectOffsetTools,
            [ImportMany]IEnumerable<IOffsetToolsProvider> offsetToolsProviders) {

            //OffsetEditTool.DrawObjectOffsetTools.AddRange(drawObjectOffsetTools);

            OffsetEditTool2.DrawObjectOffsetTools.AddRange(drawObjectOffsetTools);
            OffsetEditTool2.DrawObjectOffsetTools.AddRange(offsetToolsProviders.SelectMany(p => p.Tools));
        }
        protected override OffsetEditTool2 OnCreateEditTool() => new OffsetEditTool2(NumberBoxService.Current, DrawObjectSelectorService.Current);
    }
}
