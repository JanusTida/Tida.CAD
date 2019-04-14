using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;
using System.Windows;

namespace Tida.Canvas.Shell.StatusBar {
    [Export(typeof(IStatusBarItem))]
    class DefaultStatusBarTextItem: StatusBarTextItem {
        public DefaultStatusBarTextItem():base(Contracts.StatusBar.Constants.StatusBarItem_Default) {
            Margin = new Thickness(3, 0, 3, 0);
            Order = Contracts.StatusBar.Constants.StatusBarOrder_Default;
        }

       
    }
}
