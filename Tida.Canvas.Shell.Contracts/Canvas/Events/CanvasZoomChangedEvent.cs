
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 缩放比例发生变化事件;
    /// </summary>
    public class CanvasZoomChangedEvent:PubSubEvent<ICanvasDataContext> {
    }

    public interface ICanvasZoomChangedEventHandler:IEventHandler<ICanvasDataContext> {

    }
}
