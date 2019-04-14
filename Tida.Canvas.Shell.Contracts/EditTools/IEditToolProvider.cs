using Tida.Canvas.Contracts;
using System;

namespace Tida.Canvas.Shell.Contracts.EditTools {
    /// <summary>
    /// 编辑工具提供器;
    /// </summary>
    public interface IEditToolProvider {
        /// <summary>
        /// 创建一个新的编辑工具;
        /// </summary>
        /// <returns></returns>
        EditTool CreateEditTool();

        /// <summary>
        /// 能否进行创建;
        /// </summary>
        /// <returns></returns>
        bool CanCreate { get; }

        /// <summary>
        /// 能否创建发生了变化;
        /// </summary>
        event EventHandler CanCreateChanged;

        /// <summary>
        /// 检查编辑工具是否是来自于此编辑工具提供器;
        /// </summary>
        /// <param name="editTool"></param>
        /// <returns></returns>
        bool ValidateFromThis(EditTool editTool);
    }
}
