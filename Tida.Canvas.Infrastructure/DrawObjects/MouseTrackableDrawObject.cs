using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Infrastructure.DrawObjects {
    /// <summary>
    /// 可记录部分鼠标状态的绘制对象基类;
    /// </summary>
    public abstract class MousePositionTrackableDrawObject : DrawObject, IHaveMousePositionTracker {
        public MousePositionTrackableDrawObject() {
            MousePositionTracker.LastMouseDownPositionChanged += MousePositionTracker_LastMouseDownPositionChanged;
            MousePositionTracker.CurrentHoverPositionChanged += MousePositionTracker_CurrentHoverPositionChanged;

            MousePositionTracker.PreviewLastMouseDownPositionChanged += MousePositionTrackerPreviewLastMouseDownPositionChanged;
            MousePositionTracker.PreviewCurrentHoverPositionChanged += MousePositionTrackerPreviewCurrentHoverPositionChanged;
        }

        private void MousePositionTrackerPreviewCurrentHoverPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            OnMousePositionTrackerPreviewCurrentHoverPositionChanged(sender,e);
        }

        protected virtual void OnMousePositionTrackerPreviewCurrentHoverPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) { }

        private void MousePositionTrackerPreviewLastMouseDownPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            OnMousePositionTrackerPreviewLastMouseDownPositionChanged(sender, e);
        }

        /// <summary>
        /// 根据上次鼠标位置的变化,在必要时,本方法可能会触发<see cref="IsEditingChanged"/>事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMousePositionTrackerPreviewLastMouseDownPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            if ((e.NewValue == null) != (e.OldValue == null)) {
                RaiseIsEditingChanged(new ValueChangedEventArgs<bool>(IsEditing, !IsEditing));
            }
        }

        private void MousePositionTracker_CurrentHoverPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            RaiseVisualChanged();
        }

        private void MousePositionTracker_LastMouseDownPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            RaiseVisualChanged();
        }

        private MousePositionTracker _mousePositionTracker;
        public MousePositionTracker MousePositionTracker => _mousePositionTracker ?? (_mousePositionTracker = new MousePositionTracker(this));

        /// <summary>
        /// 鼠标按下时,将<see cref="MousePositionTracker.LastMouseDownPosition"/>与
        /// <see cref="MousePositionTracker.CurrentHoverPosition"/>设置为<paramref name="e"/>的位置;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseDownEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Button != MouseButton.Left) {
                return;
            }

            var thisPosition = e.Position;

            MousePositionTracker.SetBothMousePositions(thisPosition, true);
            RaiseVisualChanged();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 鼠标移动时,且<see cref="IsEditing"/>为真时;
        /// 将<see cref="MousePositionTracker.CurrentHoverPosition"/>置为<paramref name="e"/>的位置;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseMoveEventArgs e) {
            if (IsEditing) {
                MousePositionTracker.CurrentHoverPosition = e.Position;
            }
            base.OnMouseMove(e);
        }
        
        /// <summary>
        /// 是否正在被编辑,<see cref="MousePositionTracker.LastMouseDownPosition"/>不为空时为真;
        /// </summary>
        public override bool IsEditing => MousePositionTracker.LastMouseDownPosition != null;

        protected override void OnSelectedChanged(ValueChangedEventArgs<bool> e) {
            //若未被选中,则取消辅助线;
            if (!e.NewValue) {
                MousePositionTracker.LastMouseDownPosition = null;
                MousePositionTracker.CurrentHoverPosition = null;
            }
            base.OnSelectedChanged(e);
        }
    }
}
