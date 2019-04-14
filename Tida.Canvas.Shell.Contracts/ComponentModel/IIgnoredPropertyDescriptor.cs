using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 应该忽略的属性描述器;
    /// </summary>
    public interface IIgnoredPropertyDescriptor {
        /// <summary>
        /// 原属性反射信息集合;
        /// </summary>
        IEnumerable<PropertyInfo> PropertyInfos { get; }

    }

    /// <summary>
    /// 指示应该忽略的属性元数据;
    /// </summary>
    public interface IIgnoredPropertyDescriptorMetaData {
        
        /// <summary>
        /// 是否能够应用至派生类型;
        /// </summary>
        bool Inheritable { get; }
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportIgnoredPropertyDescriptorAttribute : ExportAttribute, IIgnoredPropertyDescriptorMetaData {
        public ExportIgnoredPropertyDescriptorAttribute():base(typeof(IIgnoredPropertyDescriptor)) {

        }
        public bool Inheritable { get; set; }
    }
}
