using Tida.Application.Contracts.Common;
using Tida.Canvas.Events;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    /// <summary>
    /// 可撤销状态发生了变化事件参数;
    /// </summary>
    public class CanvasCanUndoChangedEventArgs: CanvasEventArgs<CanUndoChangedEventArgs> {
        public CanvasCanUndoChangedEventArgs(ICanvasDataContext canvasDataContext, CanUndoChangedEventArgs args) : base(canvasDataContext, args) {

        }
    }

    public sealed class CanvasCanUndoChangedEvent : PubSubEvent<CanvasCanUndoChangedEventArgs> {

    }

    public interface ICanvasCanUndoChangedEventHandler : IEventHandler<CanvasCanUndoChangedEventArgs> {

    }
}
