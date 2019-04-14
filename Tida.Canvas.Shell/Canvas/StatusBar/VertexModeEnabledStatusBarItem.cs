using Tida.Canvas.Base.InteractionHandlers;
using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Setting;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using static Tida.Canvas.Base.Constants;
using Tida.Canvas.Events;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 正交模式状态栏项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class VertexModeEnabledStatusBarItem : StatusBarCheckBoxItem {
        public VertexModeEnabledStatusBarItem():base(Constants.StatusBarItem_VertexMode) {
            this.Order = Constants.StatusBarOrder_VertexMode;
            this.Content = LanguageService.FindResourceString(Constants.StatusBarText_VertextMode);
            
            this.IsThreeState = false;

            IsChecked = VertextInteractionHandler.IsEnabled;

            VertextInteractionHandler.IsEnabledChanged += VertextInteractionHandler_IsEnabledChanged;
        }

        private void VertextInteractionHandler_IsEnabledChanged(object sender, ValueChangedEventArgs<bool> e) {
            if(IsChecked.Value == VertextInteractionHandler.IsEnabled) {
                return;
            }
            IsChecked = VertextInteractionHandler.IsEnabled;
        }

        protected override void OnIsCheckedChanged() {
            VertextInteractionHandler.IsEnabled = IsChecked.Value;
            base.OnIsCheckedChanged();
        }
        
    }
}
