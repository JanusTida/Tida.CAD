using Tida.Canvas.Infrastructure.MoveTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.MoveTools {
    /// <summary>
    /// 绘制对象移动工具提供器;
    /// </summary>
    public interface IMoveToolsProvider {
        /// <summary>
        /// 所提供的移动工具集合;
        /// </summary>
        IEnumerable<IDrawObjectMoveTool> Tools { get; }
    }
}
