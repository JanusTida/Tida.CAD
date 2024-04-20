
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Shell.Events {
    /// <summary>
    /// Shell初始化事件;
    /// </summary>
    public class ShellInitializingEvent:PubSubEvent {

    }

    public interface IShellInitializingEventHandler :IEventHandler {

    }
}
