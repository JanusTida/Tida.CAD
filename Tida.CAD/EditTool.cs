using System;
using Tida.CAD.Events;
using System.Windows.Input;
using System.Windows;
using Tida.CAD.Input;

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
        /// 通知将执行<see cref="EditTool.OnPreviewMouseDown(CADMouseEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<CADMouseButtonEventArgs> PreviewMouseDown;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseMove(CADMouseEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<CADMouseEventArgs> PreviewMouseMove;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewMouseUp(CADMouseEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<CADMouseButtonEventArgs> PreviewMouseUp;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnPreviewKeyDown(CADKeyEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<CADKeyEventArgs> PreviewKeyDown;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnKeyUp(CADKeyEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<CADKeyEventArgs> PreviewKeyUp;

        /// <summary>
        /// 通知将执行<see cref="EditTool.OnTextInput(CADTextInputEventArgs)"/>动作;
        /// </summary>
        public event EventHandler<CADTextInputEventArgs> PreviewTextInput;

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
        ) where TEventArgs:CADRoutedEventArgs {

            previewHandler?.Invoke(sender, e);

            if (e.Handled) {
                return;
            }

            handler(e);
        }

        public void RaisePreviewMouseDown(CADMouseButtonEventArgs e) 
        {
            HandleEvent(this, PreviewMouseDown, OnMouseDown, e);
        }

        protected virtual void OnMouseDown(CADMouseButtonEventArgs e) { }

        public void RaisePreviewMouseMove(CADMouseEventArgs e) 
        {
            HandleEvent(this, PreviewMouseMove, OnMouseMove, e);
        }

        protected virtual void OnMouseMove(CADMouseEventArgs e) 
        {

        }

        public void RaisePreviewMouseUp(CADMouseButtonEventArgs e) 
        {
            HandleEvent(this, PreviewMouseUp, OnMouseUp, e);
        }

        protected virtual void OnMouseUp(CADMouseButtonEventArgs e) 
        {

        }

        public void RaisePreviewKeyDown(CADKeyEventArgs e) 
        {
            HandleEvent(this, PreviewKeyDown, args => OnKeyDown(args), e);
        }

        protected virtual void OnKeyDown(CADKeyEventArgs e) 
        {

        }

        public void RaisePreviewKeyUp(CADKeyEventArgs e)
        {
            HandleEvent(this, PreviewKeyUp, OnKeyUp, e);
        }

        protected virtual void OnKeyUp(CADKeyEventArgs e) 
        {
        }

        public void RaisePreviewTextInput(CADTextInputEventArgs e) 
        {
            HandleEvent(this, PreviewTextInput, OnTextInput, e);
        }

        protected virtual void OnTextInput(CADTextInputEventArgs e) 
        {

        }
    }
}
