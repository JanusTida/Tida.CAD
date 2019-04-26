using Tida.Application.Contracts.App;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using System.ComponentModel.Composition;
using static Tida.Canvas.Shell.Canvas.Constants;
using Tida.Canvas.Input;

namespace Tida.Canvas.Shell.Canvas.Events {
    /// <summary>
    /// 处理当前图层不可见时,编辑工具的交互预处理;
    /// </summary>
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CurrentEditToolChangedHandlerWarningUnVisibleLayer : ICanvasEditToolChangedEventHandler {
        public int Sort => 0;

        public bool IsEnabled => true;

        public void Handle(CanvasEditToolChangedEventArgs args) {
            var newEditTool = args.EventArgs.NewValue;
            var oldEditTool = args.EventArgs.OldValue;
            
            if(newEditTool != null) {
                newEditTool.CanvasPreviewMouseDown += EditTool_CanvasPreviewMouseDown;
            }
            if(oldEditTool != null) {
                oldEditTool.CanvasPreviewMouseDown -= EditTool_CanvasPreviewMouseDown;
            }
        }

        private void EditTool_CanvasPreviewMouseDown(object sender, MouseDownEventArgs e) {
            if (!(sender is EditTool editTool)) {
                return;
            }

            if(editTool.CanvasContext.ActiveLayer == null) {
                return;
            }

            if (!editTool.CanvasContext.ActiveLayer.IsVisible) {
                MsgBoxService.Show(LanguageService.FindResourceString(Exception_UnVisibleLayerNotInteractable));
                e.Handled = true;
            }
        }
    }
}
