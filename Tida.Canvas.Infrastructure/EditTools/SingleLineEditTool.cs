using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Infrastructure.EditTools {

    /// <summary>
    /// 线段(单次)的绘制工具;
    /// </summary>
    public class SingleLineEditTool : SingleLineEditToolGenericBase<Line> {
        protected override Line OnCreateDrawObject(Vector2D lastDownPosition, Vector2D thisMouseDownPosition) {
            return new Line(lastDownPosition, thisMouseDownPosition);
        }
        
    }


}
