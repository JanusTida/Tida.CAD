using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.Ribbon.Views {
    /// <summary>
    /// Ribbon.xaml 的交互逻辑
    /// </summary>
    [Export]
    public partial class Ribbon : UserControl {
        public Ribbon() {
            InitializeComponent();
        }

        public Telerik.Windows.Controls.RadRibbonView RibbonMenu => ribbon;

        public Telerik.Windows.Controls.ApplicationMenu AppMenu => appMenu;
    }
}
