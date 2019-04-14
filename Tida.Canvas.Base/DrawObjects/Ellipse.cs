using Tida.Canvas.Contracts;
using System;
using System.Linq;
using Tida.Geometry.Primitives;
using Tida.Canvas.Input;
using Tida.Canvas.Events;
using Tida.Geometry.External.Util;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Infrastructure.DrawObjects;
using static Tida.Canvas.Infrastructure.Constants;
using Tida.Geometry.External;
using Tida.Canvas.Infrastructure.Contracts;

namespace Tida.Canvas.Base.DrawObjects {
    /// <summary>
    /// 绘制对象——椭圆(圆);
    /// </summary>
    public class Ellipse : MousePositionTrackableDrawObject {
        public Ellipse(Ellipse2D ellipse2D) {
            Ellipse2D = ellipse2D ?? throw new ArgumentNullException(nameof(ellipse2D));
        }
        
        /// <summary>
        /// 指示当前Ellipse的横半径与纵半径的变化是否能够不等比;
        /// </summary>
        private bool _canRadiusChangeUnratioble;
        

        /// <summary>
        /// 椭圆的核心数据,该值不可为空;
        /// </summary>
        private Ellipse2D _ellipse2D;
        public Ellipse2D Ellipse2D {
            get {
                return _ellipse2D;
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                if(_ellipse2D == value) {
                    return;
                }

                var oldValue = _ellipse2D;
                var newValue = value;
                SetEllipse2DCore(newValue);
                //呈递事务;
                RaiseEditTransActionCommited(new StandardEditTransaction(
                    () => SetEllipse2DCore(oldValue),
                    () => SetEllipse2DCore(newValue)
                ));
            }
        }
        
        
        /// <summary>
        /// 设定椭圆数据核心;
        /// </summary>
        /// <param name="ellipse2D"></param>
        private void SetEllipse2DCore(Ellipse2D ellipse2D) {
            _ellipse2D = ellipse2D ?? throw new ArgumentNullException(nameof(ellipse2D));
            RaiseVisualChanged();
        }
        
