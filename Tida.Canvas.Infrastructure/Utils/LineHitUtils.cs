using Tida.Canvas.Contracts;
using Tida.Geometry.External.Util;
using Tida.Geometry.Primitives;
using System.Linq;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 线段画布判定相关;
    /// </summary>
    public static class LineHitUtils {

        /// <summary>
        /// 判断画布中某个位置是否在某个线段附近;
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="point"></param>
        /// <param name="canvasProxy"></param>
        /// <returns></returns>
        public static bool PointInLine(Line2D line2D, Vector2D point, ICanvasScreenConvertable canvasProxy) {
            if (canvasProxy == null) {
                return false;
            }

            if (line2D == null) {
                return false;
            }

            //以视图单位为标准进行判断;
            var screenPoint = canvasProxy.ToScreen(point);
            var screenLine2D = new Line2D(
                canvasProxy.ToScreen(line2D.Start),
                canvasProxy.ToScreen(line2D.End)
            );
            return screenLine2D.Distance(screenPoint) < TolerantedScreenLength;
        }

        /// <summary>
        /// 判断某个矩形与某个线段的包含或相交(当<paramref name="anyPoint"/>为真时)关系;
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="rect"></param>
        /// <param name="anyPoint"></param>
        /// <returns></returns>
        public static bool LineInRectangle(Line2D line2D, Rectangle2D2 rect, bool anyPoint) {
            if (line2D == null) {
                return false;
            }

            //若在完全在矩形内部,则返回为真;
            if (rect.Contains(line2D.Start)
                && rect.Contains(line2D.End)) {
                return true;
            }
            //若任意可选为真,则矩形任意一条边与之存在相交即可;
            else if (anyPoint) {
                return rect.GetLines()?.Any(p => p.Intersect(line2D) != null) ?? false;
            }
            return false;
        }
    }
}
