using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.EditTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Dialogs {
    /// <summary>
    /// 使用对话框以实现绘制对象选择器;
    /// </summary>
    [Export(typeof(IDrawObjectSelector))]
    class DrawObjectSelectWrapper : IDrawObjectSelector {
        public DrawObject SelectOneDrawObject(IEnumerable<DrawObject> drawObjects) {
            return DrawObjectSelectDialog.SelectOneDrawObject(drawObjects);
        }
    }
}
