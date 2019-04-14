using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    /// <summary>
    /// 纯色画刷;
    /// </summary>
    public class SolidColorBrush:Brush {
        public SolidColorBrush() {

        }

        public SolidColorBrush(Color color) {
            this.Color = color;
        }

        private Color _color;
        /// <summary>
        /// 色彩;
        /// </summary>
        public Color Color {
            get => _color;
            set => SetFreezableProperty(ref _color, value);
        }

        /// <summary>
        /// 创建一个冻结的画刷;
        /// </summary>
        /// <returns></returns>
        public static SolidColorBrush CreateFrozenBrush(Color color) {
            var brush = new SolidColorBrush { Color = color };
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// 创建一个冻结的画刷;
        /// </summary>
        /// <param name="argb"></param>
        /// <returns></returns>
        public static SolidColorBrush CreateFrozenBrush(uint argb) {
            var brush = new SolidColorBrush(Color.FromArgb(argb));
            brush.Freeze();
            return brush;
        }
    }
}
