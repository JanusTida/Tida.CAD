using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Prism.Commands;
using System.Linq;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Canvas.Menu {
    /// <summary>
    ////隐藏选定的对象;
    /// </summary>
    //[ExportMenuItem(GUID  = MenuItem_CanvasContextMenu_HideSelectedDrawObjects, OwnerGUID = Menu_CanvasContextMenu, HeaderLanguageKey = MenuItemName_CanvasContextMenu_HideSelectedDrawObjects)]
    class HideSelectedDrawObjectsContextMenuItem : IMenuItem {
        private DelegateCommand _hideSelectedCommand;
        public ICommand Command => _hideSelectedCommand ?? (_hideSelectedCommand = new DelegateCommand(
            () => {
                foreach (var item in CanvasService.CanvasDataContext.GetAllDrawObjects().Where(p => p.IsSelected)) {
                    item.IsVisible = false;
                }
            }
        ));
    }
}
