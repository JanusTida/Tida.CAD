using Tida.Canvas.Contracts;
using System.Linq;
using Tida.Geometry.Primitives;
using Tida.Geometry.External.Util;
using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Infrastructure.Snaping.Shapes;
using static Tida.Canvas.Infrastructure.Constants;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 椭圆(圆)的辅助规则;
    /// </summary>
    [ExportSnapShapeRule(Order = Constants.SnapRuleOrder_SingleEllipse)]
    public class SingleEllipseSnapPointRule : SingleSnapShapeRuleBase<Ellipse>,ISnapShapeRule {
        /// <summary>
        /// 本规则判断椭圆(圆)的四个顶点,以及圆心与相关Position的关系;
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="position"></param>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        protected override ISnapShape MatchSnapShape(Ellipse ellipse, Vector2D position, ICanvasContext canvasContext) {

            if (ellipse.Ellipse2D == null) {
                return null;
            }

            var screenPosition = canvasContext.CanvasProxy.ToScreen(position);
            var pointsPositions = new Vector2D[] {
                ellipse.Ellipse2D.GetTopPoint(),
                ellipse.Ellipse2D.GetBottomPoint(),
                ellipse.Ellipse2D.GetLeftPoint(),
                ellipse.Ellipse2D.GetRightPoint(),
                ellipse.Ellipse2D.Center
            };

            var screenPoint = new Vector2D();
            foreach (var pointPosition in pointsPositions) {
                canvasContext.CanvasProxy.ToScreen(pointPosition, screenPoint);
                var screenRect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                    screenPoint,
                    TolerantedScreenLength,
                    TolerantedScreenLength
                );

                if (screenRect.Contains(screenPosition)) {
                    return new StandardSnapPoint(pointPosition);
                }
            }

            return null;
        }
    }
}
