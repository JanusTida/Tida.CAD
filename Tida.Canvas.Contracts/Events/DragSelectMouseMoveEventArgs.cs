using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 拖放选取鼠标移动时,预处理参数;
    /// </summary>
    public class DragSelectMouseMoveEventArgs : MouseMoveEventArgs {
        public DragSelectMouseMoveEventArgs(Rectangle2D2 rectangle2D,Vector2D position):base(position) {

        }

        /// <summary>
        /// 拖拽关注的区域的矩形;
        /// </summary>
        public Rectangle2D2 Rectangle2D { get; }
        
        /// <summary>
        /// 是否为任意选取;
        /// 若值设定了为空,主控件将根据鼠标的操作逻辑进行是否任意选取;
        /// </summary>
        public bool? IsAnyPoint { get; set; }
    }
}
