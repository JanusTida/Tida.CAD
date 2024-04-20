
using Tida.Canvas.Shell.Canvas.ViewModels;
using Tida.Canvas.Shell.Contracts.Ribbon;
using System.Windows.Input;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.Ribbon {
    /// <summary>
    /// Ribbon按钮项——选择模式;
    /// </summary>
    [ExportRibbonItem(GroupGUID = Constants.RibbonGroup_Edit, GUID = Constants.RibbonItem_Select, Order = 1)]
    class SelectRibbonButtonItem : IRibbonButtonItem {
        public ICommand Command => ServiceProvider.GetInstance<CanvasPresenterViewModel>().SelectCommand;

        public string HeaderLanguageKey => Constants.MenuItemName_Select;

        public string Icon => Constants.MenuItemIcon_Select;
    }
}
