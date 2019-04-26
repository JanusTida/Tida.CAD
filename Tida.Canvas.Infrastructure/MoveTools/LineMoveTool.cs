using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Infrastructure.MoveTools {
    public class LineMoveTool : DrawObjectMoveToolBase<Line>, IDrawObjectMoveTool {

        protected override void OnMove(Line line0, Vector2D offset) {
            line0.Line2D += offset;
        }

    }
}
