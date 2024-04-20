﻿
using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Prism.Commands;
using System.Windows.Input;
using static Tida.Canvas.Shell.Canvas.Constants;
using static Tida.Canvas.Shell.Contracts.Canvas.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.Menu {
    [ExportMenuItem(GUID = MenuItem_CanvasContextMenu_CommitEdit, OwnerGUID = Menu_CanvasContextMenu, HeaderLanguageKey = MenuItemName_CanvasContextMenu_CommitEdit, Order = MenuItemOrder_CanvasContextMenu_CommitEdit)]
    class CommitEditContextMenuItem : IMenuItem {
        public CommitEditContextMenuItem() {
            CommonEventHelper.GetEvent<CanvasEditToolChangedEvent>().Subscribe(CanvasEditToolChanged);
        }

        private void CanvasEditToolChanged(CanvasEditToolChangedEventArgs e) {
            _commitEditCommand.RaiseCanExecuteChanged();
        }

        private readonly DelegateCommand _commitEditCommand = new DelegateCommand(() => CanvasService.CanvasDataContext.CommitEdit(),() => CanvasService.CanvasDataContext.CurrentEditTool != null);
        public ICommand Command => _commitEditCommand;
    }
}
