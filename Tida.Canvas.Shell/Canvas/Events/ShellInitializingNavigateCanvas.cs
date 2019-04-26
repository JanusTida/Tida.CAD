using Tida.Application.Contracts.Docking;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.MainPage;
using Tida.Canvas.Shell.Contracts.Shell;
using Tida.Canvas.Shell.Contracts.Shell.Events;
using Prism.Commands;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Tida.Canvas.Infrastructure.InteractionHandlers;

namespace Tida.Canvas.Shell.Canvas.Events {
    /// <summary>
    /// 导航画布区域到Shell;
    /// </summary>
    [Export(typeof(IShellInitializingEventHandler))]
    class ShellInitializingNavigateCanvas : IShellInitializingEventHandler {
        [ImportingConstructor]
        public ShellInitializingNavigateCanvas(CanvasDocumentPane canvasDocumentPane) {
            _canvasDocumentPane = canvasDocumentPane;
        }

        private readonly CanvasDocumentPane _canvasDocumentPane;

        public int Sort => 4;

        public bool IsEnabled => true;

        public void Handle() {
            MainDockingService.Current.AddPaneToDocument(new CreatedDockingPane(_canvasDocumentPane,_canvasDocumentPane));
            CanvasService.Current.Initialize();

            AddKeyBindings();
        }

        /// <summary>
        /// 添加快捷键绑定;
        /// </summary>
        private void AddKeyBindings() {
            //添加撤销/重做热键绑定;
            ShellService.Current.AddKeyBinding(_undoCommand, Key.Z, ModifierKeys.Control);
            ShellService.Current.AddKeyBinding(_redoCommand, Key.Y, ModifierKeys.Control);
            //正交模式使用F8设定;
            ShellService.Current.AddKeyBinding(_switchVertextModeCommand, Key.F8);
            ShellService.Current.AddKeyBinding(_removeSelectedDrawObjectsCommand, Key.Delete);
            //若按下了Esc则退出编辑状态;
            ShellService.Current.AddKeyBinding(_exitEditStateCommand, Key.Escape);
            ShellService.Current.AddKeyBinding(_selectAllDrawObjectsCommand, Key.A, ModifierKeys.Control);
        }

        private readonly DelegateCommand _undoCommand  = new DelegateCommand(CanvasService.CanvasDataContext.Undo);
        private readonly DelegateCommand _redoCommand = new DelegateCommand(CanvasService.CanvasDataContext.Redo);
        private readonly DelegateCommand _switchVertextModeCommand = new DelegateCommand(SwitchVertextMode);
        private readonly DelegateCommand _exitEditStateCommand = new DelegateCommand(ExitEditState);
        private readonly DelegateCommand _removeSelectedDrawObjectsCommand = new DelegateCommand(() => CanvasService.Current.CanvasDataContext.RemoveSelectedDrawObjects());
        private readonly DelegateCommand _selectAllDrawObjectsCommand = new DelegateCommand(SelectAllDrawObjects);

        /// <summary>
        /// 切换正交模式;
        /// </summary>
        private static void SwitchVertextMode() {
            VertextInteractionHandler.IsEnabled = !VertextInteractionHandler.IsEnabled;
        }

        /// <summary>
        /// 退出编辑状态;
        /// </summary>
        private static void ExitEditState() {
            if (CanvasService.CanvasDataContext?.CurrentEditTool != null) {
                CanvasService.CanvasDataContext.CurrentEditTool = null;
            }
            else {
                //若按下的键为Esc,取消所有选中的绘制对象;
                var selectedDrawObjects = CanvasService.CanvasDataContext.
                    GetAllVisibleDrawObjects().Where(p => p.IsSelected);

                foreach (var drawObject in selectedDrawObjects) {
                    drawObject.IsSelected = false;
                }
            }
        }

       
        

        /// <summary>
        /// 选定所有绘制对象命令;
        /// </summary>
        private static void SelectAllDrawObjects() {
            //处于编辑时不能进行全选;
            if(CanvasService.CanvasDataContext.CurrentEditTool != null) {
                return;
            }

            var allDrawObjects = CanvasService.CanvasDataContext?.GetAllVisibleDrawObjects();
            if(allDrawObjects == null) {
                return;
            }

            foreach (var drawObject in allDrawObjects) {
                drawObject.IsSelected = true;
            }
        }
    }
}
