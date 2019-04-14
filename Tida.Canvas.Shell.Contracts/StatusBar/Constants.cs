using System.Windows.Media;

namespace Tida.Canvas.Shell.Contracts.StatusBar {
    public static class Constants {
        /// <summary>
        /// 中空状态项的排序号;
        /// </summary>
        public const int StatusBarOrder_Indent = 512;
        public const int StatusBarOrder_Default = 128;

        public const string StatusBarItem_Default = nameof(StatusBarItem_Default);
        public const string StatusBarItem_Indent = nameof(StatusBarItem_Indent);

        public static Brush StatusBarItemForeground_Default => Brushes.White;
    }
}
