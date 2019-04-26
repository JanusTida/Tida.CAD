using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.Canvas.Events;

namespace Tida.Canvas.Shell.EditTools.Events {
    /// <summary>
    /// 编辑工具实例被发生变更时的事件处理器泛型基类;
    /// </summary>
    /// <typeparam name="TEditTool">关注的编辑工具具体类型</typeparam>
    public abstract class CanvasEditToolChangedEventHandlerGenericBase<TEditTool> : ICanvasEditToolChangedEventHandler where TEditTool :  EditTool {
        public virtual int Sort => 0;

        public virtual bool IsEnabled => true;

        public void Handle(CanvasEditToolChangedEventArgs args) {
            if(args.EventArgs.OldValue is TEditTool oldEditTool) {
                HandleOldEditTool(args,oldEditTool);
            }
            if (args.EventArgs.NewValue is TEditTool newEditTool) {
                HandleNewEditTool(args, newEditTool);
            }
        }

        /// <summary>
        /// 处理新的编辑工具;
        /// </summary>
        /// <param name="newEditTool"></param>
        protected abstract void HandleNewEditTool(CanvasEditToolChangedEventArgs args, TEditTool newEditTool);

        /// <summary>
        /// 处理旧的编辑工具;
        /// </summary>
        /// <param name="args"></param>
        /// <param name="oldEditTool"></param>
        protected abstract void HandleOldEditTool(CanvasEditToolChangedEventArgs args, TEditTool oldEditTool);
    }
}
