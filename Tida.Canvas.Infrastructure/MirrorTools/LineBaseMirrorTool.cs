using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Geometry.Alternation;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.MirrorTools {
    /// <summary>
    /// 适用于<see cref="LineBase"/>的镜像工具;
    /// </summary>
    public class LineBaseMirrorTool : DrawObjectMirroToolBase<LineBase>, IDrawObjectMirrorTool
    {
        protected override void OnMirror(LineBase drawObject, Line2D axis)
        {
            var s = TransformUtil.Mirror(drawObject.Line2D.Start, axis);
            var e= TransformUtil.Mirror(drawObject.Line2D.End, axis);
            drawObject.Line2D = Line2D.Create(s, e);
        }
    }
}
