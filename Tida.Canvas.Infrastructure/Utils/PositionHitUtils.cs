using Tida.Canvas.Contracts;
using Tida.Geometry.External.Util;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 位置<see cref="Vector2D"/>相关;
    /// </summary>
    public static class PositionHitUtils {
        /// <summary>
        /// 判断数学坐标中某个位置是否在另一个位置的附近;
        /// </summary>
        /// <param name="screenPosition0"></param>
        /// <param name="screenPosition1"></param>
        /// <returns></returns>
        public static bool GetIsSurround(Vector2D position0, Vector2D position1,ICanvasScreenConvertable canvasProxy) {

            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }


            if (position0 == null) {
                throw new ArgumentNullException(nameof(position0));
            }


            if (position1 == null) {
                throw new ArgumentNullException(nameof(position1));
            }

            return GetIsSurround(canvasProxy.ToScreen(position0), canvasProxy.ToScreen(position1));
        }

        /// <summary>
        /// 判断视图坐标中某个位置是否在另一个位置的附近;
        /// </summary>
        /// <param name="screenPosition0"></param>
        /// <param name="screenPosition1"></param>
        /// <returns></returns>
        public static bool GetIsSurround(Vector2D screenPosition0, Vector2D screenPosition1) {
            var screenRect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                screenPosition1,
                TolerantedScreenLength,
                TolerantedScreenLength
            );

            return screenRect.Contains(screenPosition0);
        }
        
    }
}
