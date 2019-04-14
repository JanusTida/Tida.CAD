using Tida.Canvas.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMedia = System.Windows.Media;

namespace Tida.Canvas.WPFCanvas.Media {
    /// <summary>
    /// <see cref="DashStyle"/>与<see cref="SystemMedia.DashStyle"/>的适配器;
    /// </summary>
    public static class DashStyleAdapter {
        /// <summary>
        /// DashStyle转化为系统DashStyle的方法;
        /// </summary>
        /// <param name="dashStyle"></param>
        /// <returns></returns>
        public static SystemMedia.DashStyle ConvertToSystemDashStyle(DashStyle dashStyle) {

            if (dashStyle == null) {
                throw new ArgumentNullException(nameof(dashStyle));
            }

            return new SystemMedia.DashStyle {
                Offset = dashStyle.Offset,
                Dashes = new SystemMedia.DoubleCollection(dashStyle.Dashes)
            };
            
        }
    }
}
