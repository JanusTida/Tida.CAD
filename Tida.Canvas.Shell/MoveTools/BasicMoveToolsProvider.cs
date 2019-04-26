using Tida.Canvas.Infrastructure.MoveTools;
using Tida.Canvas.Shell.Contracts.MoveTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.MoveTools {
    /// <summary>
    /// 基本移动工具提供器;
    /// </summary>
    [Export(typeof(IMoveToolsProvider))]
    class BasicMoveToolsProvider : IMoveToolsProvider {
        private IDrawObjectMoveTool[] _tools;
        public IEnumerable<IDrawObjectMoveTool> Tools =>
            _tools ?? (_tools = new IDrawObjectMoveTool[] {
                new LineMoveTool(),
                new EllipseMoveTool(),
                new PointMoveTool(),
                new RectangleMoveTool()
            });
    }
}
