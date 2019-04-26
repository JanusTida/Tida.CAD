using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Setting;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.Constants;
using static Tida.Canvas.Shell.StatusBar.Constants;
using Tida.Canvas.Infrastructure.Snaping.Rules;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 状态栏项-极轴追踪;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class AxisTrackingEnabledStatusBarItem: StatusBarCheckBoxItem {
        public AxisTrackingEnabledStatusBarItem():base(StatusBarItem_AxisTrackingEnabled) {
            this.Order = StatusBarOrder_AxisTrackingEnabled;
            this.Content = LanguageService.FindResourceString(StatusBarText_AxisTrackingEnabled);

            IsThreeState = false;

            IsChecked = false;

            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            IsChecked = section.GetAttribute<bool>(SettingName_AxisTrackingEnabled);

            RefreshEnabled();
        }

        protected override void OnIsCheckedChanged() {
            if(IsChecked == null) {
                return;
            }

            RefreshEnabled();
        }

        /// <summary>
        /// 刷新是否能够使用极轴追踪;
        /// </summary>
        private void RefreshEnabled() {
            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            section.SetAttribute(SettingName_AxisTrackingEnabled, IsChecked.Value);
            AxisTrackingSnapRule.IsEnabled = IsChecked.Value;
        }
    }
}
