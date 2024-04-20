using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 动态创建的停靠组;
    /// </summary>
    public class CreatedDockingGroup {

        public CreatedDockingGroup(IDockingGroup dockingGroup,IDockingGroupMetaData dockingGroupMetaData) {
            DockingGroup = dockingGroup ?? throw new ArgumentNullException(nameof(dockingGroup));
            DockingGroupMetaData = dockingGroupMetaData ?? throw new ArgumentNullException(nameof(dockingGroupMetaData));
        }

        public IDockingGroup DockingGroup { get; }
        public IDockingGroupMetaData DockingGroupMetaData { get; }
    }
}
