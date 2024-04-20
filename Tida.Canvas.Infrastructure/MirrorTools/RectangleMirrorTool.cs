using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Geometry.Alternation;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.MirrorTools {
    public class RectangleMirrorTool : DrawObjectMirroToolBase<Rectangle>
    {
        protected override void OnMirror(Rectangle drawObject, Line2D axis)
        {
            var s = TransformUtil.Mirror(drawObject.Rectangle2D.MiddleLine2D.Start, axis);
            var e = TransformUtil.Mirror(drawObject.Rectangle2D.MiddleLine2D.End, axis);
            drawObject.Rectangle2D = new Rectangle2D2(Line2D.Create(s, e), drawObject.Rectangle2D.Width);
        }
    }
}
