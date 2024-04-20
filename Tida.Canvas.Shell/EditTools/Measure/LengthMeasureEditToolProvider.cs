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
       EditToolLanguageKey = EditToolName_MeasureLength,
       GUID = EditTool_MeasureLength,
       IconResource = EditToolIcon_MeasureLength,
       Order = 699
   )]
    class LengthMeasureEditToolProvider : EditToolProviderGenericBase<LengthMeasureEditTool>, IEditToolProvider {
        protected override LengthMeasureEditTool OnCreateEditTool() {
            return new LengthMeasureEditTool {
                ShouldCommitMeasureData = MeasureSettings.ShouldCommitMeasureData
            };
        }
    }

}
