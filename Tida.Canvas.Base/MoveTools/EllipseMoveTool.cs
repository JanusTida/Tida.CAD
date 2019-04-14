using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Base.DrawObjects;
using Tida.Geometry.Primitives;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Base.MoveTools {
    /// <summary>
    /// 椭圆(圆)的移动工具;
    /// </summary>
    [Export(typeof(IDrawObjectMoveTool))]
    public class EllipseMoveTool : DrawObjectMoveToolBase<Ellipse>, IDrawObjectMoveTool {

        protected override void OnMove(Ellipse drawObject, Vector2D offset) {
            drawObject.Ellipse2D += offset;
        }

        
    }
}
