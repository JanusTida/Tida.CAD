using Tida.Canvas.Infrastructure.MoveTools;
using System.Collections.Generic;

namespace Tida.Canvas.Shell.Contracts.Canvas {
    /// <summary>
    /// 移动工具提供器;
    /// </summary>
    public interface IMoveToolProvider {
        /// <summary>
        /// 所提供的移动工具;
        /// </summary>
        IEnumerable<IDrawObjectMoveTool> MoveTools { get; }
    }
}
