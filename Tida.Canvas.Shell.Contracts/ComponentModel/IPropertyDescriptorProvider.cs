using System.Collections.Generic;

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
