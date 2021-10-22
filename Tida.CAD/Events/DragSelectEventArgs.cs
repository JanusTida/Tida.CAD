using Tida.CAD.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tida.CAD;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 拖放选取事件参数;
    /// </summary>
    public class DragSelectEventArgs:CancelEventArgs {
        public DragSelectEventArgs(Point position,Rect rectangle2D,DrawObject[] hitedDrawObjects) {
            Position = position;
            Rect = rectangle2D;
            HitedDrawObjects = hitedDrawObjects ?? throw new ArgumentNullException(nameof(hitedDrawObjects));
        }
        
        /// <summary>
        /// 鼠标的位置;
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// 拖拽区域形成的矩形;
        /// </summary>
        public Rect Rect { get; }

        /// <summary>
        /// 被命中的绘制单元;
        /// </summary>
        public DrawObject[] HitedDrawObjects { get; }
        
    }

}
