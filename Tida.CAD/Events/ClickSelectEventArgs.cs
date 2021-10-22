using System.ComponentModel;
using System.Windows;

namespace Tida.CAD.Events
{
    /// <summary>
    /// 点击选取事件参数;
    /// </summary>
    public class ClickSelectEventArgs : CancelEventArgs {
        public ClickSelectEventArgs(Point position, DrawObject[] hitedDrawObjects) {
            this.HitPosition = position;
            this.HitedDrawObjects = hitedDrawObjects;
        }

        /// <summary>
        /// 鼠标命中位置;
        /// </summary>
        public Point HitPosition { get; }
        
        /// <summary>
        /// 被命中的绘制单元;
        /// </summary>
        public DrawObject[] HitedDrawObjects { get; }
        
    }
}
