using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Setting;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.Constants;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 状态栏项-极轴追踪;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class AxisTrackingEnabledStatusBarItem: StatusBarCheckBoxItem {
        public AxisTrackingEnabledStatusBarItem():base(Constants.StatusBarItem_AxisTrackingEnabled) {
            this.Order = Constants.StatusBarOrder_AxisTrackingEnabled;
            this.Content = LanguageService.FindResourceString(Constants.StatusBarText_AxisTrackingEnabled);

            IsThreeState = false;

            IsChecked = false;

            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            IsChecked = section.GetAttribute<bool>(SettingName_AxisTrackingEnabled);
        }

        protected override void OnIsCheckedChanged() {
            if(IsChecked == null) {
                return;
            }

            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            section.SetAttribute(SettingName_AxisTrackingEnabled, IsChecked.Value);
        }
    }
}
