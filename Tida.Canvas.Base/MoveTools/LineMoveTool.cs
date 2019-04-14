using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Base.DrawObjects;
using Tida.Geometry.Primitives;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Base.MoveTools {
    [Export(typeof(IDrawObjectMoveTool))]
    public class LineMoveTool : DrawObjectMoveToolBase<Line>, IDrawObjectMoveTool {

        protected override void OnMove(Line line0, Vector2D offset) {
            line0.Line2D += offset;
        }

    }
}
