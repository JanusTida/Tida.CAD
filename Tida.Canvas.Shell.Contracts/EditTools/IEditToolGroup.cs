namespace Tida.Canvas.Shell.Contracts.EditTools {
    /// <summary>
    /// 编辑工具组;
    /// </summary>
    public interface IEditToolGroup {
        /// <summary>
        /// 唯一标识;
        /// </summary>
        string GUID { get; }

        /// <summary>
        /// 父编辑组的唯一标识,
        /// 为空,比如本类为"圆";
        /// 不为空,比如父对象为"基本图形";
        /// </summary>
        string ParentGUID { get; }

        /// <summary>
        /// 编辑工具组名的语言键名;
        /// </summary>
        string GroupNameLanguageKey { get; }

        /// <summary>
        /// 排序;
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 图标;
        /// </summary>
        string Icon { get; }
    }
}
