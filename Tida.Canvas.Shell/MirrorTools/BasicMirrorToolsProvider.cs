using Tida.Canvas.Infrastructure.MirrorTools;
using Tida.Canvas.Shell.Contracts.MirrorTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.MirrorTools {
    /// <summary>
    /// 基本的镜像工具提供器;
    /// </summary>
    [Export(typeof(IMirrorToolProvider))]
    class BasicMirrorToolsProvider : IMirrorToolProvider {
        private IDrawObjectMirrorTool[] _tools;
        public IEnumerable<IDrawObjectMirrorTool> Tools =>
            _tools ?? (_tools = new IDrawObjectMirrorTool[] {
                new LineBaseMirrorTool(),
                new EllipseMirrorTool(),
                new PointMirrorTool(),
                new RectangleMirrorTool()
            });
    }
}
