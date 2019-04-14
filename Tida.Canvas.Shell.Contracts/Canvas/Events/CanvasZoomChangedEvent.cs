using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 缩放比例发生变化事件;
    /// </summary>
    public class CanvasZoomChangedEvent:PubSubEvent<ICanvasDataContext> {
    }

    public interface ICanvasZoomChangedEventHandler:IEventHandler<ICanvasDataContext> {

    }
}
