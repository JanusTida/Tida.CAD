using Tida.Canvas.Contracts;
using System;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.Utils;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Snaping.Shapes {
    /// <summary>
    /// 表示相交的辅助点;
    /// </summary>
    public class IntersectSnapPoint : SnapShapeBase {
        /// <summary>
        /// 相交辅助节点的构建方式;
        /// </summary>
        /// <param name="position">相交的坐标</param>
        public IntersectSnapPoint(Vector2D position) {
            if(position == null) {
                throw new ArgumentNullException(nameof(position));
            }
            
            Position = position;
        }
        
        /// <summary>
        /// 交点绘制;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if(canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }

            if(canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            //画叉;
            var screenPosition = canvasProxy.ToScreen(Position);
            var rectLength = HighLightRectLength;
            //从左上到右下;
            var line1 = new Line2D(
                new Vector2D(screenPosition.X - rectLength, screenPosition.Y - rectLength),
                new Vector2D(screenPosition.X + rectLength, screenPosition.Y + rectLength)
            );
            //从左下到右上;
            var line2 = new Line2D(
                new Vector2D(screenPosition.X - rectLength,screenPosition.Y + rectLength),
                new Vector2D(screenPosition.X + rectLength,screenPosition.Y - rectLength)
            );

            canvas.NativeDrawLine(IntersectPen,line1);
            canvas.NativeDrawLine(IntersectPen, line2);
        }

        public override Rectangle2D2 GetNativeBoundingRect(ICanvasScreenConvertable canvasProxy) {
            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }


            var screenPosition = canvasProxy.ToScreen(Position);
            var rect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                screenPosition,
                HighLightRectLength,
                HighLightRectLength
            );


            return rect;
        }


    }
}
