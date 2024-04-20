using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.Snaping.Rules {
    /// <summary>
    /// 判断在线段上的辅助规则;
    /// </summary>
    public class LineSnapOnLineRule : SingleSnapShapeRuleBase<LineBase> {
        protected override ISnapShape MatchSnapShape(LineBase line, Vector2D position, ICanvasContext canvasContext) {
            
            return LineSnapExtensions.GetOnLineSnapShape(line.Line2D, position, canvasContext);
        }



    }
}
