using System;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon下拉项契约;
    /// </summary>
    public interface IRibbonDropDownButtonItem:IRibbonItem {
        
    }

    public interface IRibbonDropDownButtonItemMetaData {
        /// <summary>
        /// Icon;   
        /// </summary>
        Uri Icon { get; }

        /// <summary>
        /// 显示名;
        /// </summary>
        string HeaderLanguageKey { get; }
    }
}
