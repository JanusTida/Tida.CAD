using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.Shell.Events {
    /// <summary>
    /// Shell初始化事件;
    /// </summary>
    public class ShellInitializingEvent:PubSubEvent {

    }

    public interface IShellInitializingEventHandler :IEventHandler {

    }
}
