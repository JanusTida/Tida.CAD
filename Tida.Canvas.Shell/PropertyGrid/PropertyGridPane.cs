using Tida.Application.Contracts.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.MainPage.Constants;
using static Tida.Canvas.Shell.CommandOutput.Constants;
using System.ComponentModel.Composition;
using Tida.Application.Contracts.App;

namespace Tida.Canvas.Shell.PropertyGrid {
  
    [ExportDockingPane(CanUserClose = false, CanFloat = true, InitPaneGroupGUID = DockingPaneGroup_BottomRight, GUID = DockingPane_PropertyGrid)]
    class PropertyGridPane : DockingPaneBase {
        [ImportingConstructor]
        public PropertyGridPane(Views.PropertyGrid propertyGrid) {
            Header = LanguageService.FindResourceString(PaneHeader_PropertyGrid);
            this._propertyGrid = propertyGrid;
        }

        private readonly Views.PropertyGrid _propertyGrid;
        public override object UIObject => _propertyGrid;
    }
}
