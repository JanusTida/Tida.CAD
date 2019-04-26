using System;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 通过圆的直径的两个端点创建圆的编辑工具;
    /// </summary>
    public class RoundDiameterTwoPointsEditTool :MouseInteractableEditToolGenericBase<Ellipse> {

        protected override void OnCommit() {
            MousePositionTracker.LastMouseDownPosition = null;
            MousePositionTracker.CurrentHoverPosition = null;
            base.OnCommit();
        }

        protected override void OnEndOperation() {
            MousePositionTracker.LastMouseDownPosition = null;
            MousePositionTracker.CurrentHoverPosition = null;
            base.OnEndOperation();
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            if(canvas == null) {
                throw new ArgumentNullException(nameof(canvas));
            }

            base.Draw(canvas, canvasProxy);

            if (MousePositionTracker.LastMouseDownPosition == null || MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            //绘制编辑的圆的预览状态;

            //确定圆心;
            var centerPoint = new Vector2D(
                (MousePositionTracker.CurrentHoverPosition.X + MousePositionTracker.LastMouseDownPosition.X) / 2,
                (MousePositionTracker.CurrentHoverPosition.Y + MousePositionTracker.LastMouseDownPosition.Y) / 2
            );

            //确定半径;
            var subX = MousePositionTracker.CurrentHoverPosition.X - MousePositionTracker.LastMouseDownPosition.X;
            var subY = MousePositionTracker.CurrentHoverPosition.Y - MousePositionTracker.LastMouseDownPosition.Y;
            var radius = Math.Sqrt(subX * subX + subY * subY) / 2;
            
            canvas.DrawEllipse(NormalEllipseColorBrush,NormalEllipsePen,centerPoint, radius, radius);

            
        }

        protected override void OnApplyMouseDownPosition(Vector2D thisMouseDownPosition) {
            //若上次按下位置为空,则记录新的点;
            if (MousePositionTracker.LastMouseDownPosition == null) {
                MousePositionTracker.LastMouseDownPosition = thisMouseDownPosition;
            }
            //否则将创建一个新的圆;
            else {
                //确定圆心;
                var centerPoint = new Vector2D(
                    (thisMouseDownPosition.X + MousePositionTracker.LastMouseDownPosition.X) / 2,
                    (thisMouseDownPosition.Y + MousePositionTracker.LastMouseDownPosition.Y) / 2
                );

                //确定半径;
                var subX = thisMouseDownPosition.X - MousePositionTracker.LastMouseDownPosition.X;
                var subY = thisMouseDownPosition.Y - MousePositionTracker.LastMouseDownPosition.Y;
                var radius = Math.Sqrt(subX * subX + subY * subY) / 2;

                //创建圆;
                var round = new Ellipse(new Ellipse2D(centerPoint, radius, radius));

                AddDrawObjectToUndoStack(round);
                MousePositionTracker.LastMouseDownPosition = null;
            }
        }
    }

    
}
