using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Infrastructure.MoveTools {
    
    public class PointMoveTool : DrawObjectMoveToolBase<Point> {
        protected override void OnMove(Point point, Vector2D offset) {
            point.Position += offset;
        }

    }
}
