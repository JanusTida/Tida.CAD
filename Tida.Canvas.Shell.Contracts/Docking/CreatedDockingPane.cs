using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 手动创建的停靠区域;
    /// </summary>
    public class CreatedDockingPane {
        public CreatedDockingPane(IDockingPane dockingPane,IDockingPaneMetaData dockingPaneMetaData) {
            DockingPane = dockingPane ?? throw new ArgumentNullException(nameof(dockingPane));
            DockingPaneMetaData = dockingPaneMetaData ?? throw new ArgumentNullException(nameof(dockingPaneMetaData));
        }

        public IDockingPane DockingPane { get; }

        public IDockingPaneMetaData DockingPaneMetaData { get; }
    }
}
