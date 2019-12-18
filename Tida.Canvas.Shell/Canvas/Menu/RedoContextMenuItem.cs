
using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Prism.Commands;
using System.Windows.Input;
using static Tida.Canvas.Shell.Canvas.Constants;
using static Tida.Canvas.Shell.Contracts.Canvas.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.Menu {
    [ExportMenuItem(GUID = MenuItem_CanvasContextMenu_Redo,HeaderLanguageKey = MenuItemName_Redo,OwnerGUID = Menu_CanvasContextMenu,Order = MenuItemOrder_CanvasContextMenu_Redo)]
    class RedoContextMenuItem : IMenuItem {
        public RedoContextMenuItem() {
            CommonEventHelper.GetEvent<CanvasCanRedoChangedEvent>().Subscribe(CanvasCanRedoChanged);
        }

        private void CanvasCanRedoChanged(CanvasCanRedoChangedEventArgs e) {
            _redoCommand.RaiseCanExecuteChanged();
        }

        private readonly DelegateCommand _redoCommand = new DelegateCommand(() => CanvasService.CanvasDataContext.Redo(), () => CanvasService.CanvasDataContext.CanRedo);

        public ICommand Command => _redoCommand;

        public string HeaderLanguageKey => MenuItemName_Redo;

        public string Icon => MenuItemIcon_Redo;
    }
}
