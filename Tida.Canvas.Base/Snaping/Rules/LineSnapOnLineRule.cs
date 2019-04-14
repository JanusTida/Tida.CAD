using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using Tida.Canvas.Shell.Contracts.Snaping;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 判断在线段上的辅助规则;
    /// </summary>
    [ExportSnapShapeRule(Order = Constants.SnapRuleOrder_OnLine)]
    public class LineSnapOnLineRule : SingleSnapShapeRuleBase<LineBase> {
        protected override ISnapShape MatchSnapShape(LineBase line, Vector2D position, ICanvasContext canvasContext) {
            
            return LineSnapExtensions.GetOnLineSnapShape(line.Line2D, position, canvasContext);
        }



    }
}
