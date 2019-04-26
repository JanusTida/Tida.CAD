using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.StatusBar.Constants;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 辅助是否可用状态栏项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class CanvasSnapingEnabledStatusBarItem : StatusBarCheckBoxItem {
        public CanvasSnapingEnabledStatusBarItem():base(StatusBarItem_SnappingEnabled) {
            this.Order = StatusBarOrder_SnapingEnabled;
            this.Content = LanguageService.FindResourceString(StatusBarText_IsSnapingEnabled);

            //建立双向"绑定";
            CommonEventHelper.GetEvent<CanvasIsSnapingEnabledChangedEvent>().Subscribe(CanvasDataContext_IsSnapingEnabledChanged);
            
            IsThreeState = false;

            IsChecked = true;

            ValidateChecked();
        }

        private void CanvasDataContext_IsSnapingEnabledChanged(ICanvasDataContext canvasDataContext) {

            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            IsChecked = canvasDataContext.IsSnapingEnabled;
        }

        protected override void OnIsCheckedChanged() => ValidateChecked();
        

        /// <summary>
        /// 辅助可用状态同步到画布上下文;
        /// </summary>
        private void ValidateChecked() {
            if (IsChecked == null) {
                return;
            }
            CanvasService.CanvasDataContext.IsSnapingEnabled = IsChecked.Value;
        }
    }
}
