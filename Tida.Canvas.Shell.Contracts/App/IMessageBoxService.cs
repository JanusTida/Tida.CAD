using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.App {
    public interface IMessageBoxService {
        void ShowError(string error);
        MessageBoxResult Show(string msg);
        MessageBoxResult Show(string msgText, MessageBoxButton button);
        MessageBoxResult Show(string msgText, string caption, MessageBoxButton button);
    }
    //
    // Summary:
    //     Specifies which message box button that a user clicks. System.Windows.MessageBoxResult
    //     is returned by the Overload:System.Windows.MessageBox.Show method.
    public enum MessageBoxResult {
        //
        // Summary:
        //     The message box returns no result.
        None = 0,
        //
        // Summary:
        //     The result value of the message box is OK.
        OK = 1,
        //
        // Summary:
        //     The result value of the message box is Cancel.
        Cancel = 2,
        //
        // Summary:
        //     The result value of the message box is Yes.
        Yes = 6,
        //
        // Summary:
        //     The result value of the message box is No.
        No = 7
    }
    //
    // Summary:
    //     Specifies the buttons that are displayed on a message box. Used as an argument
    //     of the Overload:System.Windows.MessageBox.Show method.
    public enum MessageBoxButton {
        //
        // Summary:
        //     The message box displays an OK button.
        OK = 0,
        //
        // Summary:
        //     The message box displays OK and Cancel buttons.
        OKCancel = 1,
        //
        // Summary:
        //     The message box displays Yes, No, and Cancel buttons.
        YesNoCancel = 3,
        //
        // Summary:
        //     The message box displays Yes and No buttons.
        YesNo = 4
    }

    /// <summary>
	/// Contains an <see cref="IMessageBoxService"/> instance
	/// </summary>
	public class MsgBoxService : GenericServiceStaticInstance<IMessageBoxService> {
        public static void ShowError(string error) => Current.ShowError(error);

        public static MessageBoxResult Show(string msg) => Current.Show(msg);

        public static MessageBoxResult Show(string msg, string tip) =>
            Current.Show(msg, tip,MessageBoxButton.OK);

        public static MessageBoxResult Show(string msg, string tip, MessageBoxButton msgButton) =>
            Current.Show(msg, tip, msgButton);

        public static MessageBoxResult Show(string msg, MessageBoxButton msgBtn) =>
            Current?.Show(msg, msgBtn) ?? MessageBoxResult.OK;

        /// <summary>
        /// 根据给定的语言键值,显示语言字符串;
        /// </summary>
        /// <param name="languageKey"></param>
        public static void ShowLanguageString(string languageKey) => Current?.Show(LanguageService.FindResourceString(languageKey));
    }
}
