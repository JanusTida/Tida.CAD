using System.Windows.Input;

namespace Tida.Canvas.Shell.Contracts.EditTools {
    /// <summary>
    /// 编辑工具提供器元数据;
    /// </summary>
    public interface IEditToolProviderMetaData {
        /// <summary>
        /// 编辑工具组唯一标识;
        /// </summary>
        /// <example>如"基本图形"或者"圆"<see cref="IEditToolGroup.GUID"/></example>
        string GroupGUID { get; }

        /// <summary>
        /// 唯一标识;
        /// </summary>
        string GUID { get; }

        /// <summary>
        /// 编辑图标的地址;
        /// </summary>
        string IconResource { get; }

        /// <summary>
        /// 编辑工具名的语言键名;
        /// </summary>
        string EditToolLanguageKey { get; }

        /// <summary>
        /// 排序;
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 快捷按键;
        /// </summary>
        Key Key { get; }

        /// <summary>
        /// 修改键;
        /// </summary>
        ModifierKeys ModifierKeys { get; }
        
        /// <summary>
        /// 快捷命令;
        /// </summary>
        //string HotText { get; }
    }
}
