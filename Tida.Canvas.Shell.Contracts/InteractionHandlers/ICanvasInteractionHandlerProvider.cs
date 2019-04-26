
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.Contracts.InteractionHandlers {
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
