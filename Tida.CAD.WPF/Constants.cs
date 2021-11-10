using System.Windows.Media;
using Tida.CAD.WPF.Extensions;

namespace Tida.CAD.WPF
{

    /// <summary>
    /// Some constants for wpf cadcontrol;
    /// </summary>
    public static partial class Constants {
        /// <summary>
        /// The default background for cad control(#ff013939);
        /// </summary>
        public static readonly Brush DefaultCanvasBackground = BrushExtensions.CreateFrozenColorBrush(0xff, 0x1, 0x39, 0x39);

        /// <summary>
        /// The brush used to show "any selection" state(0x55195B31);
        /// </summary>
        public static readonly Brush AnySelectBrush = BrushExtensions.CreateFrozenColorBrush(0x55195B31);

        /// <summary>
        /// The pen used to show "any selection" state(white);
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
