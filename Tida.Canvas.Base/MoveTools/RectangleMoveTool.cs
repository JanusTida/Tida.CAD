using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Base.DrawObjects;
using Tida.Geometry.Primitives;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Base.MoveTools {
    [Export(typeof(IDrawObjectMoveTool))]
    public class RectangleMoveTool : DrawObjectMoveToolBase<Rectangle> {

        protected override void OnMove(Rectangle drawObject, Vector2D offset) {
            drawObject.Rectangle2D += offset;
        }
        
    }
}
