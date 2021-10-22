using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 拖放选取鼠标移动时,预处理参数;
    /// </summary>
    public class DragSelectMouseMoveEventArgs : MouseMoveEventArgs {
        public DragSelectMouseMoveEventArgs(Rect rect,Point position):base(position) {
            Rect = rect;
        }

        /// <summary>
        /// 拖拽关注的区域的矩形;
        /// </summary>
        public Rect Rect { get; }
        
        /// <summary>
        /// 是否为任意选取;
        /// 若值设定了为空,主控件将根据鼠标的操作逻辑进行是否任意选取;
        /// </summary>
        public bool? IsAnyPoint { get; set; }
    }
}
