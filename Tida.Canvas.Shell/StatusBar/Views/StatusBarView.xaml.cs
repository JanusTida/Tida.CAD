using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.StatusBar.Views {
    /// <summary>
    /// StatusBar.xaml 的交互逻辑
    /// </summary>
    [Export]
    public partial class StatusBarView : ContentControl {
        public StatusBarView() {
            InitializeComponent();
            
        }


        public Grid Grid => _grid;
    }
}
