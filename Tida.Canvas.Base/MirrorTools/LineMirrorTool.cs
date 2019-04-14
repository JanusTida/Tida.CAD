using System.ComponentModel.Composition;
using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Geometry.Alternation;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Base.MirrorTools {
    [Export(typeof(IDrawObjectMirrorTool))]
    public class LineMirrorTool : DrawObjectMirroToolBase<Line>, IDrawObjectMirrorTool
    {
        protected override void OnMirror(Line drawObject, Line2D axis)
        {
            var s = TransformUtil.Mirror(drawObject.Line2D.Start, axis);
            var e= TransformUtil.Mirror(drawObject.Line2D.End, axis);
            drawObject.Line2D = Line2D.Create(s, e);
        }
    }
}
