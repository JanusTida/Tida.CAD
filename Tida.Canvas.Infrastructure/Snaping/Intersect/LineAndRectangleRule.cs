using Tida.Geometry.Primitives;
using System.Linq;
using Tida.Canvas.Infrastructure.Snaping.Intersect;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Infrastructure.Snaping.Intersect {
    /// <summary>
    /// 线段与矩形相交规则;
    /// </summary>
    public class LineAndRectangleRule : DoubleDrawObjectIntersectRuleBase<Line, Rectangle> {
        protected override Vector2D[] GetIntersectPositions(Line line, Rectangle rectangle, bool extendDrawObject0) {
            if(line.Line2D == null) {
                return null;
            }

            var lines = rectangle.Rectangle2D.GetLines();
            if(lines == null) {
                return null;
            }
            return lines.Select(p => p.Intersect(line.Line2D)).Where(p => p != null).
                Distinct(Vector2DEqualityComparer.StaticInstance).ToArray();
        }
    }
}
