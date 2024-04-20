using Tida.Canvas.Infrastructure.TrimTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.TrimTools {
    /// <summary>
    /// 绘制对象裁剪工具提供器;
    /// </summary>
    public interface ITrimToolsProvider {
        /// <summary>
        /// 所提供的裁剪的工具;
        /// </summary>
        IEnumerable<IDrawObjectTrimTool> Tools { get; }
    }
}
