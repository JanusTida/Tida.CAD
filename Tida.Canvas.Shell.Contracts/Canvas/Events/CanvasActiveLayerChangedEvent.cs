using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 当前活跃图层发生变化事件;
    /// </summary>
    public class CanvasActiveLayerChangedEventArgs : CanvasEventArgs<ValueChangedEventArgs<CanvasLayer>> {
        public CanvasActiveLayerChangedEventArgs(ICanvasDataContext canvasDataContext,ValueChangedEventArgs<CanvasLayer> args):base(canvasDataContext, args) {

        }
    }

    /// <summary>
    /// 活跃图层发生变化事件;
    /// </summary>
    public class CanvasActiveLayerChangedEvent:PubSubEvent<CanvasActiveLayerChangedEventArgs> {
    }

    public interface ICanvasActiveLayerChangedEventHandler : IEventHandler<CanvasActiveLayerChangedEventArgs> {

    }

}
