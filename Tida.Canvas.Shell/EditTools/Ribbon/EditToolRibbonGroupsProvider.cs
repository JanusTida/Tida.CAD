using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Tida.Canvas.Shell.EditTools.Ribbon {
    /// <summary>
    /// 编辑工具->Ribbon组的导出单元;
    /// </summary>
    [ExportRibbonGroupsProvider(Order = 256)]
    class EditToolRibbonGroupsProvider : IRibbonGroupsProvider {
        [ImportingConstructor]
        public EditToolRibbonGroupsProvider(
            [ImportMany]IEnumerable<IEditToolGroup> mefEditToolGroups) {

            _mefEditGroups = mefEditToolGroups;
        }
        private readonly IEnumerable<IEditToolGroup> _mefEditGroups;

        private List<CreatedRibbonGroup> _groups;
        public IEnumerable<CreatedRibbonGroup> Groups {
            get {
                if(_groups == null) {
                    InitializeGroups();
                }

                return _groups;
            }
        }

        private void InitializeGroups() {
            _groups = new List<CreatedRibbonGroup>();
            foreach (var editGroup in _mefEditGroups.OrderBy(p => p.Order)) {
                var attr = new ExportRibbonGroupAttribute {
                    GUID = editGroup.GUID,
                    Order = editGroup.Order,
                    ParentGUID = editGroup.ParentGUID ?? Contracts.Ribbon.Constants.RibbonTab_Tool,
                    HeaderLanguageKey = editGroup.GroupNameLanguageKey,
                    Icon = editGroup.Icon != null ? editGroup.Icon : null
                };
                var ribbonGroup = new CreatedRibbonGroup(new RibbonGroup(), attr);
                _groups.Add(ribbonGroup);
            }
            
        }
    }
}
