using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWindows = System.Windows;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.WPFCanvas.Geometry {
    /// <summary>
    /// <see cref="Vector2D"/>与<see cref="SysWindows.Point"/>的适配器;
    /// </summary>
    static class Vector2DAdapter {
        /// <summary>
        /// 坐标转化为系统坐标;
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static SysWindows.Point ConvertToSystemPoint(Vector2D vector) {
            if(vector == null) {
                throw new ArgumentNullException(nameof(vector));
            }
            
            return new SysWindows.Point {
                X = vector.X,
                Y = vector.Y
            };
        }

        /// <summary>
        /// 从系统坐标转化为坐标;
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2D ConverterToVector2D(SysWindows.Point point) {
            return new Vector2D {
                X = point.X,
                Y = point.Y
            };
        }
    }
}
