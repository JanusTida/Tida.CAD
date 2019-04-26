using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Prism.Commands;
using System.Windows.Input;
using static Tida.Canvas.Shell.Canvas.Constants;
using static Tida.Canvas.Shell.Contracts.Canvas.Constants;

namespace Tida.Canvas.Shell.Canvas.Menu {
    [ExportMenuItem(GUID = MenuItem_CanvasContextMenu_Undo, HeaderLanguageKey = MenuItemName_Undo, OwnerGUID = Menu_CanvasContextMenu,Order = MenuItemOrder_CanvasContextMenu_Undo)]
    class UndoContextMenuItem : IMenuItem {
        public UndoContextMenuItem() {
            CommonEventHelper.GetEvent<CanvasCanUndoChangedEvent>().Subscribe(CanvasCanUndoChanged);
        }

        private void CanvasCanUndoChanged(CanvasCanUndoChangedEventArgs e) {
            _undoCommand.RaiseCanExecuteChanged();
        }

        private readonly DelegateCommand _undoCommand = new DelegateCommand(() => CanvasService.CanvasDataContext.Undo(), () => CanvasService.CanvasDataContext.CanUndo);
        public ICommand Command => _undoCommand;
        public string HeaderLanguageKey => Constants.MenuItemName_Undo;

        public string Icon => Constants.MenuItemIcon_Undo;
    }
}
