using Tida.Application.Contracts.Common;
using Prism.Events;

namespace Tida.Canvas.Shell.Contracts.MainPage.Events {
    /// <summary>
    /// 主页被加载事件;
    /// </summary>
    public class MainPageLoadedEvent:PubSubEvent {
    }

    public interface IMainPageLoadedEventHandler : IEventHandler {

    }
}
