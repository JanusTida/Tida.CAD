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
    public abstract partial class DrawObject : CanvasElement,ICloneable<DrawObject> {
        
        /// <summary>
        /// Indicates whether the point in inside the object;
        /// </summary>
        /// <param name="point">坐标(工程数学坐标)</param>
        /// <param name="canvasScreenConverter">视图单位转化器,可用于内部进行误差判断</param>
        /// <returns></returns>
        public virtual bool PointInObject(Point point, ICanvasScreenConverter canvasScreenConverter) => false;

        /// <summary>
        /// Indicated whether the object in inside a rectangle;
        /// </summary>
        /// <param name="rect">区域(工程数学坐标为准)</param>
        /// <param name="anyPoint">是否模糊匹配,即判定相交是否满足条件</param>
        /// <param name="canvasScreenConverter">视图单位转化器,可用于内部进行误差判断</param>
        /// <returns></returns>
        public virtual bool ObjectInRectangle(CADRect rect, ICanvasScreenConverter canvasScreenConverter, bool anyPoint) => false;


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
        /// 是否正在被编辑修改;
        /// </summary>
        public virtual bool IsEditing { get; }

        /// <summary>
        /// 是否正在被编辑发生了变化;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<bool>> IsEditingChanged;

        /// <summary>
        /// 触发是否正在被编辑变化事件;
        /// </summary>
        protected void RaiseIsEditingChanged(ValueChangedEventArgs<bool> e) {

            if (e == null) {
                throw new ArgumentNullException(nameof(e));
            }
            
            IsEditingChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 父对象;
        /// </summary>
        public CanvasElement Parent => InternalParent;
        internal CanvasElement InternalParent { get; set; }

        /// <summary>
        /// 本绘制对象的信息发生变更时发生;
        /// </summary>
        public event EventHandler<IEditTransaction> EditTransActionCommited;
        

        /// <summary>
        /// 触发绘制对象信息变更事务,由子类调用;
        /// </summary>
        /// <param name="editTransaction"></param>
        protected void RaiseEditTransActionCommited(IEditTransaction editTransaction) {
            if(editTransaction == null) {
                throw new ArgumentNullException(nameof(editTransaction));
            }

            EditTransActionCommited?.Invoke(this, editTransaction);
        }

        /// <summary>
        /// 复制自身;
        /// </summary>
        /// <returns></returns>
        public virtual DrawObject Clone() => null;
        
        /// <summary>
        /// 设定字段值,
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="canBeNull">值能否为空</param>
        /// <param name="commitTransaction">是否呈递标准事务;</param>
        /// <param name="getField">获取字段值</param>
        /// <param name="setField">设定字段值</param>
        /// <param name="raiseVisualChanged">是否触发视觉变化事件</param>
        protected void SetProperty<T>(Action<T> setField, Func<T> getField, T value, bool commitTransaction = true,bool raiseVisualChanged = true) {
            SetProperty(setField, getField, value, new SetPropertySettings {
                CommitTransaction= commitTransaction,
                RaiseVisualChanged = raiseVisualChanged
            });
        }

        protected void SetProperty<T>(Action<T> setField, Func<T> getField,T value, SetPropertySettings settings) {
            var oldValue = getField();
            if (Equals(oldValue,value)) {
                return;
            }

            setField(value);

            if (settings.RaiseVisualChanged) {
                RaiseVisualChanged();
            }

            void Undo() {
                setField(oldValue);
                if (settings.RaiseVisualChanged) {
                    RaiseVisualChanged();
                }
            }

            void Redo() {
                setField(value);
                if (settings.RaiseVisualChanged) {
                    RaiseVisualChanged();
                }
            }

            if (settings.CommitTransaction) {
                //呈递事务;
                RaiseEditTransActionCommited(new StandardEditTransaction(Undo, Redo));
            }
        }
        
        /// <summary>
        /// 设定字段值所使用的设定;
        /// </summary>
        public struct SetPropertySettings {
            /// <summary>
            /// 是否呈递事务;
            /// </summary>
            public bool CommitTransaction { get; set; }

            /// <summary>
            /// 是否触发视觉变化事件;
            /// </summary>
            public bool RaiseVisualChanged { get; set; }
        }
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
        public void OnPreviewMouseDown(CADMouseButtonEventArgs e) {
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
