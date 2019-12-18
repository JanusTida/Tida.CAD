using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 停靠容器;
    /// </summary>
    public interface IDockingContainer:IDockingItem {
        
    }

    /// <summary>
    /// 停靠容器元数据;
    /// </summary>
    public interface IDockingContainerMetaData:IDockingItemMetaData {
        /// <summary>
        /// 初始停靠位置;
        /// </summary>
        DockingPosition InitDockingPosition { get; }

        /// <summary>
        /// 停靠服务GUID;
        /// </summary>
        string DockingServiceGUID { get; }

        /// <summary>
        /// 朝向;
        /// </summary>
        Orientation Orientation { get; }
    }

    /// <summary>
    /// 导出停靠容器注解;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExportDockingContainerAttribute : ExportAttribute,IDockingContainerMetaData {
        public ExportDockingContainerAttribute():base(typeof(IDockingContainer)) {
            
        }

        public DockingPosition InitDockingPosition { get; set; }

        public string DockingServiceGUID { get; set; }

        public string GUID { get; set; }

        public Orientation Orientation { get; set; }
    }
}
