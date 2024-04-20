using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 文档停靠服务;本单位不唯一;
    /// </summary>
    public interface IDockingService {
        
        /// <summary>
        /// 停靠区域集合;
        /// </summary>
        IEnumerable<CreatedDockingPane> DockingPanes { get; }

        /// <summary>
        /// 停靠组集合;
        /// </summary>
        IEnumerable<CreatedDockingGroup> DockingPaneGroups { get; }

        /// <summary>
        /// 停靠容器集合;
        /// </summary>
        IEnumerable<CreatedDockingContainer> DockingContainers { get; }
        
        /// <summary>
        /// 添加停靠区域;
        /// </summary>
        /// <param name="dockingPane"></param>
        void AddPane(CreatedDockingPane dockingPane);

        /// <summary>
        /// 移除停靠区域;
        /// </summary>
        /// <param name="pane"></param>
        void RemovePane(CreatedDockingPane pane);

        /// <summary>
        /// 向文档区域中加入停靠区域;
        /// </summary>
        /// <param name="pane"></param>
        void AddPaneToDocument(CreatedDockingPane pane);

        /// <summary>
        /// 从文档区域中移除停靠区域;
        /// </summary>
        /// <param name="pane"></param>
        void RemovePaneFromDocument(CreatedDockingPane pane);
    }

    
}
