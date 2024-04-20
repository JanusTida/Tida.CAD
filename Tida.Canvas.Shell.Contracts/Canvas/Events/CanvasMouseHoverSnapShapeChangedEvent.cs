
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 当前的辅助图形发生了变化事件;
    /// </summary>
    public class CanvasMouseHoverSnapShapeChangedEvent:PubSubEvent<ICanvasDataContext> {

    }

    public interface ICanvasMouseHoverSnapShapeChangedEventHandler : IEventHandler<ICanvasDataContext> {

    }
}
