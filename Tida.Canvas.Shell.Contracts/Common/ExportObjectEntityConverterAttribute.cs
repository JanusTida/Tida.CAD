using Tida.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 原始数据与其等价数据的转换器 的 导出注解;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportObjectEntityConverterAttribute : ExportAttribute, IConverterMetaData {
        public ExportObjectEntityConverterAttribute() : base(typeof(IObjectEntityConverter)) {

        }
        public string PropertyName { get; set; }

        public string CollectionName { get; set; }

        public string[] Types { get; set; }
    }

    /// <summary>
    /// 数据转换器的元数据;
    /// </summary>
    public interface IConverterMetaData {
        /// <summary>
        /// 单个转换时将使用的属性名;
        /// </summary>
        string PropertyName { get; }
        
        /// <summary>
        /// 批量转换时,将使用的集合属性名;
        /// </summary>
        string CollectionName { get; }

        /// <summary>
        /// 分类名称约束;
        /// </summary>
        string[] Types { get; }
    }
}
