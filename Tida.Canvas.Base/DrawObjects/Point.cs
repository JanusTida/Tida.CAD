using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Base.DrawObjects {
    /// <summary>
    /// 绘制对象——点;
    /// </summary>
    public class Point : PointBase {
        public Point(Vector2D position):base(position) { }
        
        public override DrawObject Clone() => new Point(Position);
    }
}
