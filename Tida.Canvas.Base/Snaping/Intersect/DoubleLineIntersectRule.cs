using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.Snaping.Intersect;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Intersect {
    /// <summary>
    /// 两线段相交交点规则;
    /// </summary>
    [ExportDrawObjectIntersectRule(Order = 2)]
    class DoubleLineIntersectRule : DoubleDrawObjectIntersectRuleBase<LineBase, LineBase>,IDrawObjectIntersectRule {
        protected override Vector2D[] GetIntersectPositions(LineBase line0, LineBase line1,bool extendDrawObject0) {
            Vector2D intersectPointPosition = null;

            if (extendDrawObject0) {
                intersectPointPosition = line0.Line2D.IntersectStraightLine(line1.Line2D);
            }
            else {
                intersectPointPosition = line0.Line2D.Intersect(line1.Line2D);
            }
            
            return intersectPointPosition == null ? null : new Vector2D[] { intersectPointPosition };
        }

    }
}
