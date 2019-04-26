using Tida.Canvas.Infrastructure.OffsetTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.OffsetTools {
    /// <summary>
    /// 绘制对象偏移工具提供器;
    /// </summary>
    public interface IOffsetToolsProvider {
        /// <summary>
        /// 所提供的偏移工具集合;
        /// </summary>
        IEnumerable<IDrawObjectOffsetTool> Tools { get; }
    }
}
