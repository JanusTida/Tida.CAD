using Tida.Canvas.Contracts;
using System;
using System.Linq;
using Tida.Geometry.Primitives;
using Tida.Geometry.External.Util;
using Tida.Canvas.Infrastructure.Utils;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Base.DrawObjects {
    /// <summary>
    /// 绘制对象——矩形;
    /// </summary>
    public class Rectangle : DrawObject {
        public Rectangle(Rectangle2D2 rectangle2D) {
            if(rectangle2D == null) {
                throw new ArgumentNullException(nameof(rectangle2D));
            }
            this.Rectangle2D = rectangle2D;
        }
        private Rectangle2D2 _rectangle2D;

        public Rectangle2D2 Rectangle2D {
            get { return _rectangle2D; }
            set {
                if(value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                var oldValue = _rectangle2D;
                var newValue = value;
                SetRetangle2DCore(newValue);
                //呈递事务;
                RaiseEditTransActionCommited(new StandardEditTransaction(
                    () => SetRetangle2DCore(oldValue),
                    () => SetRetangle2DCore(newValue)
                ));
            }
        }

        /// <summary>
        /// 设定矩形数据核心;
        /// </summary>
        /// <param name="rectangle2D"></param>
        private void SetRetangle2DCore(Rectangle2D2 rectangle2D) {
            if (rectangle2D == null) {
                throw new ArgumentNullException(nameof(rectangle2D));
            }

            _rectangle2D = rectangle2D;
            RaiseVisualChanged();
        }
        
        public override Rectangle2D2 GetBoundingRect() {
            if (Rectangle2D == null) {
                return null;
            }

            return Rectangle2D;
        }

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint) {
            if(rect == null) {
                throw new ArgumentNullException(nameof(rect));
            }

            if(Rectangle2D == null) {
                return false;
            }

            //根据四个顶点的位置判断与指定矩形的包含关系;
            if (anyPoint) {
                return Rectangle2D.GetVertexes()?.Any(p => rect.Contains(p)) ?? false;
            }
            else {
                return Rectangle2D.GetVertexes()?.All(p => rect.Contains(p)) ?? false;
            }
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy) {
            if(point == null) {
                throw new ArgumentNullException(nameof(point));
            }

            if(canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            if(Rectangle2D == null) {
                return false;
            }

            var screenPoint = canvasProxy.ToScreen(point);

            //判断与中心的关系;
            var centerScreenPoint = canvasProxy.ToScreen(Rectangle2D.Center);
            var screenRect = Tida.Canvas.Infrastructure.Utils.NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                centerScreenPoint, 
                TolerantedScreenLength,
                TolerantedScreenLength
            );

            if (screenRect.Contains(screenPoint)) {
                return true;
            }

            //判断与四边的关系;
            var lines = Rectangle2D.GetLines();
            var screenStartPoint = new Vector2D();
            var screenEndPoint = new Vector2D();

            foreach (var line in lines) {
                canvasProxy.ToScreen(line.Start, screenStartPoint);
                canvasProxy.ToScreen(line.End, screenEndPoint);
                var screenLine2D = new Line2D(
                    screenStartPoint,screenEndPoint
                );
                if(screenLine2D.Distance(screenPoint) < TolerantedScreenLength) {
                    return true;
                }
            }

            return false;
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }

            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            //绘制自身;
            if(Rectangle2D == null) {
                return;
            }
            canvas.DrawRectangle(Rectangle2D, NormalRectColorBrush, NormalRectPen);

            //绘制选择状态;
            DrawSelectedState(canvas, canvasProxy);

            base.Draw(canvas, canvasProxy);
        }

        /// <summary>
        /// 绘制选择状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        private void DrawSelectedState(ICanvas canvas,ICanvasScreenConvertable canvasProxy) {
            if (!IsSelected) {
                return;
            }

            if(Rectangle2D == null) {
                return;
            }

            //绘制四边的中点;
            var lines = Rectangle2D.GetLines();

            var rectLength = TolerantedScreenLength;

            foreach (var line in lines) {
                var point = line.MiddlePoint;
                var rect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(canvasProxy.ToScreen(point), rectLength, rectLength);
                canvas.NativeDrawRectangle(rect, HighLightEllipseColorBrush, HighLightLinePen);
            }

            //绘制中心;
            var centerPointX = Rectangle2D.GetVertexes().Average(p => p.X);
            var centerPointY = Rectangle2D.GetVertexes().Average(p => p.Y);
           
            var centerScreenPoint = canvasProxy.ToScreen(new Vector2D(centerPointX, centerPointY));
            var centerRect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(centerScreenPoint, rectLength, rectLength);
            canvas.NativeDrawRectangle(centerRect, HighLightEllipseColorBrush, HighLightLinePen);
        }

        public override DrawObject Clone() => new Rectangle(Rectangle2D);
    }
}
