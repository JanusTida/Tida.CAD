using Tida.Util.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ComponentModel {
    /// <summary>
    /// 属性描述器;
    /// </summary>
    public interface IPropertyDescriptor {
        /// <summary>
        /// 原属性反射信息;
        /// </summary>
        PropertyInfo PropertyInfo { get; }


    }

    /// <summary>
    /// 属性描述器元数据;
    /// </summary>
    public interface IPropertyDescriptorMetaData {
        /// <summary>
        /// 显示属性语言键名;
        /// </summary>
        string DisplayNameKey { get; }

        /// <summary>
        /// 是否是只读的属性;
        /// </summary>
        bool IsReadOnly { get; }
        
        /// <summary>
        /// 描述键名;
        /// </summary>
        string DescriptionNameKey { get; }

        /// <summary>
        /// 分类键名;
        /// </summary>
        string CategoryNameKey { get; }

        /// <summary>
        /// 编辑器类型标识(可选);
        /// </summary>
        string EditorTypeGUID { get; }

        /// <summary>
        /// 编辑器目标属性(可选);;
        /// </summary>
        string EditorTargetProperty { get; }

        /// <summary>
        /// 编辑器的显示形式;
        /// </summary>
        EditorStyle EditorStyle { get; }

        /// <summary>
        /// 是否能够应用至派生类型;
        /// </summary>
        bool Inheritable { get; }
    }

    /// <summary>
    /// 导出属性描述器注解;
    /// </summary>
    [MetadataAttribute,AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public sealed class ExportPropertyDescriptorAttribute:ExportAttribute,IPropertyDescriptorMetaData {
        public ExportPropertyDescriptorAttribute():base(typeof(IPropertyDescriptor)) {
            
        }
        
        /// <summary>
        /// 显示属性语言键名;
        /// </summary>
        public string DisplayNameKey { get; set; } 
        
        /// <summary>
        /// 是否是只读的属性;
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 描述键名;
        /// </summary>
        public string DescriptionNameKey { get; set; }

        /// <summary>
        /// 分类键名;
        /// </summary>
        public string CategoryNameKey { get; set; }

        /// <summary>
        /// 编辑器类型标识;
        /// </summary>
        public string EditorTypeGUID { get; set; }

        /// <summary>
        /// 目标属性名称;
        /// </summary>
        public string EditorTargetProperty { get; set; }

        /// <summary>
        /// 编辑器的显示形式;
        /// </summary>
        public EditorStyle EditorStyle { get; set; }

        /// <summary>
        /// 是否能够应用至派生类型;
        /// </summary>
        public bool Inheritable { get; set; }

        
    }
}
