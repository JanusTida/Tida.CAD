using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Docking;

namespace Tida.Canvas.Shell.Contracts.MainPage {
    /// <summary>
    /// 主停靠服务;
    /// </summary>
    public static class MainDockingService {
        private static IDockingService _current;
        public static IDockingService Current => _current ?? (_current = ServiceProvider.GetInstance<IDockingService>(Constants.DockingService_Main));
    }
}
