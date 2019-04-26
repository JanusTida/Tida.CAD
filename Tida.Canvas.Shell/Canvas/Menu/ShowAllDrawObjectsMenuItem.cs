using Tida.Application.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Prism.Commands;
using System.Windows.Input;
using static Tida.Canvas.Shell.Contracts.Canvas.Constants;

namespace Tida.Canvas.Shell.Canvas.Menu {
    /// <summary>
    /// 显示所有的绘制对象菜单项;
    /// </summary>
    [ExportMenuItem(
        GUID = Constants.MenuItemGUID_ShowAllDrawObjects,
        HeaderLanguageKey = Constants.MenuItemName_ShowAllDrawObjects,
        OwnerGUID =Menu_CanvasContextMenu,
        Order = 2048
    )]
    class ShowAllDrawObjectsMenuItem : IMenuItem {
        public ICommand Command => _showAllDrawObjectsCommand ?? (_showAllDrawObjectsCommand = new DelegateCommand(
            () => {
                foreach (var drawObject in CanvasService.CanvasDataContext.GetAllDrawObjects()) {
                    drawObject.IsVisible = true;
                }
            }
        ));
        private DelegateCommand _showAllDrawObjectsCommand;
    }
}
