using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Base.DrawObjects;
using Tida.Geometry.Primitives;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Base.MoveTools {
    [Export(typeof(IDrawObjectMoveTool))]
    public class PointMoveTool : DrawObjectMoveToolBase<Point> {
        protected override void OnMove(Point point, Vector2D offset) {
            point.Position += offset;
        }

    }
}
