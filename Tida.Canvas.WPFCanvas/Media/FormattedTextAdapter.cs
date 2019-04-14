using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMedia = System.Windows.Media;
using Tida.Canvas.Media;
using System.Globalization;
using System.Windows;

namespace Tida.Canvas.WPFCanvas.Media {
    /// <summary>
    /// <see cref="FormattedText"/>与<see cref="SystemMedia.FormattedText"/>转化;
    /// </summary>
    public static class FormattedTextAdapter {
        /// <summary>
        /// 绘制文字所需用到的一个参数;
        /// </summary>
        private static readonly SystemMedia.Typeface _typeFace = new SystemMedia.Typeface("微软雅黑");

        public static SystemMedia.FormattedText ConvertToSystemFormattedText(FormattedText formattedText) {

            if (formattedText == null) {
                throw new ArgumentNullException(nameof(formattedText));
            }

            var brush = BrushAdapter.ConvertToSystemBrush(formattedText.ForegroundBlocks.FirstOrDefault()?.Brush);
            var ft = new SystemMedia.FormattedText(
                formattedText.Text, 
                CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight, 
                _typeFace, 
                formattedText.FontSize,
                brush
            );
                
            if(formattedText.ForegroundBlocks != null) {
                foreach (var item in formattedText.ForegroundBlocks) {
                    ft.SetForegroundBrush(BrushAdapter.ConvertToSystemBrush(item.Brush), item.Start, item.Length);
                }
            }

            return ft;
        }
    }
}
