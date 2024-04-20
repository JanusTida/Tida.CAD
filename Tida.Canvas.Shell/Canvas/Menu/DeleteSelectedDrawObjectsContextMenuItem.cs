using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Prism.Commands;
using System.Windows.Input;
using static Tida.Canvas.Shell.Canvas.Constants;
using static Tida.Canvas.Shell.Contracts.Canvas.Constants;

namespace Tida.Canvas.Shell.Canvas.Menu {
    /// <summary>
    /// 移除选中的绘制对象菜单项;
    /// </summary>
    [ExportMenuItem(GUID = MenuItem_CanvasContextMenu_DeleteSelectedDrawObjects, OwnerGUID = Menu_CanvasContextMenu,HeaderLanguageKey = MenuItemName_CanvasContextMenu_DeleteSelectedDrawObjects,Order = MenuItemOrder_CanvasContextMenu_DeleteSelectedDrawObjects)]
    class DeleteSelectedDrawObjectsContextMenuItem : IMenuItem {

        public ICommand Command { get; } = new DelegateCommand(() => CanvasService.CanvasDataContext.RemoveSelectedDrawObjects());
    }
}
