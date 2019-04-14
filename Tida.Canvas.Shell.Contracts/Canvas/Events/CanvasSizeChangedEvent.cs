using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 画布尺寸发生变化时事件;
    /// </summary>
    public class CanvasSizeChangedEvent : PubSubEvent<ICanvasDataContext>{
    }

    public interface ICanvasSizeChangedEventHandler:IEventHandler<ICanvasDataContext> {

    }
}
