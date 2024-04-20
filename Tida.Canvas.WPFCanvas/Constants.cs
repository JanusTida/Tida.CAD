using Tida.Canvas.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.WPFCanvas {
    
    /// <summary>
    /// 媒体相关常量;
    /// </summary>
    public static partial class Constants {
        /// <summary>
        /// 画布的默认背景色;
        /// </summary>
        public static readonly System.Windows.Media.Brush 
            DefaultCanvasBackground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff, 0x1, 0x39, 0x39));

        /// <summary>
        /// 拖放操作中,任意全选的矩形画刷;
        /// </summary>
        public static readonly Brush AnySelectBrush = new SolidColorBrush {
            Color = new Color {
                A = 0x55,
                R = 0x19,
                G = 0x5B,
                B = 0x31
            }
        };

        /// <summary>
        /// 拖放操作中,任意全选的笔;
        /// </summary>
        public static readonly Pen AnySelectPen = Pen.CreateFrozenPen(Brushes.White, 1);

        /// <summary>
        /// 拖放操作中,全选的矩形画刷;
        /// </summary>
        public static readonly Brush AllSelectBrush = SolidColorBrush.CreateFrozenBrush(0x55193762);

        /// <summary>
        /// 拖放操作中,全选的笔;
        /// </summary>
        public static readonly Pen AllSelectPen = Pen.CreateFrozenPen(Brushes.White, 1);
    }
}
