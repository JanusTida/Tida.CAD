
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.StatusBar.Events {
    /// <summary>
    /// 状态初始化事件;
    /// </summary>
    public class StatusBarInitializeEvent : PubSubEvent<IStatusBarService> {

    }


    public interface IStatusBarInitializeEventHandler : IEventHandler<IStatusBarService> {

    }
}
