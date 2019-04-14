using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Infrastructure.Utils;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 单次绘制的线性编辑工具泛型基类;
    /// </summary>
    /// <typeparam name="TLine"></typeparam>
    public abstract class SingleLineEditToolGenericBase<TLine>:MouseInteractableEditToolGenericBase<TLine> where TLine:LineBase {
        protected override void OnApplyMouseDownPosition(Vector2D thisMouseDownPosition) {

            //若上一次鼠标按下的位置不为空,则不是第一次按下鼠标,需添加杆件对象;
            if (MousePositionTracker.LastMouseDownPosition != null) {
                var line = OnCreateDrawObject(MousePositionTracker.LastMouseDownPosition, thisMouseDownPosition);

                MousePositionTracker.LastMouseDownPosition = null;

                AddDrawObjectToUndoStack(line);
            }
            else {
                MousePositionTracker.LastMouseDownPosition = thisMouseDownPosition;
            }
        }

        /// <summary>
        /// 根据关键信息,创建一个特定的绘制对象;
        /// </summary>
        /// <param name="lastDownPosition"></param>
        /// <param name="thisMouseDownPosition"></param>
        /// <returns></returns>
        protected abstract TLine OnCreateDrawObject(Vector2D lastDownPosition, Vector2D thisMouseDownPosition);

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
            DrawEditingLineState(canvas, canvasProxy);
        }

        /// <summary>
        /// 绘制未完成的编辑线段状态;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="canvasProxy"></param>
        private void DrawEditingLineState(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            //绘制未完成状态;
            var editingLine = new Line2D(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition);
            LineDrawExtensions.DrawEditingLine(canvas, canvasProxy, editingLine);
            //绘制未完成线段;
            canvas.DrawLine(LinePen, editingLine);
        }
    }
}
