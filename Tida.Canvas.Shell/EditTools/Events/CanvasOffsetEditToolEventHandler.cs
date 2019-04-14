using Tida.Canvas.Base.EditTools;
using Tida.Application.Contracts.App;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.CommandOutput;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.EditTools.Events {
    [Export(typeof(ICanvasEditToolChangedEventHandler))]
    class CanvasOffsetEditToolEventHandler : CanvasEditToolChangedEventHandlerGenericBase<OffsetEditTool> {
        /// <summary>
        /// 当当前工具变更为复制工具后,进行对应的UI响应(输出响应等);
        /// </summary>
        protected override void HandleNewEditTool(CanvasEditToolChangedEventArgs args, OffsetEditTool newEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_BeginText_Offset));
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_InputOffset));
            newEditTool.FixedOffsetChanged += EditTool_FixedOffsetChanged;
            newEditTool.DrawObjectSelected += EditTool_DrawObjectSelected;
            newEditTool.OffsetApplied += EditTool_OffsetApplied;
        }

        protected override void HandleOldEditTool(CanvasEditToolChangedEventArgs args, OffsetEditTool oldEditTool) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_EndText_Offset));
            oldEditTool.FixedOffsetChanged -= EditTool_FixedOffsetChanged;
            oldEditTool.DrawObjectSelected -= EditTool_DrawObjectSelected;
            oldEditTool.OffsetApplied -= EditTool_OffsetApplied;
        }


        private static void EditTool_FixedOffsetChanged(object sender, EventArgs e) {
            if (!(sender is OffsetEditTool offsetEditTool)) {
                return;
            }

            if(offsetEditTool.FixedOffset == null) {
                return;
            }

            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_SelectDrawObject));
        }

        private static void EditTool_DrawObjectSelected(object sender, EventArgs e) {
            if (!(sender is OffsetEditTool offsetEditTool)) {
                return;
            }
            
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_DrawObjectSelected));
        }

        private static void EditTool_OffsetApplied(object sender, EventArgs e) {
            CommandOutputService.WriteLine(LanguageService.FindResourceString(Constants.EditToolTip_InputOffset));
        }


        
    }
}
