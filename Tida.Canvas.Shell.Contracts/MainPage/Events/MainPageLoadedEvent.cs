
using Prism.Events;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.MainPage.Events {
    /// <summary>
    /// 主页被加载事件;
    /// </summary>
    public class MainPageLoadedEvent:PubSubEvent {
    }

    public interface IMainPageLoadedEventHandler : IEventHandler {

    }
}
