using Tida.Canvas.Shell.Contracts.ComponentModel.Views;
using Prism.Mef.Modularity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Tida.Canvas.Shell.ComponentModel {
    [ModuleExport(typeof(ComponentModelModule))]
    public class ComponentModelModule : IModule {
        public void Initialize() {
            StandardValuesEditorManager.SelectorFactory = () => new RadComboBox();
        }
    }
}
