
using Tida.Canvas.Events;
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 绘制对象选择状态发生更改事件参数;
    /// </summary>
    public class CanvasDrawObjectSelectedChangedEventArgs : CanvasEventArgs<DrawObjectSelectedChangedEventArgs> {
        public CanvasDrawObjectSelectedChangedEventArgs(ICanvasDataContext canvasDataContext,DrawObjectSelectedChangedEventArgs args):base(canvasDataContext,args) {
            
        }
    }

    /// <summary>
    /// 绘制对象选定状态发生了变化事件;
    /// </summary>
    public class CanvasDrawObjectIsSelectedChangedEvent:PubSubEvent<CanvasDrawObjectSelectedChangedEventArgs> {

    }

    public interface ICanvasDrawObjectIsSelectedChangedEventHandler:IEventHandler<CanvasDrawObjectSelectedChangedEventArgs> {

    }

}
