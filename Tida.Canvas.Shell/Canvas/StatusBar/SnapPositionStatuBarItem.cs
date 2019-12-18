using Tida.Canvas.Shell.Contracts.App;

using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.StatusBar.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 状态栏辅助信息项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class SnapPositionStatuBarItem: StatusBarTextItem {
        public SnapPositionStatuBarItem():base(StatusBarItem_SnapPosition) {
            this.Text = LanguageService.FindResourceString(StatusBarText_CurrentSnapPosition);
            this.Order = StatusBarOrder_SnapingPosition;

            CommonEventHelper.GetEvent<CanvasMouseHoverSnapShapeChangedEvent>().Subscribe(CanvasDataContext_MouseHoverSnapShapeChanged);
            
        }

        private void CanvasDataContext_MouseHoverSnapShapeChanged(ICanvasDataContext canvasDataContext) {
            var currentSnapShape = CanvasService.CanvasDataContext?.MouseHoverSnapShape;
            if(currentSnapShape != null && currentSnapShape.Position != null) {
                Text = LanguageService.FindResourceString(StatusBarText_CurrentSnapPosition) + $"({currentSnapShape.Position.X:F3},{currentSnapShape.Position.Y:F3})";
            }
            else {
                Text = LanguageService.FindResourceString(StatusBarText_CurrentSnapPosition);
            }

        }
    }
}
