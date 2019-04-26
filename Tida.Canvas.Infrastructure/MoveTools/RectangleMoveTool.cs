using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Infrastructure.MoveTools {
    public class RectangleMoveTool : DrawObjectMoveToolBase<Rectangle> {

        protected override void OnMove(Rectangle drawObject, Vector2D offset) {
            drawObject.Rectangle2D += offset;
        }
        
    }
}
