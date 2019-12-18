
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Prism.Commands;
using System.Windows.Input;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.Ribbon {
    /// <summary>
    /// Ribbon按钮项——撤销;
    /// </summary>
    [ExportRibbonItem(GroupGUID = Constants.RibbonGroup_Edit, GUID = Constants.RibbonItem_Redo, Order = 4)]
    class RedoRibbonButtonItem : IRibbonButtonItem {
        public RedoRibbonButtonItem() {
            CommonEventHelper.GetEvent<CanvasCanRedoChangedEvent>().Subscribe(CanvasCanRedoChanged);
        }

      
        private void CanvasCanRedoChanged(CanvasCanRedoChangedEventArgs e) {
            _redoCommand.RaiseCanExecuteChanged();
        }

        private readonly DelegateCommand _redoCommand = new DelegateCommand(CanvasService.CanvasDataContext.Redo, () => CanvasService.CanvasDataContext.CanRedo);
        public ICommand Command => _redoCommand;

        public string HeaderLanguageKey => Constants.MenuItemName_Redo;

        public string Icon => Constants.MenuItemIcon_Redo;

    }
}
