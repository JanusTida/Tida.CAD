using System.ComponentModel.Composition;
using System.Windows.Controls;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.Canvas.Views {
    /// <summary>
    /// CanvasView.xaml 的交互逻辑
    /// </summary>
    [Export]
    public partial class Canvas : ContentControl {
        public Canvas() {
            InitializeComponent();
        }
        

        public ICanvasControl CanvasControl => canvasControl;
        
    }
}
