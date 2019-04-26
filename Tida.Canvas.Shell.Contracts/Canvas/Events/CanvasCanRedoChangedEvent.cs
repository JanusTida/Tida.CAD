using Tida.Application.Contracts.Common;
using Prism.Events;
using Tida.Canvas.Events;

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
