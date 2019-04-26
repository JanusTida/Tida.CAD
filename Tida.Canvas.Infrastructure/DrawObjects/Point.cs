using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 绘制对象——点;
    /// </summary>
    public class Point : PointBase {
        public Point(Vector2D position):base(position) { }
        
        public override DrawObject Clone() => new Point(Position);
    }
}
