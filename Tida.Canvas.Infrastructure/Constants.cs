using Tida.Canvas.Media;

namespace Tida.Canvas.Infrastructure {

    /// <summary>
    /// 媒体相关(画刷,笔,长度，字体等);
    /// </summary>
    public static partial class Constants {

        /// <summary>
        /// 线段的默认笔;
        /// </summary>
        public static readonly Pen LinePen = Pen.CreateFrozenPen(Brushes.White, 1.2);


        public static readonly Pen MiddleLinePen = Pen.CreateFrozenPen(Brushes.Red, 1);

        public static readonly Brush HighLightBrush = Brushes.DodgerBlue;

        public static readonly Pen HighLightLinePen = Pen.CreateFrozenPen(HighLightBrush, 1);

        public static readonly Pen HilightLinePen = HighLightLinePen;

        /// <summary>
        /// 鼠标上次按下位置到鼠标当前位置的辅助线的笔;
        /// </summary>
        public static readonly Pen LastMouseDownToCurrentMouseLinePen = Pen.CreateFrozenPen(
            new SolidColorBrush {
                Color = Color.FromRgb(0xDCC590)
            },1.0
        );

        /// <summary>
        /// 用于表示绘制对象正在编辑中的方块的画刷;
        /// </summary>
        public static readonly Brush NodeBrush = new SolidColorBrush {
            Color = Color.FromRgb(0xFFFFFF)
        };

        /// <summary>
        /// 高亮的方块边长/填充画刷,将用于编辑,辅助;
        /// </summary>
        public const double HighLightRectLength = 8.0D;
        public static readonly Brush HighLightRectColorBrush = new SolidColorBrush {
            Color = Color.FromRgb(0x007FFF)
        };

        #region 椭圆(圆)相关
        public static readonly Brush NormalEllipseColorBrush = null;
        //new SolidColorBrush {
        //    Color = new Color {
        //        A = 0xFF,
        //        R = 0xC9,
        //        G = 0xE9,
        //        B = 0xF8
        //    }
        //};

        public static readonly Pen NormalEllipsePen = Pen.CreateFrozenPen(Brushes.White, 1);

        public static readonly Brush HighLightEllipseColorBrush = SolidColorBrush.CreateFrozenBrush(Color.FromRgb(0x007Fff));

        #endregion

        #region 矩形相关;
        public static readonly Brush NormalRectColorBrush = null;

        public static readonly Pen NormalRectPen = Pen.CreateFrozenPen(SolidColorBrush.CreateFrozenBrush(Color.FromRgb(0xFFFFFF)), 1);



        #endregion


        /// <summary>
        /// 辅助默认误差允许的范围(使用此常量时，应以视图坐标为准);
        /// </summary>
        public const double TolerantedScreenLength = 8.0D;


        /// <summary>
        /// 表示点的圆的视图半径;
        /// </summary>
        public const double PointEllipseScreenRadius = 4.0D;



        /// <summary>
        /// 线段在编辑之时,显示长度的文字的大小;
        /// </summary>
        public const double LineEditingSnappingLengthFontSize = 12;


        /// <summary>
        /// 编辑中线段提示状态与原线段的垂直距离;
        /// </summary>
        public const double ScreenDistanceLineEditingWithLine = 36;


        /// <summary>
        /// 线段在编辑之时,显示提示的颜色;
        /// </summary>
        public static readonly Brush LineEditingTipBrush = new SolidColorBrush { Color = Color.FromRgb(0xFFFFFF) };

        public static readonly Pen LineEditingTipPen = Pen.CreateFrozenPen(
            SolidColorBrush.CreateFrozenBrush(Color.FromRgb(0xFFFFFF)),
            1.2, 
            new DashStyle {
                Dashes = new double[] { 5, 2.5 }
            }
        );


        /// <summary>
        /// 关注点在线段上时的辅助点背景;
        /// </summary>
        public static readonly Brush InLineSnapPointBackground = SolidColorBrush.CreateFrozenBrush(Color.FromArgb(0x00FFFFFF));


        public static readonly Pen IntersectPen = Pen.CreateFrozenPen(SolidColorBrush.CreateFrozenBrush(Color.FromRgb(0x00A500)), 1.2);

        /// <summary>
        /// 极轴颜色;
        /// </summary>
        public static readonly Pen RayPen = Pen.CreateFrozenPen(SolidColorBrush.CreateFrozenBrush(Color.FromRgb(0xFFFFFF)), 1.2);

        /// <summary>
        /// 测量长度的笔;
        /// </summary>
        public static readonly Pen MeasureLengthPen = Pen.CreateFrozenPen(Brushes.Crimson, 2);

        /// <summary>
        /// 测量长度的笔;
        /// </summary>
        public static readonly Pen MeasureArcPen = Pen.CreateFrozenPen(Brushes.Crimson, 2);

        public static readonly Brush TextForeground_LengthMeasurement = Brushes.Crimson;

        public static readonly Brush TextForeground_AngleMeasurement = Brushes.Crimson;
    }

    /// <summary>
    /// 图层相关常量;
    /// </summary>
    public static partial class Constants {
        //默认图层;
        public const string CanvasLayerGUID_Default = nameof(CanvasLayerGUID_Default);

        /// <summary>
        /// 编辑组-基本处理(复制、移动等);
        /// </summary>
        public const string EditToolGroup_EditBase = nameof(EditToolGroup_EditBase);

    }
}
