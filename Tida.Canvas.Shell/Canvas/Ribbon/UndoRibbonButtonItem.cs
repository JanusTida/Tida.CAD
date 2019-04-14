using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Canvas.ViewModels;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Prism.Commands;
using System;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Canvas.Ribbon {
    /// <summary>
    /// Ribbon按钮项——撤销;
    /// </summary>
    [ExportRibbonItem(GroupGUID = Constants.RibbonGroup_Edit, GUID = Constants.RibbonItem_Undo, Order = 2)]
    class UndoRibbonButtonItem : IRibbonButtonItem {
        public UndoRibbonButtonItem() {
            CommonEventHelper.GetEvent<CanvasCanUndoChangedEvent>().Subscribe(CanvasCanUndoChanged);
        }

        private void CanvasCanUndoChanged(CanvasCanUndoChangedEventArgs e) {
            _undoCommand.RaiseCanExecuteChanged();
        }

        private readonly DelegateCommand _undoCommand = new DelegateCommand(CanvasService.CanvasDataContext.Undo,() => CanvasService.CanvasDataContext.CanUndo);
        public ICommand Command => _undoCommand;
        public string HeaderLanguageKey => Constants.MenuItemName_Undo;

        public string Icon => Constants.MenuItemIcon_Undo;
    }
}
