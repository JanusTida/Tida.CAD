using System.Windows.Media;
using Tida.CAD.WPF.Extensions;

namespace Tida.CAD.WPF
{

    /// <summary>
    /// 媒体相关常量;
    /// </summary>
    public static partial class Constants {
        /// <summary>
        /// 画布的默认背景色(#ff013939);
        /// </summary>
        public static readonly Brush DefaultCanvasBackground = BrushExtensions.CreateFrozenColorBrush(0xff, 0x1, 0x39, 0x39);

        /// <summary>
        /// 拖放操作中,任意全选的矩形画刷(0x55195B31);
        /// </summary>
        public static readonly Brush AnySelectBrush = BrushExtensions.CreateFrozenColorBrush(0x55195B31);

        /// <summary>
        /// 拖放操作中,任意全选的笔;
        /// </summary>
        public static readonly Pen AnySelectPen = PenExtensions.CreateFrozenPen(Brushes.White, 1);

        /// <summary>
        /// 拖放操作中,全选的矩形画刷(0x55193762);
        /// </summary>
        public static readonly Brush AllSelectBrush = BrushExtensions.CreateFrozenColorBrush(0x55193762);

        /// <summary>
        /// 拖放操作中,全选的笔;
        /// </summary>
        public static readonly Pen AllSelectPen = PenExtensions.CreateFrozenPen(Brushes.White, 1);
    }
}
