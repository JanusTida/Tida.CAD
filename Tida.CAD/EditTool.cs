using System;
using Tida.CAD.Events;
using System.Windows.Input;
using System.Windows;

namespace Tida.CAD
{

    /// <summary>
    /// 编辑工具协约;
    /// </summary>
    public  abstract partial class EditTool:CanvasElement, IDrawable {

        public abstract bool CanUndo { get; }

        public abstract bool CanRedo { get; }

        public virtual bool IsEditing => false;


        public event EventHandler<IEditTransaction> TransactionCommited;
        public event EventHandler<CanUndoChangedEventArgs> CanUndoChanged;
        public event EventHandler<CanRedoChangedEventArgs> CanRedoChanged;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseDown(MouseEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> PreviewMouseDown;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseMove(MouseEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<MouseEventArgs> PreviewMouseMove;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseUp(MouseEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<MouseEventArgs> PreviewMouseUp;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewKeyDown(KeyEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<KeyEventArgs> PreviewKeyDown;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnKeyUp(KeyEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<KeyEventArgs> PreviewKeyUp;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnTextInput(KeyEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<TextCompositionEventArgs> PreviewTextInput;

        /// <summary>
        /// 开始/终止操作事件事件;
        /// </summary>
        public event EventHandler OperationBegan;
        public event EventHandler OperationEnded;
        /// <summary>
        /// 发生了错误事件;
        /// </summary>
        public event EventHandler<Exception> ErrorOccurred;

        /// <summary>
        /// 触发可撤销/重做状态变更事件;
        /// </summary>
        protected void RaiseCanUndoRedoChanged() {
            CanRedoChanged?.Invoke(this, new CanRedoChangedEventArgs(CanRedo));
            CanUndoChanged?.Invoke(this, new CanUndoChangedEventArgs(CanUndo));
        }

        /// <summary>
        /// 触发错误发生事件;
        /// </summary>
        /// <param name="ex"></param>
        protected void RaiseErrorOccurred(Exception ex) {
            if (ex == null) {
                throw new ArgumentNullException(nameof(ex));
            }

            ErrorOccurred?.Invoke(this, ex);
        }

        /// <summary>
        /// 呈递事务,并触发已呈递事务事件,可被子类使用;
        /// </summary>
        /// <param name="transaction"></param>
        protected void CommitTransaction(IEditTransaction transaction) {
            if (transaction == null) {
                throw new ArgumentNullException(nameof(transaction));
            }

            TransactionCommited?.Invoke(this, transaction);
        }

        /// <summary>
        /// 应用未更改的编辑操作;
        /// </summary>
        public void Commit() {
            OnCommit();
        }

        protected virtual void OnCommit() { }

        public abstract void Redo();

        public abstract void Undo();

        private ICanvasContextEx _canvasContext;
        public ICanvasContextEx CanvasContext {
            get => _canvasContext;
            set {
                var oldCanvasContext = _canvasContext;
                _canvasContext = value;
                OnCanvasContextChanged(new ValueChangedEventArgs<ICanvasContextEx>(_canvasContext, oldCanvasContext));
            }
        }

        /// <summary>
        /// 当<see cref="CanvasContext"/>发生变化时;
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCanvasContextChanged(ValueChangedEventArgs<ICanvasContextEx> args) { }

        /// <summary>
        /// 开始操作;
        /// </summary>
        /// <param name="canvasContext"></param>
        public void BeginOperation() {
            OnBeginOperation();
            OperationBegan?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 开始操作可重写方法;
        /// </summary>
        /// <param name="canvasContext"></param>
        protected virtual void OnBeginOperation() {

        }

        /// <summary>
        /// 终止操作可重写方法;
        /// </summary>
        /// <param name="canvasContext"></param>
        public void EndOperation() {
            OnEndOperation();
            OperationEnded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 停止操作;
        /// </summary>
        /// <param name="canvasContext"></param>
        protected virtual void OnEndOperation() {

        }
        
        
    }

    public  abstract partial class EditTool {

        /// <summary>
        /// Handle a routed event;
        /// </summary>
        /// <param name="previewHandler"></param>
        /// <param name="handler"></param>
        /// <param name="e"></param>
        private static void HandleEvent<TEventArgs>(
            object sender, EventHandler<TEventArgs> previewHandler, Action<TEventArgs> handler, TEventArgs e
        ) where TEventArgs:RoutedEventArgs {

            previewHandler?.Invoke(sender, e);

            if (e.Handled) {
                return;
            }

            handler(e);
        }

        public void RaisePreviewMouseDown(MouseButtonEventArgs e) {
            HandleEvent(this, PreviewMouseDown, OnMouseDown, e);
        }

        protected virtual void OnMouseDown(MouseButtonEventArgs e) { }

        public void RaisePreviewMouseMove(MouseEventArgs e) {
            HandleEvent(this, PreviewMouseMove, OnMouseMove, e);
        }

        protected virtual void OnMouseMove(MouseEventArgs e) {

        }

        public void RaisePreviewMouseUp(MouseButtonEventArgs e) {
            HandleEvent(this, PreviewMouseUp, OnMouseUp, e);
        }

        protected virtual void OnMouseUp(MouseEventArgs e) {

        }

        public void RaisePreviewKeyDown(KeyEventArgs e) {
            HandleEvent(this, PreviewKeyDown, args => OnKeyDown(args), e);
        }

        protected virtual void OnKeyDown(KeyEventArgs e) {

        }

        public void RaisePreviewKeyUp(KeyEventArgs e) {
            HandleEvent(this, PreviewKeyUp, OnKeyUp, e);
        }

        protected virtual void OnKeyUp(KeyEventArgs e) {
        }

        public void RaisePreviewTextInput(TextCompositionEventArgs e) {
            PreviewTextInput?.Invoke(this, e);
            if (e.Handled)
            {
                return;
            }
            OnTextInput(e);
        }

        protected virtual void OnTextInput(TextCompositionEventArgs e) {

        }
    }
}
