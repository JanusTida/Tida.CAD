using Tida.Canvas.Infrastructure.MirrorTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.MirrorTools {
    /// <summary>
    /// 绘制对象-镜像工具提供器;
    /// </summary>
    public interface IMirrorToolProvider {
        /// <summary>
        /// 所提供的镜像工具集合;
        /// </summary>
        IEnumerable<IDrawObjectMirrorTool> Tools { get; }
    }
}
