using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysWindows = System.Windows;
using CDO.Common.Geometry.Primitives;

namespace CDO.Common.WPFCanvas.Geometry {
    /// <summary>
    /// <see cref="CDO.Common.Geometry.Primitives.Rectangle2D"/>与<see cref="System.Windows.Rect"/>的适配器;
    /// </summary>
    static class Rect2DAdapter {
        /// <summary>
        ///矩形转化为系统矩形;
        /// </summary>
        /// <param name="vector"></param>
        /// <remarks>这将忽略掉矩形倾角信息</remarks>
        /// <returns></returns>
        public static SysWindows.Rect ConvertToSystemPoint(Rectangle2D vector) {

        }
    }
}
