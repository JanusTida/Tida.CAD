using Tida.Canvas.Events;
using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD {

    /// <summary>
    /// 编辑工具协约;
    /// </summary>
    public  abstract partial class EditTool:CanvasElement, IDrawable,ICanvasInputElement {

        public abstract bool CanUndo { get; }

        public abstract bool CanRedo { get; }

        public virtual bool IsEditing => false;


        public event EventHandler<IEditTransaction> TransactionCommited;
        public event EventHandler<CanUndoChangedEventArgs> CanUndoChanged;
        public event EventHandler<CanRedoChangedEventArgs> CanRedoChanged;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseDown(ICanvasContextEx, MouseDownEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<MouseDownEventArgs> CanvasPreviewMouseDown;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseMove(ICanvasContextEx, MouseMoveEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<MouseMoveEventArgs> CanvasPreviewMouseMove;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseUp(ICanvasContextEx, MouseUpEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<MouseUpEventArgs> CanvasPreviewMouseUp;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewKeyDown(ICanvasContextEx, KeyDownEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<KeyDownEventArgs> CanvasPreviewKeyDown;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewKeyUp(ICanvasContextEx, KeyUpEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<KeyUpEventArgs> CanvasPreviewKeyUp;

        public event EventHandler<TextInputEventArgs> CanvasPreviewTextInput;

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
        /// 处理事件的泛型方法;
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="previewHandler"></param>
        /// <param name="handler"></param>
        /// <param name="e"></param>
        private static void HandleEventArgs<TEventArgs>(
            object sender,
            EventHandler<TEventArgs> previewHandler,
            Action<TEventArgs> handler,
            TEventArgs e
        ) where TEventArgs : HandleableEventArgs {

            previewHandler?.Invoke(sender, e);

            if (e.Handled) {
                return;
            }

            handler(e);
        }


        public void RaisePreviewMouseDown(MouseDownEventArgs e) {
            HandleEventArgs(this, CanvasPreviewMouseDown, OnMouseDown, e);
        }

        protected virtual void OnMouseDown(MouseDownEventArgs e) { }

        public void RaisePreviewMouseMove(MouseMoveEventArgs e) {
            HandleEventArgs(this, CanvasPreviewMouseMove, OnMouseMove, e);
        }

        protected virtual void OnMouseMove(MouseMoveEventArgs e) {

        }

        public void RaisePreviewMouseUp(MouseUpEventArgs e) {
            HandleEventArgs(this, CanvasPreviewMouseUp, OnMouseUp, e);
        }

        protected virtual void OnMouseUp(MouseUpEventArgs e) {

        }

        public void RaisePreviewKeyDown(KeyDownEventArgs e) {
            HandleEventArgs(this, CanvasPreviewKeyDown, args => OnKeyDown(args), e);
        }

        protected virtual void OnKeyDown(KeyDownEventArgs e) {

        }

        public void RaisePreviewKeyUp(KeyUpEventArgs e) {
            HandleEventArgs(this, CanvasPreviewKeyUp, OnKeyUp, e);
        }

        protected virtual void OnKeyUp(KeyUpEventArgs e) {
        }

        public void RaisePreviewTextInput(TextInputEventArgs e) {
            HandleEventArgs(this, CanvasPreviewTextInput, OnTextInput, e);
        }

        protected virtual void OnTextInput(TextInputEventArgs e) {

        }
    }
}
