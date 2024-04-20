
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 当前鼠标的工程数学坐标发生变化事件;
    /// </summary>
    public class CanvasCurrentMousePositionChangedEvent:PubSubEvent<ICanvasDataContext> {

    }

    public interface ICanvasCurrentMousePositionChangedEventHandler : IEventHandler<ICanvasDataContext> {

    }
}
