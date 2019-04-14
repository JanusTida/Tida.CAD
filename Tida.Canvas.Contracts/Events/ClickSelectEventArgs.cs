using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 点击选取事件参数;
    /// </summary>
    public class ClickSelectEventArgs : CancelEventArgs {
        public ClickSelectEventArgs(Vector2D position, DrawObject[] hitedDrawObjects) {
            this.HitPosition = position;
            this.HitedDrawObjects = hitedDrawObjects;
        }

        /// <summary>
        /// 鼠标命中位置;
        /// </summary>
        public Vector2D HitPosition { get; }
        
        /// <summary>
        /// 被命中的绘制单元;
        /// </summary>
        public DrawObject[] HitedDrawObjects { get; }
        
    }
}
