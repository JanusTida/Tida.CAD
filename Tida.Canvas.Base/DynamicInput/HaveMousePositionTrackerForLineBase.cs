using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Base.DynamicInput {

    /// <summary>
    /// 为编辑线的一端所封装的<see cref="IHaveMousePositionTracker,IInputElement,IDisposable"/>;
    /// 本类实现了<see cref="IDisposable"/>,在构造时订阅了<see cref="LineBase"/>的部分事件,
    /// 在<see cref="LineBase"/>的生命周期相对本实例较长时,务必在必要时调用<see cref="IDisposable.Dispose"/>方法,
    /// 否则可能导致本类的实例无法被回收,以及可能存在的应用程序功能未按预期执行;
    /// </summary>
    public class HaveMousePositionTrackerForLineBase : IHaveMousePositionTracker, IInputElement, IDisposable {
        public HaveMousePositionTrackerForLineBase(LineBase lineBase) {

            Line = lineBase ?? throw new ArgumentNullException(nameof(lineBase));


            InitializeLine();
        }

        private void InitializeLine() {
            if (!Line.MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line.Line2D.Start) &&
               !Line.MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line.Line2D.End)) {

                throw new InvalidOperationException();

            }

            MousePositionTracker = new MousePositionTracker(this);

            ///建立两个<see cref="MousePositionTracker"/>的双向"绑定";
            MousePositionTracker.CurrentHoverPositionChanged += MousePositionTrackerForLine_PreviewCurrentHoverPositionChanged;
            Line.MousePositionTracker.CurrentHoverPositionChanged += MousePositionTracker_CurrentHoverPositionChanged;

            /////设置与上次点击点相对另一端为<see cref="MousePositionTracker"/>的上次鼠标点击位置;
            var otherSidePosition = Line.MousePositionTracker.LastMouseDownPosition.IsAlmostEqualTo(Line.Line2D.Start) ? Line.Line2D.End : Line.Line2D.Start;
            MousePositionTracker.LastMouseDownPosition = otherSidePosition;
        }

        private void UnInitializeLine() {
            MousePositionTracker.CurrentHoverPositionChanged -= MousePositionTrackerForLine_PreviewCurrentHoverPositionChanged;
            Line.MousePositionTracker.CurrentHoverPositionChanged -= MousePositionTracker_CurrentHoverPositionChanged;
        }

        private void MousePositionTracker_CurrentHoverPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            MousePositionTracker.CurrentHoverPosition = e.NewValue;
        }

        public LineBase Line { get; }

        public event EventHandler<MouseDownEventArgs> CanvasPreviewMouseDown {
            add => Line.CanvasPreviewMouseDown += value;
            remove => Line.CanvasPreviewMouseDown -= value;
        }

        public event EventHandler<MouseMoveEventArgs> CanvasPreviewMouseMove {
            add => Line.CanvasPreviewMouseMove += value;
            remove => Line.CanvasPreviewMouseMove -= value;
        }

        public event EventHandler<MouseUpEventArgs> CanvasPreviewMouseUp {
            add => Line.CanvasPreviewMouseUp += value;
            remove => Line.CanvasPreviewMouseUp += value;
        }

        public event EventHandler<KeyDownEventArgs> CanvasPreviewKeyDown {
            add => Line.CanvasPreviewKeyDown += value;
            remove => Line.CanvasPreviewKeyDown -= value;
        }

        public event EventHandler<KeyUpEventArgs> CanvasPreviewKeyUp {
            add => Line.CanvasPreviewKeyUp += value;
            remove => Line.CanvasPreviewKeyUp -= value;
        }

        public event EventHandler<TextInputEventArgs> CanvasPreviewTextInput {
            add => Line.CanvasPreviewTextInput += value;
            remove => Line.CanvasPreviewTextInput -= value;
        }

        public MousePositionTracker MousePositionTracker { get; private set; }

        private void MousePositionTrackerForLine_PreviewLastMouseDownPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            Line.MousePositionTracker.LastMouseDownPosition = e.NewValue;
        }

        private void MousePositionTrackerForLine_PreviewCurrentHoverPositionChanged(object sender, ValueChangedEventArgs<Vector2D> e) {
            Line.MousePositionTracker.CurrentHoverPosition = e.NewValue;
        }

        public void RaisePreviewMouseDown(MouseDownEventArgs e) {
            Line.RaisePreviewMouseDown(e);
        }

        public void RaisePreviewMouseMove(MouseMoveEventArgs e) {
            Line.RaisePreviewMouseMove(e);
        }

        public void RaisePreviewMouseUp(MouseUpEventArgs e) {
            Line.RaisePreviewMouseUp(e);
        }

        public void RaisePreviewKeyDown(KeyDownEventArgs e) {
            Line.RaisePreviewKeyDown(e);
        }

        public void RaisePreviewKeyUp(KeyUpEventArgs e) {
            Line.RaisePreviewKeyUp(e);
        }

        public void RaisePreviewTextInput(TextInputEventArgs e) {
            Line.RaisePreviewTextInput(e);
        }

        public void Dispose() {
            UnInitializeLine();
        }
    }
}
