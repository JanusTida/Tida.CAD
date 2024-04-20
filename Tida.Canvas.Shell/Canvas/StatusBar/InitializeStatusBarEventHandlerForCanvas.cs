using Tida.Canvas.Shell.Contracts.App;

using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.StatusBar;
using Tida.Canvas.Shell.Contracts.StatusBar.Events;
using System.ComponentModel.Composition;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 状态栏初始化时加入画布相关信息及调节选项;
    /// </summary>
    [Export(typeof(IStatusBarInitializeEventHandler))]
    class InitializeStatusBarEventHandlerForCanvas : IStatusBarInitializeEventHandler {
        public bool IsEnabled => true;
        public int Sort => 4;

        public void Handle(IStatusBarService statusBarService) {
            if(statusBarService == null) {
                return;
            }

            //画布当前鼠标位置发生变化时,通知状态栏;
            CommonEventHelper.GetEvent<CanvasCurrentMousePositionChangedEvent>().Subscribe(CanvasDataContext_CurrentMousePositionChanged);
            
        }

        /// <summary>
        /// 画布当前鼠标位置发生变化时,通知状态栏;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasDataContext_CurrentMousePositionChanged(ICanvasDataContext canvasDataContext) {
            var currentPosition = CanvasService.CanvasDataContext.CurrentMousePosition;
            if(currentPosition == null) {
                return;
            }

            StatusBarService.Report(
                LanguageService.TryGetStringWithFormat(
                    Constants.LanguageFormat_CurrentMousePosition,
                    currentPosition.X.ToString("F4"),
                    currentPosition.Y.ToString("F4")
                )
            );
        }
    }
}
