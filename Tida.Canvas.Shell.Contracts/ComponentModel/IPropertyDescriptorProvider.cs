using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 属性描述其提供器;
    /// </summary>
    public interface IPropertyDescriptorProvider {
        /// <summary>
        /// 对应的属性描述器集合;
        /// </summary>
        IEnumerable<CreatedPropertyDescriptor> PropertyDescriptors { get; }
    }
}