        public override Rectangle2D2 GetBoundingRect() {
            if(Ellipse2D == null) {
                return null;
            }

            if(Ellipse2D.Center == null) {
                return null; 
            }

            var centerPoint = Ellipse2D.Center;
            var radiusX = Ellipse2D.RadiusX;
            var radiusY = Ellipse2D.RadiusY;

            return new Rectangle2D2(
                new Line2D(
                    new Vector2D(Ellipse2D.Center.X - radiusX, Ellipse2D.Center.Y),
                    new Vector2D(Ellipse2D.Center.X + radiusX, Ellipse2D.Center.Y)
                ),
                2 * radiusY
            );
            
        }

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint) {
            if(rect == null) {
                throw new ArgumentNullException(nameof(rect));
            }

            if(Ellipse2D == null) {
                return false;
            }

            //无论是否为任意选中,若包含了,则能够选中;
            if (rect.Contains(Ellipse2D.Center) && rect.GetLines().All(p => p.Distance(Ellipse2D.Center) >= Ellipse2D.RadiusX)) {
                return true;
            }


            //若为任意点选中,矩形的四个顶点中,存在任意两点任意一点在椭圆内即可;
            if (anyPoint && rect.GetLines().Any(p => {
                var intersectPoints = Extension.IntersectWithLine(Ellipse2D, p);
                return (intersectPoints?.Length ?? 0) != 0;
            })) {
                return rect.GetVertexes().Any(p => Ellipse2D.Contains(p));
            }


            return false;
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy) {
            if(point == null) {
                throw new ArgumentNullException(nameof(point));
            }

            if(canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            if(Ellipse2D == null) {
                return false;
            }

#if DEBUG
#endif

            //判断是否与圆心有关系;
            var centerScreenPosition = canvasProxy.ToScreen(Ellipse2D.Center);
            var centerScreenRect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                centerScreenPosition,
                TolerantedScreenLength,
                TolerantedScreenLength
            );
            if (centerScreenRect.Contains(canvasProxy.ToScreen(point))) {
                return true;
            }

            //若是圆,则通过点到圆心的距离判断;
            if(Ellipse2D.RadiusX == Ellipse2D.RadiusY) {
                var screenDis = canvasProxy.ToScreen(Ellipse2D.Center.Distance(point));
                var screenRadius = canvasProxy.ToScreen(Ellipse2D.RadiusX);

                //若半径小于误差值,则判断距离是否小于误差值;
                if (screenRadius < TolerantedScreenLength) {
                    return screenDis < TolerantedScreenLength;
                }
                //判断是否处于圆周上;
                else {
                    return screenDis > screenRadius - TolerantedScreenLength &&
                        screenDis < screenRadius + TolerantedScreenLength;
                }

            }
            //To-Do,椭圆,未完成;
            else {
                return Ellipse2D.Contains(point);
            }
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if(canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }

            if(canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            if (Ellipse2D == null) {
                return;
            }

            //绘制自身;
            canvas.DrawEllipse(
                NormalEllipseColorBrush,
                NormalEllipsePen,
                Ellipse2D.Center, 
                Ellipse2D.RadiusX, 
                Ellipse2D.RadiusY
            );

            DrawSelectedState(canvas,canvasProxy);
            DrawPreviewState(canvas, canvasProxy);
        }

        /// <summary>
        /// 绘制选择状态;
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawSelectedState(ICanvas canvas,ICanvasScreenConvertable canvasProxy) {
            if (!IsSelected) {
                return;
            }

            if(Ellipse2D == null) {
                return;
            }

            if(Ellipse2D.Center == null) {
                return;
            }

            var centerScreenPoint = canvasProxy.ToScreen(Ellipse2D.Center);
            var screenRadiusX = canvasProxy.ToScreen(Ellipse2D.RadiusX);
            var screenRadiusY = canvasProxy.ToScreen(Ellipse2D.RadiusY);
            var rectLength = TolerantedScreenLength;

            //依次绘制左、下、右、上、圆心的矩形;
            var points = new Vector2D[] {
                new Vector2D(centerScreenPoint.X - screenRadiusX,centerScreenPoint.Y),
                new Vector2D(centerScreenPoint.X ,centerScreenPoint.Y - screenRadiusY),
                new Vector2D(centerScreenPoint.X + screenRadiusX,centerScreenPoint.Y ),
                new Vector2D(centerScreenPoint.X ,centerScreenPoint.Y + screenRadiusY),
                new Vector2D(centerScreenPoint.X ,centerScreenPoint.Y),
            };


            foreach (var point in points) {
                var rect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(point, rectLength, rectLength);
                canvas.NativeDrawRectangle(rect,HighLightEllipseColorBrush, HighLightLinePen);
            }
        }

        /// <summary>
        /// 绘制更改后的预览状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        private void DrawPreviewState(ICanvas canvas,ICanvasScreenConvertable canvasProxy) {
            if(MousePositionTracker.LastMouseDownPosition == null) {
                return;
            }

            if(MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            var preEllipse = GetPreviewEllipse2D();
            if(preEllipse != null) {
                canvas.DrawEllipse(NormalEllipseColorBrush, NormalEllipsePen, preEllipse.Center, preEllipse.RadiusX, preEllipse.RadiusY);
            }
        }
        

        /// <summary>
        /// 获得根据编辑状态返回更改后的椭圆数据;
        /// </summary>
        /// <param name="currentPosition">当前的终止位置</param>
        /// <returns></returns>
        private Ellipse2D GetPreviewEllipse2D() {
            if(Ellipse2D == null) {
                return null;
            }

            if(MousePositionTracker.LastMouseDownPosition == null) {
                return null; 
            }

            if(MousePositionTracker.CurrentHoverPosition == null) {
                return null;
            }

            var preEllipse = Ellipse2D.Clone();
            var centerPoint = Ellipse2D.Center;
            var radiusX = Ellipse2D.RadiusX;
            var radiusY = Ellipse2D.RadiusY;

            //判断上一次鼠标按下的位置是否处于圆心,若是圆心,则动作为平移操作;
            if (MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Ellipse2D.Center)) {
                preEllipse.Center = MousePositionTracker.CurrentHoverPosition;
                return preEllipse;
            }
            //判断若上次鼠标按下的位置是否处于上下，左右四点中任意一个,则缩放半径;
            else {

                //若半径变化必须等比,则由等比缩放半径;
                if (!_canRadiusChangeUnratioble) {
                    preEllipse.RadiusX = MousePositionTracker.CurrentHoverPosition.Distance(Ellipse2D.Center);
                    preEllipse.RadiusY = MousePositionTracker.CurrentHoverPosition.Distance(Ellipse2D.Center);
                    return preEllipse;
                }
                //椭圆,未完成;
                else {

                }
                //var pointPairs = new 
            }

            return preEllipse;
        }
        
        protected override void OnMouseDown(MouseDownEventArgs e) {
            if(e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            var thisPosition = e.Position;
            //若上次点击位置为空,则记录点击位置;
            if(MousePositionTracker.LastMouseDownPosition == null) {
                if (thisPosition.IsAlmostEqualTo(Ellipse2D.Center)) {
                    MousePositionTracker.LastMouseDownPosition = thisPosition;
                }
                else if(thisPosition.IsAlmostEqualTo(Ellipse2D.GetTopPoint())
                    || thisPosition.IsAlmostEqualTo(Ellipse2D.GetBottomPoint())
                    || thisPosition.IsAlmostEqualTo(Ellipse2D.GetLeftPoint())
                    || thisPosition.IsAlmostEqualTo(Ellipse2D.GetRightPoint())){
                    MousePositionTracker.LastMouseDownPosition = thisPosition;
                }
                else {
                    return;
                }

                e.Handled = true;
            }
            //否则将应用更改;
            else if(MousePositionTracker.CurrentHoverPosition != null){
                var newEllipse = GetPreviewEllipse2D();
                if(newEllipse != null) {
                    Ellipse2D = newEllipse;
                }

                MousePositionTracker.Reset(true);
                
                e.Handled = true;
            }
        }
        

        protected override void OnSelectedChanged(ValueChangedEventArgs<bool> e) {
            if (!e.NewValue) {
                MousePositionTracker.Reset(true);
                _canRadiusChangeUnratioble = false;
            }

            RaiseVisualChanged();
            base.OnSelectedChanged(e);
        }

        public override DrawObject Clone() => new Ellipse(Ellipse2D);
    }
}
