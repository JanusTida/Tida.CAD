using CDO.Common.Canvas.Shell.Contracts.EditTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDO.Common.Canvas.Shell.EditTools {
    /// <summary>
    /// 编辑工具组——测量;
    /// </summary>
    [Export(typeof(IEditToolGroup))]
    class MeasureEditGroup: IEditToolGroup {
        public string GroupGUID => null;

        public string GroupNameLanguageKey => Constants.EditToolGroupName_Measure;

        public string GUID => Constants.EditToolGroupGUID_Measure;

        public string Icon => null;

        public int Order => 2048;
    }
}
