using Tida.Canvas.Infrastructure.EditTools;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.EditTools;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;
using static Tida.Canvas.Shell.EditTools.Constants;

namespace Tida.Canvas.Shell.EditTools.Measure {
    /// <summary>
    /// 编辑工具组——基本图形;
    /// </summary>
    [Export(typeof(IEditToolGroup))]
    class MeasureEditToolGroup : IEditToolGroup {
        public string GUID => EditToolGroup_Measure;

        public string ParentGUID => null;

        public string GroupNameLanguageKey => EditToolGroupName_Measure;

        public int Order => EditToolGroupOrder_Measure;

        public string Icon => null;
    }

    
    
    
}
