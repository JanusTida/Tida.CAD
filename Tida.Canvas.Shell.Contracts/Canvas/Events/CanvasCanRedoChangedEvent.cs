
using Prism.Events;
using Tida.Canvas.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas.Events {
    public class CanvasCanRedoChangedEventArgs: CanvasEventArgs<CanRedoChangedEventArgs> {
        public CanvasCanRedoChangedEventArgs(ICanvasDataContext canvasDataContext, CanRedoChangedEventArgs args) : base(canvasDataContext, args) {

        }
    }

    public class CanvasCanRedoChangedEvent: PubSubEvent<CanvasCanRedoChangedEventArgs> {

    }

    public interface ICanvasCanRedoChangedEventHandler: IEventHandler<CanvasCanRedoChangedEventArgs> {

    }
}
