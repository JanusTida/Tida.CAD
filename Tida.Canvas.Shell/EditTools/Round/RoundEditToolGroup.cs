using Tida.Canvas.Shell.Contracts.EditTools;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Round {
    /// <summary>
    /// 编辑组——圆;
    /// </summary>
    [Export(typeof(IEditToolGroup))]
    class RoundEditToolGroup : IEditToolGroup {
        public string ParentGUID => EditToolGroup_BasicEditor;

        public string GroupNameLanguageKey => Constants.EditToolGroupName_Round;

        public string GUID => EditToolGroup_Round;

        public string Icon => Constants.EditToolIcon_Round;

        public int Order => EditToolGroupOrder_Round;
    }
}
