using Tida.Canvas.Shell.Contracts.EditTools;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Rectangle {
    /// <summary>
    /// 编辑组——矩形;
    /// </summary>
    [Export(typeof(IEditToolGroup))]
    class RectangleEditToolGroup : IEditToolGroup {
        public string ParentGUID => EditToolGroup_BasicEditor;

        public string GroupNameLanguageKey => Constants.EditToolGroupName_Rectangle;

        public string GUID => EditToolGroup_Rectangle;

        public string Icon => Constants.EditToolIcon_Rectangle;

        public int Order => 16;
    }
}
