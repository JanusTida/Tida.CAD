using System.ComponentModel.Composition;
using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Geometry.Alternation;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Base.MirrorTools {
    [Export(typeof(IDrawObjectMirrorTool))]
    public class EllipseMirrorTool : DrawObjectMirroToolBase<Ellipse>, IDrawObjectMirrorTool
    {
        protected override void OnMirror(Ellipse drawObject, Line2D axis)
        {
            var ellipse = drawObject.Ellipse2D;
            var center = TransformUtil.Mirror(ellipse.Center, axis);
            drawObject.Ellipse2D = new Ellipse2D(center, ellipse.RadiusX, ellipse.RadiusY);
        }
    }
}
