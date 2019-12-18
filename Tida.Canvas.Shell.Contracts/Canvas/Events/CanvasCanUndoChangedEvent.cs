
using Prism.Events;
using Tida.Canvas.Events;
using Tida.Canvas.Shell.Contracts.Common;

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
