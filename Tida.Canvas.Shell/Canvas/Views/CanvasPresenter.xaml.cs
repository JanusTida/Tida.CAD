using System.ComponentModel.Composition;
using System.Windows.Controls;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Canvas.IViews;

namespace Tida.Canvas.Shell.Canvas.Views {
    /// <summary>
    /// CanvasView.xaml 的交互逻辑
    /// </summary>
    [Export,Export(typeof(ICanvasPresenter))]
    public partial class CanvasPresenter : ContentControl, ICanvasPresenter
    {
        public CanvasPresenter() {
            InitializeComponent();
        }
        
        public ICanvasControl CanvasControl => canvasControl;
        
    }
}
