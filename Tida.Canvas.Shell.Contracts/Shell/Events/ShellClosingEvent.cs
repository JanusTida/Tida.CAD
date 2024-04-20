
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Shell.Events {
    /// <summary>
    /// Shell正关闭时发生的事件;
    /// </summary>
    public class ShellClosingEvent:PubSubEvent<CancelEventArgs> {

    }

    public interface IShellClosingEventHandler:IEventHandler<CancelEventArgs> {

    }
}
