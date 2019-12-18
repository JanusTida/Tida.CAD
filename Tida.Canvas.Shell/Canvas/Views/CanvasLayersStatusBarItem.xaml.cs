using Tida.Canvas.Shell.Contracts.Controls;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.Canvas.Views {
    /// <summary>
    /// CanvasLayerStatusBarItem.xaml 的交互逻辑
    /// </summary>
    [ExportView(Tida.Canvas.Shell.StatusBar.Constants.CanvasLayersStatusBarItem)]
    public partial class CanvasLayersStatusBarItem : ContentControl {
        public CanvasLayersStatusBarItem() {
            InitializeComponent();
        }
    }
}
