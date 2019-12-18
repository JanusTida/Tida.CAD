
using Tida.Canvas.Contracts;
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 编辑事务被呈递事件;
    /// </summary>
    public class CanvasEditTransactionCommitedEvent:PubSubEvent<(ICanvasDataContext canvasDataContext,IEditTransaction editTransaction)> {

    }

    public interface ICanvasEditTranactionCommitedEventHandler : IEventHandler<(ICanvasDataContext canvasDataContext, IEditTransaction editTransaction)> {

    }
}
