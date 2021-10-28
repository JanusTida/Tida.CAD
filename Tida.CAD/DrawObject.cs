using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tida.CAD.Events;
using Tida.CAD.Input;

namespace Tida.CAD {
    /// <summary>
    /// Drawobject in layer;
    /// </summary>
    public abstract partial class DrawObject : CADElement {
        
        /// <summary>
        /// Indicates whether the point in inside the object;
        /// </summary>
        /// <param name="point">坐标(工程数学坐标)</param>
        /// <param name="cadScreenConverter">视图单位转化器,可用于内部进行误差判断</param>
        /// <returns></returns>
        public virtual bool PointInObject(Point point, ICADScreenConverter cadScreenConverter) => false;

        /// <summary>
        /// Indicated whether the object in inside a rectangle;
        /// </summary>
        /// <param name="rect">区域(工程数学坐标为准)</param>
        /// <param name="anyPoint">是否模糊匹配,即判定相交是否满足条件</param>
        /// <param name="cadScreenConverter">视图单位转化器,可用于内部进行误差判断</param>
        /// <returns></returns>
        public virtual bool ObjectInRectangle(CADRect rect, ICADScreenConverter cadScreenConverter, bool anyPoint) => false;


        /// <summary>
        /// 获取绘制对象有效的区域范围,该区域以工程坐标为准;
        /// </summary>
        /// <returns></returns>
        public virtual CADRect? GetBoundingRect() => null;
        
        private bool _isSelected;
        /// <summary>
        /// 是否被选中;
        /// </summary>
        public bool IsSelected {
            get {
                return _isSelected;
            }
            set{
                if(_isSelected == value) {
                    return;
                }
                _isSelected = value;

                var e = new ValueChangedEventArgs<bool>(_isSelected, !_isSelected);
                OnSelectedChanged(e);
                IsSelectedChanged?.Invoke(this, e);

                //通知图像发生了变化;
                RaiseVisualChanged();
            }
        }
        
        /// <summary>
        /// 选定状态发生变化的可重载方法;
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectedChanged(ValueChangedEventArgs<bool> e) {
            
        }
        
        /// <summary>
        /// 选中状态发生变化后事件;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<bool>> IsSelectedChanged;
        
        /// <summary>
        /// 父对象;
        /// </summary>
        public CADElement Parent => InternalParent;
        internal CADElement InternalParent { get; set; }

    }

    /// <summary>
    /// 交互响应(当且仅当<see cref="IsSelected"/>为True时，以下交互动作才可能被调用);
    /// </summary>
    public abstract partial class DrawObject {
        /// <summary>
        /// 鼠标移动响应;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point"></param>
        public void OnMouseMove(CADMouseEventArgs e) 
        {
            PreviewMouseMove?.Invoke(this, e);
            if (e.Handled) {
                return;
            }

            OnMouseMoveCore(e);
        }

        protected virtual void OnMouseMoveCore(CADMouseEventArgs e) { }

        /// <summary>
        /// 鼠标按下响应;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point"></param>
        /// <param name="snapShape"></param>
        public void OnMouseDown(CADMouseButtonEventArgs e) {
            PreviewMouseDown?.Invoke(this, e);
            if (e.Handled) {
                return;
            }

            OnMouseDownCore(e);
        }

        protected virtual void OnMouseDownCore(CADMouseButtonEventArgs e) { }

        /// <summary>
        /// 鼠标弹起响应;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point"></param>
        /// <param name="snapShape"></param>
        public void OnMouseUp(CADMouseButtonEventArgs e) {
            PreviewMouseUp?.Invoke(this, e);
            if(e.Handled) {
                return;
            }

            OnMouseUpCore(e);
        }


        protected virtual void OnMouseUpCore(CADMouseButtonEventArgs e) { }

        /// <summary>
        /// 键盘按键响应;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="e"></param>
        public void OnKeyDown(CADKeyEventArgs e) {
            PreviewKeyDown?.Invoke(this, e);
            if (e.Handled) {
                return;
            }

            OnKeyDownCore(e);
        }

        protected virtual void OnKeyDownCore(CADKeyEventArgs e) { }

        public void OnKeyUp(CADKeyEventArgs e) {
            PreviewKeyUp?.Invoke(this, e);

            if (e.Handled) {
                return;
            }

            OnKeyUpCore(e);
        }

        protected virtual void OnKeyUpCore(CADKeyEventArgs e) {

        }

        public void OnTextInput(TextCompositionEventArgs e) {
            PreviewTextInput?.Invoke(this, e);
            if (e.Handled) {
                return;
            }
            
            OnTextInputCore(e);
        }

        protected virtual void OnTextInputCore(TextCompositionEventArgs e) {
            
        }

        public event EventHandler<CADMouseButtonEventArgs> PreviewMouseDown;
        public event EventHandler<CADMouseEventArgs> PreviewMouseMove;
        public event EventHandler<CADMouseButtonEventArgs> PreviewMouseUp;
        public event EventHandler<CADKeyEventArgs> PreviewKeyDown;
        public event EventHandler<CADKeyEventArgs> PreviewKeyUp;
        public event EventHandler<TextCompositionEventArgs> PreviewTextInput;
    }

}
