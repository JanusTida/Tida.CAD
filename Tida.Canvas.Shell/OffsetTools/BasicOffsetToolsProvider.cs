using Tida.Canvas.Infrastructure.OffsetTools;
using Tida.Canvas.Shell.Contracts.OffsetTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.OffsetTools {
    /// <summary>
    /// 基本偏移工具提供器;
    /// </summary>
    [Export(typeof(IOffsetToolsProvider))]
    class BasicOffsetToolsProvider : IOffsetToolsProvider {
        private IDrawObjectOffsetTool[] _tools;
        public IEnumerable<IDrawObjectOffsetTool> Tools =>
            _tools ?? (_tools = new IDrawObjectOffsetTool[]{
                new LineOffsetTool()
            });
    }
}
