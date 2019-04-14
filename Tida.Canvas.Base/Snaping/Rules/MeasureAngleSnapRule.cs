using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Infrastructure.Snaping.Shapes;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using static Tida.Canvas.Infrastructure.Utils.PositionHitUtils;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 绘制角的辅助规则;
    /// </summary>
    [ExportSnapShapeRule(Order = Constants.SnapRuleOrder_MeasureAngle)]
    public class MeasureAngleSnapRule : SingleSnapShapeRuleBase<MeasureAngle>, ISnapShapeRule {
        protected override ISnapShape MatchSnapShape(MeasureAngle measureAngle, Vector2D position, ICanvasContext canvasContext) {
            var screenPosition = canvasContext.CanvasProxy.ToScreen(position);

            var startPoint = measureAngle.Start;
            var endPoint = measureAngle.End;
            var middlePoint = measureAngle.Vertex;

            var points = new Vector2D[]{ startPoint, endPoint, middlePoint };
            var screenPoint = new Vector2D();

            if (canvasContext.CanvasProxy != null) {
                var canvasProxy = canvasContext.CanvasProxy;

                foreach (var point in points) {
                    canvasContext.CanvasProxy.ToScreen(point,screenPoint);
                    if (GetIsSurround(screenPosition, screenPoint)) {
                        return new StandardSnapPoint(point);
                    }
                }
            }

            return null;
        }
    }
}
