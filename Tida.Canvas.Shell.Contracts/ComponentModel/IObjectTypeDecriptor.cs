using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 类型描述器;
    /// </summary>
    public interface  IObjectTypeDescriptor {
    
    }

    /// <summary>
    /// 类型描述器描述器;
    /// </summary>
    public interface IObjectTypeDescriptorMetaData {
        /// <summary>
        /// 类型描述语言名;
        /// </summary>
        string TypeNameKey { get; }

        /// <summary>
        /// 类型本体;
        /// </summary>
        Type Type { get; }
    }

    [MetadataAttribute,AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public sealed class ExportObjectTypeDescriptorAttribute : ExportAttribute,IObjectTypeDescriptorMetaData {
        public ExportObjectTypeDescriptorAttribute():base(typeof(IObjectTypeDescriptor)) {

        }

        public string TypeNameKey { get; set; }

        public Type Type { get; set; }
    }
}
