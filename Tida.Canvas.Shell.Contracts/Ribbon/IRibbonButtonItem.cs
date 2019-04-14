using System.Windows.Input;

namespace Tida.Canvas.Shell.Contracts.Ribbon {

    /// <summary>
    /// Ribbon按钮项;
    /// </summary>
    public interface IRibbonButtonItem:IRibbonItem {
        /// <summary>
        /// Icon;   
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// 命令;
        /// </summary>
        ICommand Command { get; }

        /// <summary>
        /// 显示名;
        /// </summary>
        string HeaderLanguageKey { get; }
    }

    
}
