using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Geometry.Primitives;
using System;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.EditTools {

    /// <summary>
    /// 测量工具——长度;
    /// </summary>
    public class LengthMeasureEditTool : MouseInteractableEditToolGenericBase<MeasureLine>, IHaveMousePositionTracker {
        public LengthMeasureEditTool() {
            MousePositionTracker.LastMouseDownPositionChanged += MousePositionTracker_LastMouseDownPositionChanged;
            MousePositionTracker.CurrentHoverPositionChanged += MousePositionTracker_CurrentHoverPositionChanged;
        }

        private void MousePositionTracker_CurrentHoverPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            if(MousePositionTracker.LastMouseDownPosition != null) {
                RaiseVisualChanged();
            }
        }

        private void MousePositionTracker_LastMouseDownPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            if(e.OldValue != null && e.NewValue != null) {
                var measureLien = GetMeasureLine(e.OldValue, e.NewValue);
                if(measureLien == null) {
                    return;
                }

                MousePositionTracker.LastMouseDownPosition = null;
                AddDrawObjectToUndoStack(measureLien);
                RaiseVisualChanged();
            }
        }

        public override bool CanUndo => false;

        public override bool CanRedo => false;

        /// <summary>
        /// 呈递事务时是否将保持的数据绘制对象添加到到指定图层中;
        /// </summary>
        public bool ShouldCommitMeasureData { get; set; } = true;

        protected override void OnCommit() {
            if (ShouldCommitMeasureData) {
                base.OnCommit();
            }
            
        }

        public override void Redo() {
            return;
        }

        public override void Undo() {
            return;
        }

        public override bool IsEditing => true;
        
        /// <summary>
        /// 获取测量用线段;
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static MeasureLine GetMeasureLine(Vector2D start,Vector2D end) {

            if (start == null) {
                throw new ArgumentNullException(nameof(start));
            }

            if (end == null) {
                throw new ArgumentNullException(nameof(end));
            }


            return new MeasureLine(new Line2D(start, end)) { Pen = MeasureLengthPen };
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy) {
            base.Draw(canvas, canvasProxy);
            
            if (MousePositionTracker.LastMouseDownPosition != null && MousePositionTracker.CurrentHoverPosition != null) {
                var previewMeasureLine = GetMeasureLine(MousePositionTracker.LastMouseDownPosition, MousePositionTracker.CurrentHoverPosition);

                previewMeasureLine.Draw(canvas, canvasProxy);
            }
        }

        protected override void OnApplyMouseDownPosition(Vector2D thisMouseDownPosition) {
            MousePositionTracker.LastMouseDownPosition = thisMouseDownPosition;
        }
    }

    
}
