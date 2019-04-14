using Tida.Application.Contracts.Common;
using Tida.Canvas.Events;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {

    /// <summary>
    /// 画布已撤销事件参数;
    /// </summary>
    public class CanvasEditTransactionUndoneEventArgs : CanvasEventArgs<EditTransactionUndoneEventArgs> {
        public CanvasEditTransactionUndoneEventArgs(ICanvasDataContext canvasDataContext, EditTransactionUndoneEventArgs eventArgs) : base(canvasDataContext, eventArgs) {

        }
    }

    /// <summary>
    /// 编辑事务已撤销事件;
    /// </summary>
    public class CanvasEditTransactionUndoneEvent :PubSubEvent<CanvasEditTransactionUndoneEventArgs> {

    }

    
    public interface ICanvasEditTransactionUndoneEventHandler:IEventHandler<CanvasEditTransactionUndoneEventArgs> {

    }
}
