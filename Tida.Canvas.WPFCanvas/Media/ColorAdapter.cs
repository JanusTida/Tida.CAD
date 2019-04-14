using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMedia = System.Windows.Media;
using Tida.Canvas.Media;

namespace Tida.Canvas.WPFCanvas.Media {
    /// <summary>
    /// <see cref="Media.Color"/>与<see cref="SystemMedia.Color"/>的适配器;
    /// </summary>
    public static class ColorAdapter {
        /// <summary>
        /// 从颜色转化为系统颜色;
        /// </summary>
        /// <param name="pen"></param>
        /// <returns></returns>
        public static SystemMedia.Color ConvertToSystemColor(Color color) {
            return new SystemMedia.Color {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }
    }
}
