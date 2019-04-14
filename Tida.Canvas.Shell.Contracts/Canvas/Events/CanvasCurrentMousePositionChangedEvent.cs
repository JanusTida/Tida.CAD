using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 当前鼠标的工程数学坐标发生变化事件;
    /// </summary>
    public class CanvasCurrentMousePositionChangedEvent:PubSubEvent<ICanvasDataContext> {

    }

    public interface ICanvasCurrentMousePositionChangedEventHandler : IEventHandler<ICanvasDataContext> {

    }
}
