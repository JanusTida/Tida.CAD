using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 动态创建的停靠容器;
    /// </summary>
    public class CreatedDockingContainer {
        public CreatedDockingContainer(IDockingContainer dockingContainer,IDockingContainerMetaData dockingContainerMetaData) {
            DockingContainer = dockingContainer ?? throw new ArgumentNullException(nameof(dockingContainer));
            DockingContainerMetaData = dockingContainerMetaData ?? throw new ArgumentNullException(nameof(dockingContainerMetaData));
        }

        public IDockingContainer DockingContainer { get; }
        public IDockingContainerMetaData DockingContainerMetaData { get; }
    }
}
