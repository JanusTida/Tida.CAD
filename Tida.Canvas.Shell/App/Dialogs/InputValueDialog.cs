using Tida.Canvas.Shell.App.ViewModels;
using Tida.Canvas.Shell.App.Views;
using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.App.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.App.Dialogs {
    /// <summary>
    /// 输入字符串对话框;
    /// </summary>
    public static class InputValueDialog {
        /// <summary>
        /// 请求输入;
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="des">对话框内容</param>
        /// <returns></returns>
        public static string Show(GetInputValueSetting setting = null) {

            var vm = new InputValueWindowViewModel(setting?.InputChecker) {
                Title = setting?.Title,
                Val = setting?.Val,
                Desc = setting?.Desc
            };
            

            var window = new InputValueWindow {
                DataContext = vm
            };

            vm.ClosedRequest += delegate { window.Close(); };

            window.ShowDialog(setting.Owner);

            if (vm.Confirmed) {
                return vm.Val;
            }

            return null;
        }
    }
}
