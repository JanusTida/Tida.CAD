using Tida.Canvas.Shell.Contracts.EditTools;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;
using static Tida.Canvas.Shell.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools {
    /// <summary>
    /// 编辑工具组——基本图形;
    /// </summary>
    [Export(typeof(IEditToolGroup))]
    class BasicEditToolGroup : IEditToolGroup {
        public string ParentGUID => null;

        public string GroupNameLanguageKey => EditToolGroupName_BasicEditor;

        public string GUID => EditToolGroup_BasicEditor;

        public string Icon => null;

        public int Order => EditToolGroupOrder_Basic;
    }
}
