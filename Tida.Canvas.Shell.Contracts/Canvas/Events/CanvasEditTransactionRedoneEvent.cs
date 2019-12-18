
using Tida.Canvas.Events;
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {

    /// <summary>
    /// 画布已撤销事件参数;
    /// </summary>
    public class CanvasEditTransactionRedoneEventArgs : CanvasEventArgs<EditTransactionRedoneEventArgs> {
        public CanvasEditTransactionRedoneEventArgs(ICanvasDataContext canvasDataContext, EditTransactionRedoneEventArgs eventArgs):base(canvasDataContext,eventArgs) {

        }
    }

    /// <summary>
    /// 编辑事务已重做事件;
    /// </summary>
    public class CanvasEditTransactionRedoneEvent :PubSubEvent<CanvasEditTransactionRedoneEventArgs> {
    }

    public interface ICanvasEditTransactionRedoneEventHandler : IEventHandler<CanvasEditTransactionRedoneEventArgs> {

    }
}
