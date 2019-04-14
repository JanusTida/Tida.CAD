using System.Collections.Generic;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 忽略属性描述器集合提供器;
    /// </summary>
    public interface IIgnoredPropertyDescriptorProvider {
        /// <summary>
        /// 对应的忽略属性描述器集合;
        /// </summary>
        IEnumerable<CreatedIgnoredPropertyDescriptor> IgnoredPropertyDescriptors { get; }
    }
}
