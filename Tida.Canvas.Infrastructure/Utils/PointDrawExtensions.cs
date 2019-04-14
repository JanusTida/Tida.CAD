using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Geometry.External.Util;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 位置的绘制拓展;
    /// </summary>
    public static class PointDrawExtensions {
        /// <summary>
        /// 绘制某位置的选中状态(使用一个矩形);
        /// </summary>
        /// <param name="point"></param>
        public static void DrawSelectedPointState(Vector2D point, ICanvas canvas, ICanvasScreenConvertable canvasProxy) {

            if (point == null) {
                throw new ArgumentNullException(nameof(point));
            }
            
            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }
            
            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }


            //得到以某点为中心的视图矩形;
            var rect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                canvasProxy.ToScreen(point),
                TolerantedScreenLength,
                TolerantedScreenLength
            );

            canvas.NativeDrawRectangle(
                rect,
                HighLightRectColorBrush,
                HighLightLinePen
            );
        }
    }
}
