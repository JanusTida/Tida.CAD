using CDO.Common.Canvas.Contracts;
using CDO.Common.Canvas.Shell.Contracts.Canvas;
using CDO.Common.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDO.Common.Canvas.Shell.InteractionHandlers {
    /// <summary>
    /// 命令输入交互处理器;
    /// </summary>
    //[ExportInteractionHandler(Order = 2)]

    public class CommandInputHandler : CanvasInteractionHandlerBase {
        public override void HandlePosition(ICanvasControl canvasContext, Vector2D oriPosition) {
            
        }
    }
}
