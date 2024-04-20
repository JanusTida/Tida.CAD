using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;
using static Tida.Canvas.Shell.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Measure {
    /// <summary>
    /// 编辑工具-测量长度;
    /// </summary>
    [ExportEditToolProvider(
       GroupGUID = EditToolGroup_Measure,
       EditToolLanguageKey = EditToolName_MeasureAngle,
       GUID = EditTool_MeasureAngle,
       IconResource = EditToolIcon_MeasureAngle,
       Order = 720
   )]
    class AngleMeasureEditToolProvider : EditToolProviderGenericBase<AngleMeasureEditTool>, IEditToolProvider {
        protected override AngleMeasureEditTool OnCreateEditTool() {
            return new AngleMeasureEditTool(DrawObjectSelectorService.Current) {
                ShouldCommitMeasureData = MeasureSettings.ShouldCommitMeasureData
            };
        }
    }

}
