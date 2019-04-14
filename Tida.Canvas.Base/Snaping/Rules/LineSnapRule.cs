using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 线段的辅助规则;
    /// 依次判断以下情况:1.中点 2.端点 
    /// </summary>
    [ExportSnapShapeRule(Order = Constants.SnapRuleOrder_LineSnap)]
    public class LineSnapRule : SingleSnapShapeRuleBase<LineBase>,ISnapShapeRule {
        /// <summary>
        /// 判断某线段与关注点的辅助命中;
        /// </summary>
        /// <param name="line"></param>
        /// <param name="position"></param>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        protected override ISnapShape MatchSnapShape(LineBase line, Vector2D position, ICanvasContext canvasContext) {
            return LineSnapExtensions.GetLine2DSnapShape(line.Line2D, position, canvasContext.CanvasProxy);
        }

    }
}
