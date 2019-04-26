using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.MainPage {
    /// <summary>
    /// 主停靠服务;
    /// </summary>
    public static class MainDockingService {
        private static IDockingService _current;
        public static IDockingService Current => _current ?? (_current = ServiceProvider.GetInstance<IDockingService>(Constants.DockingService_Main));
    }
}
