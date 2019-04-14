using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.StatusBar.Events {
    /// <summary>
    /// 状态初始化事件;
    /// </summary>
    public class StatusBarInitializeEvent : PubSubEvent<IStatusBarService> {

    }


    public interface IStatusBarInitializeEventHandler : IEventHandler<IStatusBarService> {

    }
}
