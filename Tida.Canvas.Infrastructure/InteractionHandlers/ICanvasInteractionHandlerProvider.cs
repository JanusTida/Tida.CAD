using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.InteractionHandlers {
    /// <summary>
    /// 画布交互处理器的提供器契约;
    /// </summary>
    public interface ICanvasInteractionHandlerProvider {
        /// <summary>
        /// 创建一个新的Handler;
        /// </summary>
        /// <returns></returns>
        CanvasInteractionHandler CreateHandler();
    }
}
