using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 编辑器描述器;
    /// </summary>
    public interface IEditorDescriptor {
        
    }

    /// <summary>
    /// 编辑器描述器元数据;
    /// </summary>
    public interface IEditorDescriptorMetaData {
        /// <summary>
        /// 编辑器类型,该类型必须具备一个无参的公开构造方法。
        /// </summary>
        Type EditorType { get; }

        /// <summary>
        /// 类型唯一标识;
        /// </summary>
        string TypeGUID { get; }
        
    }

    /// <summary>
    ///  导出编辑器描述器注解;
    /// </summary>
    [MetadataAttribute,AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public sealed class ExportEditorDescriptorAttribute : ExportAttribute,IEditorDescriptorMetaData {
        public ExportEditorDescriptorAttribute():base(typeof(IEditorDescriptor)) {

        }

        public Type EditorType { get; set; }
        public string TypeGUID { get; set; }
    }
}
