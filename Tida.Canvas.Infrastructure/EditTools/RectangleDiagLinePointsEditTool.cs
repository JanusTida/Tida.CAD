using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 根据对角线创建矩形;
    /// </summary>
    public class RectangleDiagLinePointsEditTool :UniqueTypeEditToolGenericBase<Rectangle> {
        public override bool IsEditing => _lastMouseDownPosition != null;

        /// <summary>
        /// 鼠标上一次按下的位置;
        /// </summary>
        private Vector2D _lastMouseDownPosition;
        /// <summary>
        /// 鼠标的当前位置;
        /// </summary>
        private Vector2D _currentMousePosition;

        protected override void OnMouseDown(MouseDownEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }
            
            if (e.Position == null) {
                throw new ArgumentException($"The {e.Position} of {nameof(MouseDownEventArgs)} can't be null.");
            }
            

            e.Handled = true;

            if (CanvasContext.ActiveLayer == null) {
                throw new InvalidOperationException($"The {CanvasContext.ActiveLayer} of {nameof(ICanvasContextEx )} can't be null.");
            }

            var thisPosition = e.Position;
            if(thisPosition == null) {
                return;
            }

            //若上次按下位置为空,则记录新的点;
            if (_lastMouseDownPosition == null) {
                _lastMouseDownPosition = thisPosition;
            }
            //否则将创建一个新的矩形;
            else {
                //若对角线两端的横坐标或纵坐标相等,将不能构成一个矩形;
                if(_lastMouseDownPosition.X == thisPosition.X || _lastMouseDownPosition.Y == thisPosition.Y) {
                    return;
                }

                //根据四个顶点创建矩形;
                var middleLineY = (thisPosition.Y + _lastMouseDownPosition.Y) / 2;
                var rect2D = new Rectangle2D2(
                    new Line2D(
                        new Vector2D(_lastMouseDownPosition.X, middleLineY),
                        new Vector2D(thisPosition.X, middleLineY)
                    ),
                    Math.Abs(thisPosition.Y - _lastMouseDownPosition.Y)
                );
                var rect = new Rectangle(rect2D);
                AddDrawObjectToUndoStack(rect);
                _lastMouseDownPosition = null;
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Position == null) {
                throw new ArgumentException();
            }

            //记录当前的鼠标位置;
            _currentMousePosition = e.Position;

            if (_lastMouseDownPosition != null) {
                RaiseVisualChanged();
            }
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if (canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }

            base.Draw(canvas, canvasProxy);

            if (_lastMouseDownPosition == null || _currentMousePosition == null) {
                return;
            }
            
            //若对角线的两端纵坐标或横坐标相等,则不进行绘制;
            if(_currentMousePosition.X == _lastMouseDownPosition.X || _lastMouseDownPosition.Y == _currentMousePosition.Y) {
                return;
            }

            //绘制编辑的矩形的预览状态;
            var middleLineY = (_currentMousePosition.Y + _lastMouseDownPosition.Y) / 2;
            var rect2D = new Rectangle2D2(
                new Line2D(
                    new Vector2D(_lastMouseDownPosition.X, middleLineY),
                    new Vector2D(_currentMousePosition.X, middleLineY)
                ),
                Math.Abs(_currentMousePosition.Y - _lastMouseDownPosition.Y)
            );
            
            canvas.DrawRectangle(
                rect2D,
                NormalRectColorBrush,
                NormalRectPen
            );

            
        }

        protected override void OnCommit() {
            _lastMouseDownPosition = null;
            _currentMousePosition = null;
            base.OnCommit();
        }

        protected override void OnEndOperation() {
            _lastMouseDownPosition = null;
            _currentMousePosition = null;

            base.OnEndOperation();
        }

        
    }

   

}
