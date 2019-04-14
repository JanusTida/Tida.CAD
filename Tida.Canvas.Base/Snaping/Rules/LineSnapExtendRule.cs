using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.Snaping.Shapes;
using Tida.Canvas.Contracts;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using static Tida.Canvas.Infrastructure.Constants;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 线段拓展辅助;
    /// </summary>
    [ExportSnapShapeRule(Order = Constants.SnapRuleOrder_ExtendLine)]
    public class LineSnapExtendRule : SingleSnapShapeRuleBase<LineBase> {
        protected override ISnapShape MatchSnapShape(LineBase line, Vector2D position, ICanvasContext canvasContext) {
            if (!line.IsEditing) {
                return null;
            }

            var screenPosition = canvasContext.CanvasProxy.ToScreen(position);

            /*判断关注点是否"在线段的延长线上" */
            
            var screenLine2D = new Line2D(
                canvasContext.CanvasProxy.ToScreen(line.Line2D.Start),
                canvasContext.CanvasProxy.ToScreen(line.Line2D.End)
            );


            //确定该点不"在"该线段上;
            if (screenLine2D.Distance(screenPosition) <= TolerantedScreenLength) {
                return null;
            }

            /*确定该点在该线段的延长线上;*/

            var projectScreenPosition = screenPosition.ProjectOn(screenLine2D);
            if(projectScreenPosition == null) {
                return null;
            }

            if(projectScreenPosition.Distance(screenPosition) >= TolerantedScreenLength) {
                return null;
            }

            //寻找距离投影点最近的一个端点作为延伸辅助线起始点;
            var screenStart = canvasContext.CanvasProxy.ToScreen(line.Line2D.Start);
            var screenEnd = canvasContext.CanvasProxy.ToScreen(line.Line2D.End);

            var closestPosition = screenStart.Distance(projectScreenPosition) < screenEnd.Distance(projectScreenPosition) ? line.Line2D.Start :line.Line2D.End;
            var projectPosition = canvasContext.CanvasProxy.ToUnit(projectScreenPosition);
            return new LineSnapShape(new Line2D(closestPosition, projectPosition),projectPosition);
        }
    }
}
