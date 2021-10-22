using Tida.CAD.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 正在进行辅助判断的事件参数;
    /// </summary>
    public class SnapingEventArgs:CancelEventArgs {
        public SnapingEventArgs(IEnumerable<DrawObject> drawObjects) {
            if (drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }

            this.DrawObjects = drawObjects.ToList();
        }

        /// <summary>
        /// 将要参与辅助判断的绘制对象集合;
        /// </summary>
        public ICollection<DrawObject> DrawObjects { get; }
    }
}
