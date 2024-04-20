
using Tida.Canvas.Events;
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 绘制对象被移除事件参数;
    /// </summary>
    public class CanvasDrawObjectsRemovedEventArgs : CanvasEventArgs<DrawObjectsRemovedEventArgs> {
        public CanvasDrawObjectsRemovedEventArgs(ICanvasDataContext canvasDataContext,DrawObjectsRemovedEventArgs drawObjectRemovedEventArgs) :base(canvasDataContext,drawObjectRemovedEventArgs){

        }
    }

    public class CanvasDrawObjectsRemovedEvent:PubSubEvent<CanvasDrawObjectsRemovedEventArgs> {

    }

    public interface ICanvasDrawObjectsRemovedEventHandler : IEventHandler<CanvasDrawObjectsRemovedEventArgs> {

    }
}
