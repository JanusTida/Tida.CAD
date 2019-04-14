using System.Collections.Generic;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 编辑器描述器提供者;
    /// </summary>
    public interface IEditorDescriptorProvider {
        /// <summary>
        /// 对应的编辑器描述器;
        /// </summary>
        IEnumerable<CreatedEditorDescriptor> EditorDescriptors { get; }
    }
}
