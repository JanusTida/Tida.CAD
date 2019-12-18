using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 停靠组;
    /// </summary>
    public interface IDockingGroup : IDockingItem {
        
    }

    /// <summary>
    /// 停靠组圆数据;
    /// </summary>
    public interface IDockingGroupMetaData : IDockingItemMetaData,IHaveOrder {
        /// <summary>
        /// Container 的唯一标识;
        /// </summary>
        string ContainerGUID { get; }


        /// <summary>
        /// 是否无样式;
        /// </summary>
        bool NoStyle { get; }
        
    }

    /// <summary>
    /// 导出停靠组注解;
    /// </summary>
    [MetadataAttribute,AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportDockingGroupAttribute : ExportAttribute,IDockingGroupMetaData {
        public ExportDockingGroupAttribute():base(typeof(IDockingGroup)) {

        }

        public string ContainerGUID { get; set; }

        public bool NoStyle { get; set; }

        public string GUID { get; set; }

        public int Order { get; set; }
    }
}
