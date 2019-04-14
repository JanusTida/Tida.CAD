using Tida.Application.Contracts.Common;
using Tida.Canvas.Contracts;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 编辑事务被呈递事件;
    /// </summary>
    public class CanvasEditTransactionCommitedEvent:PubSubEvent<(ICanvasDataContext canvasDataContext,IEditTransaction editTransaction)> {

    }

    public interface ICanvasEditTranactionCommitedEventHandler : IEventHandler<(ICanvasDataContext canvasDataContext, IEditTransaction editTransaction)> {

    }
}
