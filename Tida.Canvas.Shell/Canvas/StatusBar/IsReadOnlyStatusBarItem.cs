using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Setting;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.Constants;
using static Tida.Canvas.Shell.StatusBar.Constants;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 状态栏项——只读;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class IsReadOnlyStatusBarItem: StatusBarCheckBoxItem {
        public IsReadOnlyStatusBarItem():base(StatusBarItem_IsReadOnly) {
            this.Order = StatusBarOrder_IsReadOnly;
            this.Content = LanguageService.FindResourceString(StatusBarText_IsReadOnly);

            IsThreeState = false;

            IsChecked = false;

            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            IsChecked = section.GetAttribute<bool>(SettingName_IsReadOnly);
        }

        protected override void OnIsCheckedChanged() {
            if(IsChecked == null) {
                return;
            }

            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            section.SetAttribute(SettingName_IsReadOnly, IsChecked.Value);

            CanvasService.CanvasDataContext.IsReadOnly = IsChecked.Value;
        }
    }
}
