using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Infrastructure.EditTools;

namespace Tida.Canvas.Shell.EditTools.Events {
    /// <summary>
    /// 偏移工具对应的UI响应器;
    /// </summary>
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CanvasOffsetEditToolEventHandler : CanvasEditToolChangedEventHandlerGenericBase<OffsetEditTool2> {
        /// <summary>
        /// 当当前工具变更为复制工具后,进行对应的UI响应(输出响应等);
        /// </summary>
        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, OffsetEditTool2 newEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_BeginText_Offset));
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_InputOffset));
            newEditTool.FixedOffsetChanged += EditTool_FixedOffsetChanged;
            newEditTool.DrawObjectSelected += EditTool_DrawObjectSelected;
            newEditTool.OffsetApplied += EditTool_OffsetApplied;
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, OffsetEditTool2 oldEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_EndText_Offset));
            oldEditTool.FixedOffsetChanged -= EditTool_FixedOffsetChanged;
            oldEditTool.DrawObjectSelected -= EditTool_DrawObjectSelected;
            oldEditTool.OffsetApplied -= EditTool_OffsetApplied;
        }


        private static void EditTool_FixedOffsetChanged(object sender, EventArgs e) {
            if (!(sender is OffsetEditTool2 offsetEditTool)) {
                return;
            }

            if(offsetEditTool.FixedOffset == null) {
                return;
            }

            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_SelectDrawObject));
        }

        private static void EditTool_DrawObjectSelected(object sender, EventArgs e) {
            if (!(sender is OffsetEditTool2 offsetEditTool)) {
                return;
            }
            
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_DrawObjectSelected));
        }

        private static void EditTool_OffsetApplied(object sender, EventArgs e) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_InputOffset));
        }


        
    }
}
