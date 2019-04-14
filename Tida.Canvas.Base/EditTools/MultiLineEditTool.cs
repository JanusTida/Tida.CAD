using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Base.EditTools {
    public class MultiLineEditTool:MultiLineEditToolGenericBase<Line> {

        protected override void OnEndOperation() {
            MousePositionTracker.LastMouseDownPosition = null;
            MousePositionTracker.CurrentHoverPosition = null;
            base.OnEndOperation();
        }


        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            base.Draw(canvas, canvasProxy);

            //检查两个关键位置是否为空;
            if (MousePositionTracker.LastMouseDownPosition == null || MousePositionTracker.CurrentHoverPosition == null) {
                return;
            }

            //绘制当前随鼠标位置移动的编辑线段;
            //绘制本次尚未完成的线段的起始点小正方形;
            var lastMouseDownScreenPosition = canvasProxy.ToScreen(MousePositionTracker.LastMouseDownPosition);
            var sideLength = HighLightRectLength;

            var startEditRect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(lastMouseDownScreenPosition, sideLength, sideLength);

            canvas.NativeDrawRectangle(startEditRect, HighLightRectColorBrush, LinePen);

            //在外部加入了动态输入,此时显示线段编辑状态多余;
            //DrawEditingLineState(canvas, canvasProxy);
        }

        /// <summary>
        /// 绘制未完成的编辑线段状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        private void DrawEditingLineState(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            //绘制未完成状态;
            var editingLine = new Line2D(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition);
            LineDrawExtensions.DrawEditingLine(canvas,canvasProxy,editingLine);
        }
        

        protected override Line OnCreateDrawObject(Vector2D lastDownPosition, Vector2D thisMouseDownPosition) {
            return new Line(lastDownPosition, thisMouseDownPosition);
        }
        
#if DEBUG
        ~MultiLineEditTool() {

        }
#endif
    }
}
