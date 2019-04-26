using Tida.Canvas.Shell.Contracts.EditTools;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;
using static Tida.Canvas.Shell.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
    [Export(typeof(IEditToolGroup))]
    class LineEditToolGroup : IEditToolGroup {
        public string GUID => EditToolGroup_Line;

        public string ParentGUID => EditToolGroup_BasicEditor;

        public string GroupNameLanguageKey => EditToolGroupName_Line;

        public int Order => EditToolGroupOrder_Line;

        public string Icon => EditToolIcon_Line;
    }
}
