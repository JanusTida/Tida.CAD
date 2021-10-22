using Tida.CAD.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 绘制对象已被添加事件参数;
    /// </summary>
    public class DrawObjectsAddedEventArgs:EventArgs {
        public DrawObjectsAddedEventArgs(IEnumerable<DrawObject> drawObjects) {

            DrawObjects = drawObjects ?? throw new ArgumentNullException(nameof(drawObjects));

        }

        /// <summary>
        /// 对应的绘制对象;
        /// </summary>
        public IEnumerable<DrawObject> DrawObjects { get; }
    }
}
