using Tida.Canvas.Contracts;
using System;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.Snaping.Shapes;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 点的辅助规则;
    /// </summary>
    [ExportSnapShapeRule(Order = Constants.SnapRuleOrder_PointSnap)]
    public class PointSnapPointRule : SingleSnapShapeRuleBase<PointBase>, ISnapShapeRule {
       
        /// <summary>
        /// 当某个位置在点附近(以视图为准)时,返回点的绝对位置;
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="position"></param>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        protected override ISnapShape MatchSnapShape(PointBase point, Vector2D position, ICanvasContext canvasContext) {
            if(position == null) {
                throw new ArgumentNullException(nameof(position));
            }


            var screenPosition = canvasContext.CanvasProxy.ToScreen(position);
            var pointScreenPosition = canvasContext.CanvasProxy.ToScreen(point.Position);
            var ellipse = new Ellipse2D(pointScreenPosition, point.ScreenRadius, point.ScreenRadius);

            if (ellipse.Contains(screenPosition)) {
                return new StandardSnapPoint(point.Position);
            }

            return null;
        }
    }
}

