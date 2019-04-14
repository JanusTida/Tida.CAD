using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 状态栏缩放信息项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class ZoomStatusBarItem:StatusBarTextItem {
        public ZoomStatusBarItem():base(Constants.StatusBarItem_Zoom) {
            Order = Constants.StatusBarOrder_Zoom;
            CommonEventHelper.GetEvent<CanvasZoomChangedEvent>().Subscribe(CanvasDataContext_ZoomChanged);
        }

        private readonly string _statusBarText_Zoom = 
            LanguageService.FindResourceString(Constants.StatusBarText_Zoom);

        private void CanvasDataContext_ZoomChanged(ICanvasDataContext canvasDataContext) {
            var zoom = CanvasService.CanvasDataContext.Zoom;

            Text = $"{_statusBarText_Zoom}{zoom:F3}";
        }
    }
}
