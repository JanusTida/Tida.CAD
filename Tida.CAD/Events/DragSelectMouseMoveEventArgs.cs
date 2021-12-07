using System.Windows;

namespace Tida.CAD.Events
{
    /// <summary>
    /// The mouse move events args when drag selecting;
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
        /// The rect that contains the draging select area;
        /// </summary>
        public CADRect Rect { get; }
        
        /// <summary>
        /// Whether anypoint select;
        /// </summary>
        public bool? IsAnyPoint { get; set; }

        /// <summary>
        /// The position of mouse;
        /// </summary>
        public Point Position { get; }
    }
}
