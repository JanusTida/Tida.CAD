using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 手动创建的编辑器描述器;
    /// </summary>
    public sealed class CreatedEditorDescriptor {
        public CreatedEditorDescriptor(IEditorDescriptor editorDescriptor,IEditorDescriptorMetaData metaData) {

            EditorDescriptor = editorDescriptor ?? throw new ArgumentNullException(nameof(editorDescriptor));

            EditorDescriptorMetaData = metaData ?? throw new ArgumentNullException(nameof(metaData));

        }


        public IEditorDescriptor EditorDescriptor { get; }

        public IEditorDescriptorMetaData EditorDescriptorMetaData { get; }

    }
}
