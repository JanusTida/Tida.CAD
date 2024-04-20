using Tida.Geometry.Primitives;
using System;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.Utils;

namespace Tida.Canvas.Infrastructure.Snaping.Shapes {

    /// <summary>
    /// 辅助点与某线段相关的辅助图形;
    /// </summary>
    public class SnapShapeForLine : StandardSnapPoint {
        public SnapShapeForLine(Vector2D position,Line2D relatedLine2D):base(position) {

            if (relatedLine2D == null) {
                throw new ArgumentNullException(nameof(relatedLine2D));
            }

            this.RelatedLine2D = relatedLine2D;
        }

        /// <summary>
        /// 与之相关的线段几何;
        /// </summary>
        public Line2D RelatedLine2D { get; }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            //绘制与RelatedLine2D两个端点的关系;
            var lineWithStart = new Line2D(RelatedLine2D.Start, Position);
            var lineWidthEnd = new Line2D(Position, RelatedLine2D.End);

            LineDrawExtensions.DrawEditingLineOutLines(canvas, canvasProxy, lineWithStart);
            LineDrawExtensions.DrawEditingLineLengthString(canvas, canvasProxy, lineWithStart);

            LineDrawExtensions.DrawEditingLineOutLines(canvas, canvasProxy, lineWidthEnd);
            LineDrawExtensions.DrawEditingLineLengthString(canvas, canvasProxy, lineWidthEnd);
            
            base.Draw(canvas, canvasProxy);
        }
    }
}
