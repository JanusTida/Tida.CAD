using Tida.Canvas.Base.DrawObjects;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.Snaping.Intersect;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Intersect {
    /// <summary>
    /// 线段与椭圆相交规则;
    /// </summary>
    [ExportDrawObjectIntersectRule(Order = 2)]
    class LineAndEllipseIntersectRule : DoubleDrawObjectIntersectRuleBase<Line, Ellipse> {
        protected override Vector2D[] GetIntersectPositions(Line line, Ellipse ellipse, bool extendLine) {
            if(ellipse.Ellipse2D == null) {
                return null;
            }

            if(line.Line2D == null) {
                return null;
            }

            if (extendLine) {
                return ellipse.Ellipse2D.IntersectWithStraightLine(line.Line2D);
            }
            else {
                return ellipse.Ellipse2D.IntersectWithLine(line.Line2D, false);
            }
            
        }
    }
}
