using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using Tida.Canvas.Infrastructure.InteractionHandlers;
using static Tida.Canvas.Shell.StatusBar.Constants;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 动态输入状态栏项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class DynamicInputStatusBarItem : StatusBarCheckBoxItem {
        public DynamicInputStatusBarItem():base(StatusBarItem_DynamicInput) {
            this.Order = StatusBarOrder_DynamicInput;
            this.Content = LanguageService.FindResourceString(StatusBarText_DynamicInput);

            this.IsThreeState = false;

            IsChecked = DynamicInputInteractionHandler.IsEnabled;
        }

        protected override void OnIsCheckedChanged() {
            DynamicInputInteractionHandler.IsEnabled = IsChecked.Value;
        }
    }
}
