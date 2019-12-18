using Tida.Canvas.Shell.Contracts.Controls;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.StatusBar.Constants;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 向状态栏图层信息项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class CanvasLayersStatuBarItem : StatusBarItemBase {
        public CanvasLayersStatuBarItem():base(StatusBarItem_CanvasLayers) {
            Order = StatusBarOrder_CanvasLayers;
        }

        
        public override object UIObject => ViewProvider.GetView(CanvasLayersStatusBarItem);
    }
}
