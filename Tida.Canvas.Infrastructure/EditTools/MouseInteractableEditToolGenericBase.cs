using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 根据鼠标状态再封装的泛型编辑工具;
    /// </summary>
    /// <typeparam name="TDrawObject">针对的绘制对象</typeparam>
    public abstract class MouseInteractableEditToolGenericBase<TDrawObject> : UniqueTypeEditToolGenericBase<TDrawObject>,IHaveMousePositionTracker where TDrawObject : DrawObject {
        public MouseInteractableEditToolGenericBase() {
            MousePositionTracker.CurrentHoverPositionChanged += MouseTracker_PositionStateChanged;
            MousePositionTracker.LastMouseDownPositionChanged += MouseTracker_PositionStateChanged;
        }

        private void MouseTracker_PositionStateChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            this.RaiseVisualChanged();
        }

        private MousePositionTracker _mousePositionTracker;
        public MousePositionTracker MousePositionTracker => _mousePositionTracker??(_mousePositionTracker = new MousePositionTracker(this));

        public override bool IsEditing => true;
        
        public override bool CanUndo => base.CanUndo || MousePositionTracker.LastMouseDownPosition != null;

        protected override void OnMouseDown(MouseDownEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Button != MouseButton.Left) {
                return;
            }


            e.Handled = true;

            //记录本次鼠标按下的位置;
            ApplyMouseDownPosition(e.Position);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }

            if (e.Position == null) {
                throw new ArgumentException();
            }

            //记录当前的鼠标位置;
            MousePositionTracker.CurrentHoverPosition = e.Position;
        }

        /// <summary>
        /// 应用鼠标按下的位置,并进行数据的修改;
        /// </summary>
        /// <param name="thisMouseDownPosition"></param>
        public void ApplyMouseDownPosition(Vector2D thisMouseDownPosition) {
            OnApplyMouseDownPosition(thisMouseDownPosition);
        }

        /// <summary>
        /// 应用鼠标按下的位置,并进行数据的修改,将在调用<see cref="ApplyMouseDownPosition(Vector2D)"/>被调用;
        /// </summary>
        /// <param name="thisMouseDownPosition"></param>
        protected abstract void OnApplyMouseDownPosition(Vector2D thisMouseDownPosition);

        

    }
}
