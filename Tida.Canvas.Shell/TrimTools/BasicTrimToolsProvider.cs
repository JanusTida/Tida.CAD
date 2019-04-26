using Tida.Canvas.Infrastructure.TrimTools;
using Tida.Canvas.Shell.Contracts.TrimTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.TrimTools {
    /// <summary>
    /// 基本的裁剪工具提供器;
    /// </summary>
    [Export(typeof(ITrimToolsProvider))]
    class BasicTrimToolsProvider : ITrimToolsProvider {
        private IDrawObjectTrimTool[] _tools;
        public IEnumerable<IDrawObjectTrimTool> Tools =>
            _tools ?? (_tools = new IDrawObjectTrimTool[] {
                new LineTrimTool()
            });
    }
}
