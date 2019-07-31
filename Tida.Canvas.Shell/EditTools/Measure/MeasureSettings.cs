using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.Canvas;

namespace Tida.Canvas.Shell.EditTools.Measure {
    /// <summary>
    /// 测量相关设定;
    /// </summary>
    static class MeasureSettings {
        private static bool _shouldCommitMeasureData;
        /// <summary>
        /// 测量操作完成后是否保存测量对象;
        /// </summary>
        public static bool ShouldCommitMeasureData {
            get => _shouldCommitMeasureData;
            set {
                if (_shouldCommitMeasureData == value) {
                    return;
                }

                _shouldCommitMeasureData = value;
                if (CanvasService.Current.CanvasDataContext?.CurrentEditTool is IMeasureEditTool measureEditTool) {
                    measureEditTool.ShouldCommitMeasureData = value;
                }
            }
        }
        
    }
}
