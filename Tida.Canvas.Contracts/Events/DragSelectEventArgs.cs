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
    /// 拖放选取事件参数;
    /// </summary>
    public class DragSelectEventArgs:CancelEventArgs {
        public DragSelectEventArgs(Vector2D position,Rectangle2D2 rectangle2D,DrawObject[] hitedDrawObjects) {
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Rectangle2D = rectangle2D ?? throw new ArgumentNullException(nameof(rectangle2D));
            HitedDrawObjects = hitedDrawObjects ?? throw new ArgumentNullException(nameof(hitedDrawObjects));
        }
        
        /// <summary>
        /// 鼠标的位置;
        /// </summary>
        public Vector2D Position { get; }

        /// <summary>
        /// 拖拽区域形成的矩形;
        /// </summary>
        public Rectangle2D2 Rectangle2D { get; }

        /// <summary>
        /// 被命中的绘制单元;
        /// </summary>
        public DrawObject[] HitedDrawObjects { get; }
        
    }

}
