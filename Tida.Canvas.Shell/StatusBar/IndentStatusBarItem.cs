using Tida.Canvas.Shell.Contracts.Controls;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.StatusBar {
    /// <summary>
    /// 中间用于中空的状态栏项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class IndentStatusBarItem : StatusBarItemBase {
        public IndentStatusBarItem():base(Contracts.StatusBar.Constants.StatusBarItem_Indent) {
            this.Order = Contracts.StatusBar.Constants.StatusBarOrder_Indent;
        }
        
        public override GridChildLength GridChildLength => GridChildLength.Star;

        public override object UIObject => null;
    }
}
