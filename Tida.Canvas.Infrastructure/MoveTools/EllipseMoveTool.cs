using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Infrastructure.MoveTools {
    /// <summary>
    /// 椭圆(圆)的移动工具;
    /// </summary>
    public class EllipseMoveTool : DrawObjectMoveToolBase<Ellipse>, IDrawObjectMoveTool {

        protected override void OnMove(Ellipse drawObject, Vector2D offset) {
            drawObject.Ellipse2D += offset;
        }

        
    }
}
