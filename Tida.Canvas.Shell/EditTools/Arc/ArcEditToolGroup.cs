using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.EditTools;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Arc {
    /// <summary>
    /// 编辑组-圆弧;
    /// </summary>
    [Export(typeof(IEditToolGroup))]
    class ArcEditToolGroup : IEditToolGroup {
        public string GUID => EditToolGroup_Arc;

        public string ParentGUID => EditToolGroup_BasicEditor;

        public string GroupNameLanguageKey => Constants.EditToolGroupName_Arc;

        public int Order => EditToolGroupOrder_Arc;

        public string Icon => Constants.EditToolIcon_Arc;
    }
}
