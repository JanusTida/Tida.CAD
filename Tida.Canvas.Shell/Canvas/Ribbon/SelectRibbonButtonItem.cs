using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Canvas.ViewModels;
using Tida.Canvas.Shell.Contracts.Ribbon;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Canvas.Ribbon {
    /// <summary>
    /// Ribbon按钮项——选择模式;
    /// </summary>
    [ExportRibbonItem(GroupGUID = Constants.RibbonGroup_Edit, GUID = Constants.RibbonItem_Select, Order = 1)]
    class SelectRibbonButtonItem : IRibbonButtonItem {
        public ICommand Command => ServiceProvider.GetInstance<CanvasViewModel>().SelectCommand;

        public string HeaderLanguageKey => Constants.MenuItemName_Select;

        public string Icon => Constants.MenuItemIcon_Select;
    }
}
