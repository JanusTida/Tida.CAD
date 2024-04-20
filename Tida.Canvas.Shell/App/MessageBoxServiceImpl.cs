using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.App;

namespace Tida.Canvas.Shell.App {
    [Export(typeof(IMessageBoxService))]
    class MessageBoxServiceImpl : IMessageBoxService { 
        public MessageBoxResult Show(string msg) {
            return Show(msg, LanguageService.FindResourceString(Constants.WindowTitle_Tip), MessageBoxButton.OK);
        }

        public void ShowError(string error) {
            Show(error);
        }

        public MessageBoxResult Show(string msgText, MessageBoxButton button) {
            var res = Show(msgText, LanguageService.FindResourceString(Constants.WindowTitle_Tip), button);
            return res;
        }

        public MessageBoxResult Show(string msgText, string caption, MessageBoxButton button) {
            var msgResult = System.Windows.MessageBox.Show(msgText, caption,FromMsgBtnToNativeBtn(button));
            return FromNativeResultToMsgResult(msgResult);
        }

        /// <summary>
        /// 从<see cref="MessageBoxButton"/>转换为WPF<see cref="System.Windows.MessageBoxButton"/>
        /// </summary>
        /// <returns></returns>
        private static System.Windows.MessageBoxButton FromMsgBtnToNativeBtn(MessageBoxButton msgBtn) {
            switch (msgBtn) {
                case MessageBoxButton.OK:
                    return System.Windows.MessageBoxButton.OK;
                case MessageBoxButton.OKCancel:
                    return System.Windows.MessageBoxButton.OKCancel;
                case MessageBoxButton.YesNoCancel:
                    return System.Windows.MessageBoxButton.YesNoCancel;
                case MessageBoxButton.YesNo:
                    return System.Windows.MessageBoxButton.YesNo;
                default:
                    return System.Windows.MessageBoxButton.OK;
            }
        }

        /// <summary>
        /// 从WPF<see cref="System.Windows.MessageBoxResult"/>转换为<see cref="MessageBoxResult"/>
        /// </summary>
        /// <param name="msgResult"></param>
        /// <returns></returns>
        private static MessageBoxResult FromNativeResultToMsgResult(System.Windows.MessageBoxResult msgResult) {
            switch (msgResult) {
                case System.Windows.MessageBoxResult.None:
                    return MessageBoxResult.None;
                case System.Windows.MessageBoxResult.OK:
                    return MessageBoxResult.OK;
                case System.Windows.MessageBoxResult.Cancel:
                    return MessageBoxResult.Cancel;
                case System.Windows.MessageBoxResult.Yes:
                    return MessageBoxResult.Yes;
                case System.Windows.MessageBoxResult.No:
                    return MessageBoxResult.No;
                default:
                    return MessageBoxResult.None;
            }
        }
    }
}
