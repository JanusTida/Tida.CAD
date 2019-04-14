using Tida.Application.Contracts.Controls;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 向状态栏图层信息项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class CanvasLayersStatuBarItem : StatusBarItemBase {
        public CanvasLayersStatuBarItem():base(Constants.StatusBarItem_CanvasLayers) {
            Order = Constants.StatusBarOrder_CanvasLayers;
        }

        
        public override object UIObject => ViewProvider.GetView(Constants.CanvasLayersStatusBarItem);
    }
}
