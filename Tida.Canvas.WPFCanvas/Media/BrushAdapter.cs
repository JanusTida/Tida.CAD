using Tida.Canvas.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMedia = System.Windows.Media;

namespace Tida.Canvas.WPFCanvas.Media {
    /// <summary>
    /// <see cref="Media.Brush"/>与<see cref="SystemMedia.Brush"/>的适配器;
    /// </summary>
    public static class BrushAdapter {
        private static readonly Dictionary<Brush, SystemMedia.Brush> _frozenBrushes = new Dictionary<Brush, SystemMedia.Brush>();
        /// <summary>
        /// 画刷转化为系统画刷的方法;
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        public static SystemMedia.Brush ConvertToSystemBrush(Brush brush) {
            if(brush == null) {
                return null;
            }

            if(_frozenBrushes.TryGetValue(brush,out var sysBrush)) {
                return sysBrush;
            }
            
            var newSystemBrush = CreateBrushCore(brush);
            if(newSystemBrush != null && brush.IsFrozen) {
                newSystemBrush.Freeze();
                _frozenBrushes.Add(brush, newSystemBrush);
            }

            return newSystemBrush;
        }

        private static SystemMedia.Brush CreateBrushCore(Brush brush) {
            if (brush is SolidColorBrush solidBrush) {
                var sysBrush = new SystemMedia.SolidColorBrush(ColorAdapter.ConvertToSystemColor(solidBrush.Color));
                return sysBrush;
            }

            return null;
        }

        
    }
}
