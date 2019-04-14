using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 辅助是否可用发生了变化事件;
    /// </summary>
    public class CanvasIsSnapingEnabledChangedEvent : PubSubEvent<ICanvasDataContext> { 
    }
    
    public interface ICanvasIsSnapingEnabledChangedEventHandler : IEventHandler<ICanvasDataContext> {

    }
}
