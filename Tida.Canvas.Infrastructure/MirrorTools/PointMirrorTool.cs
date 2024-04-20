using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Geometry.Alternation;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.MirrorTools {

    public class PointMirrorTool : DrawObjectMirroToolBase<Point>, IDrawObjectMirrorTool
    {
        protected override void OnMirror(Point drawObject, Line2D axis)
        {
            drawObject.Position = TransformUtil.Mirror(drawObject.Position, axis);
        }
    }
}
