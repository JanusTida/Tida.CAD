using Tida.Canvas.Base.InteractionHandlers;
using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Setting;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.Constants;


namespace Tida.Canvas.Shell.Canvas.StatusBar
{
    /// <summary>
    /// 动态输入状态栏项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class DynamicInputStatusBarItem : StatusBarCheckBoxItem {
        public DynamicInputStatusBarItem():base(Constants.StatusBarItem_DynamicInput) {
            this.Order = Constants.StatusBarOrder_DynamicInput;
            this.Content = LanguageService.FindResourceString(Constants.StatusBarText_DynamicInput);

            this.IsThreeState = false;

            IsChecked = DynamicInputInteractionHandler.IsEnabled;
        }

        protected override void OnIsCheckedChanged() {
            DynamicInputInteractionHandler.IsEnabled = IsChecked.Value;
        }
    }
}
