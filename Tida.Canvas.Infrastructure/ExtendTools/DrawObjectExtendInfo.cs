using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;


namespace Tida.Canvas.Infrastructure.ExtendTools {
    /// <summary>
    /// 进行延伸所需的信息;
    /// </summary>
    public class DrawObjectExtendInfo {
        /// <summary>
        /// 将要被延伸的绘制对象;
        /// </summary>
        public DrawObject ExtendedDrawObject { get; set; }

        /// <summary>
        /// 延伸指定区域;
        /// </summary>
        public Rectangle2D2 ExtendArea { get; set; }

        /// <summary>
        /// 用做延伸基准的对象;
        /// </summary>
        public DrawObject[] ExtendingDrawObjects { get; set; }

        /// <summary>
        /// <see cref="ExtendedDrawObject"/>与<see cref="ExtendingDrawObjects"/>的交点;
        /// </summary>
        public Vector2D[] IntersectPositions { get; set; }
    }
}
