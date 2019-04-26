using Tida.Canvas.Infrastructure.ExtendTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.ExtendTools {
    /// <summary>
    /// 绘制对象拓展工具提供器;
    /// </summary>
    public interface IExtendToolsProvider {
        /// <summary>
        /// 所有拓展工具集合;
        /// </summary>
        IEnumerable<IDrawObjectExtendTool> Tools { get; }
    }
}
