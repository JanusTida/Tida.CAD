using Tida.Application.Contracts.Common;
using Tida.Canvas.Events;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 绘制对象被添加事件参数;
    /// </summary>
    public class CanvasDrawObjectsAddedEventArgs : CanvasEventArgs<DrawObjectsAddedEventArgs> {
        public CanvasDrawObjectsAddedEventArgs(ICanvasDataContext canvasDataContext,DrawObjectsAddedEventArgs drawObjectAddedEventArgs) :base(canvasDataContext,drawObjectAddedEventArgs){

        }
    }

    public class CanvasDrawObjectsAddedEvent:PubSubEvent<CanvasDrawObjectsAddedEventArgs> {

    }

    public interface ICanvasDrawObjectsAddedEventHandler : IEventHandler<CanvasDrawObjectsAddedEventArgs> {

    }
}
