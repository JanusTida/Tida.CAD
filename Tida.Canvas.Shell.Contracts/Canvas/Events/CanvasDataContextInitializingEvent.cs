
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 画布初始化事件;
    /// </summary>
    public class CanvasDataContextInitializingEvent:PubSubEvent<ICanvasDataContext> {
    }

    public interface ICanvasDataContextInitializingEventHandler : IEventHandler<ICanvasDataContext> {

    }
}
