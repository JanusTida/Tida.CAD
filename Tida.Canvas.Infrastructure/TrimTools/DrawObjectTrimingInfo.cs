using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.TrimTools {
    /// <summary>
    /// 进行裁剪所需的信息;
    /// </summary>
    public class DrawObjectTrimingInfo {
        /// <summary>
        /// 将要被裁剪的绘制对象;
        /// </summary>
        public DrawObject TrimedDrawObject { get; set; }

        /// <summary>
        /// 裁剪指定区域;
        /// </summary>
        public Rectangle2D2 TrimArea { get; set; }

        /// <summary>
        /// 用做裁剪基准的对象;
        /// </summary>
        public DrawObject[] TrimingDrawObjects { get; set; }

        /// <summary>
        /// <see cref="TrimedDrawObject"/>与<see cref="TrimingDrawObjects"/>的交点;
        /// </summary>
        public Vector2D[] IntersectPositions { get; set; }
    }
}
