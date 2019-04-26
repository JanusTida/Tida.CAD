using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 绘制对象-线段;
    /// </summary>
    public class Line: LineBase {
        public Line(Vector2D start, Vector2D end) : base(start, end) { }
        public Line(Line2D line2D) : base(line2D) { }

        public override DrawObject Clone() => new Line(Line2D);
    }
}
