using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 当前的辅助图形发生了变化事件;
    /// </summary>
    public class CanvasMouseHoverSnapShapeChangedEvent:PubSubEvent<ICanvasDataContext> {

    }

    public interface ICanvasMouseHoverSnapShapeChangedEventHandler : IEventHandler<ICanvasDataContext> {

    }
}
