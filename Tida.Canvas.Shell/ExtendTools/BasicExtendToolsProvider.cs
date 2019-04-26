using Tida.Canvas.Infrastructure.ExtendTools;
using Tida.Canvas.Shell.Contracts.ExtendTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.ExtendTools {
    [Export(typeof(IExtendToolsProvider))]
    class BasicExtendToolsProvider : IExtendToolsProvider {
        private IDrawObjectExtendTool[] _tools;
        public IEnumerable<IDrawObjectExtendTool> Tools =>
            _tools ?? (_tools = new IDrawObjectExtendTool[] {
                new LineExtendTool()
            });
    }
}
