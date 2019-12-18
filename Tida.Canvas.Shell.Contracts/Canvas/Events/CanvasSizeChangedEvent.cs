
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 画布尺寸发生变化时事件;
    /// </summary>
    public class CanvasSizeChangedEvent : PubSubEvent<ICanvasDataContext>{
    }

    public interface ICanvasSizeChangedEventHandler:IEventHandler<ICanvasDataContext> {

    }
}
