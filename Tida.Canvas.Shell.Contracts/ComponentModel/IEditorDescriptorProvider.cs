using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
