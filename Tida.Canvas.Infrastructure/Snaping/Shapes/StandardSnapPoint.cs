using Tida.Canvas.Contracts;
using System;
using Tida.Geometry.Primitives;
using Tida.Canvas.Media;
using Tida.Canvas.Infrastructure.Utils;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Snaping.Shapes {
    /// <summary>
    /// 标准辅助点实现;
    /// </summary>
    public class StandardSnapPoint : SnapShapeBase {
        public StandardSnapPoint(Vector2D position) {
            if(position == null) {
                throw new ArgumentNullException(nameof(position));
            }
            //if(owner == null) {
            //    throw new ArgumentNullException(nameof(owner));
            //}
            this.Position = position;
            //this.Owner = owner;
        }

        public override Rectangle2D2 GetNativeBoundingRect(ICanvasScreenConvertable canvasProxy) {
            if(canvasProxy == null) {
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
        
        public override void Draw(ICanvas canvas,ICanvasScreenConvertable canvasProxy) {
            var screenPosition = canvasProxy.ToScreen(Position);
            var rectLength = HighLightRectLength;
            var rect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(screenPosition,rectLength ,rectLength);
            canvas.NativeDrawRectangle(rect,Background , LinePen);
        }

        /// <summary>
        /// 背景色;
        /// </summary>
        public Brush Background { get; set; } = HighLightRectColorBrush;
    }
}
