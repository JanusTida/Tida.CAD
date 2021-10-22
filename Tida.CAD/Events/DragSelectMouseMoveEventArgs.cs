using System.Windows;

namespace Tida.CAD.Events
{
    /// <summary>
    /// 拖放选取鼠标移动时,预处理参数;
    /// </summary>
    public class DragSelectMouseMoveEventArgs :
#if WPF
        RoutedEventArgs
#endif
    {
        public DragSelectMouseMoveEventArgs(CADRect rect,Point position) {
            Rect = rect;
            Position = position;
        }

        /// <summary>
        /// 拖拽关注的区域的矩形;
        /// </summary>
        public CADRect Rect { get; }
        
        /// <summary>
        /// 是否为任意选取;
        /// 若值设定了为空,主控件将根据鼠标的操作逻辑进行是否任意选取;
        /// </summary>
        public bool? IsAnyPoint { get; set; }

        public Point Position { get; }
    }
}
