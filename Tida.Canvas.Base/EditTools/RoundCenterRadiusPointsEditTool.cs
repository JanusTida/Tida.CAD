using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Base.EditTools {
    /// <summary>
    /// 根据圆心与直径创建圆的工具;
    /// </summary>
    public class RoundCenterRadiusPointsEditTool : UniqueTypeEditToolGenericBase<Ellipse> {
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
            

            if (CanvasContext.ActiveLayer == null) {
                throw new InvalidOperationException($"The {CanvasContext.ActiveLayer} of {nameof(ICanvasContextEx )} can't be null.");
            }

            e.Handled = true;

            var thisPosition = e.Position;
            //若上次按下位置为空,则记录新的点;
            if (_lastMouseDownPosition == null) {
                _lastMouseDownPosition = thisPosition;
            }
            //否则将创建一个新的圆;
            else {
                //确定半径;
                var subX = thisPosition.X - _lastMouseDownPosition.X;
                var subY = thisPosition.Y - _lastMouseDownPosition.Y;
                var radius = Math.Sqrt(subX * subX + subY * subY);

                //创建圆;
                var round = new Ellipse(new Ellipse2D(_lastMouseDownPosition, radius, radius));

                AddDrawObjectToUndoStack(round);
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
            //绘制编辑的圆的预览状态;

            //确定圆心;
            var centerPoint = _lastMouseDownPosition;

            //绘制编辑的圆的预览状态;
            //确定半径;
            var subX = _currentMousePosition.X - _lastMouseDownPosition.X;
            var subY = _currentMousePosition.Y - _lastMouseDownPosition.Y;
            var radius = Math.Sqrt(subX * subX + subY * subY);

            canvas.DrawEllipse(
                NormalEllipseColorBrush, 
                NormalEllipsePen, centerPoint, radius, radius
            );
            
        }

        protected override void OnCommit( ) {
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
