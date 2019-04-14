using Tida.Canvas.Base.EditTools;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.EditTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;
using static Tida.Canvas.Shell.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
    /// <summary>
    /// 编辑工具组——基本图形;
    /// </summary>
    [Export(typeof(IEditToolGroup))]
    class MeasureEditToolGroup : IEditToolGroup {
        public string GUID => EditToolGroup_Measure;

        public string ParentGUID => null;

        public string GroupNameLanguageKey => EditToolGroupName_Measure;

        public int Order => EditToolGroupOrder_Measure;

        public string Icon => null;
    }

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
            return new AngleMeasureEditTool {
                ShouldCommitMeasureData = MeasureSettings.ShouldCommitMeasureData
            };
        }
    }

    /// <summary>
    /// 测量相关设定;
    /// </summary>
    static class MeasureSettings {
        private static bool _shouldCommitMeasureData;
        public static bool ShouldCommitMeasureData {
            get => _shouldCommitMeasureData;
            set {
                if(_shouldCommitMeasureData == value) {
                    return;
                }

                _shouldCommitMeasureData = value;
                if(CanvasService.Current.CanvasDataContext?.CurrentEditTool is LengthMeasureEditTool lmEditTool) {
                    lmEditTool.ShouldCommitMeasureData = value;
                }

                if (CanvasService.Current.CanvasDataContext?.CurrentEditTool is AngleMeasureEditTool amEditTool) {
                    amEditTool.ShouldCommitMeasureData = value;
                }

            }
        }
    }
}
