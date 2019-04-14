using Tida.Application.Contracts.Common;
using Prism.Events;
using System.ComponentModel;

namespace Tida.Canvas.Shell.Contracts.Shell.Events {
    /// <summary>
    /// Shell正关闭时发生的事件;
    /// </summary>
    public class ShellClosingEvent:PubSubEvent<CancelEventArgs> {

    }

    public interface IShellClosingEventHandler:IEventHandler<CancelEventArgs> {

    }
}
