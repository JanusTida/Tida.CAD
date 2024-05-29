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
        /// <param name="point">The posion in cad coordinates</param>
        /// <param name="cadScreenConverter"></param>
        /// <returns></returns>
        public virtual bool PointInObject(Point point, ICADScreenConverter cadScreenConverter) => false;

        /// <summary>
        /// Indicated whether the object in inside a rectangle;
        /// </summary>
        /// <param name="rect">The selection rectangle</param>
        /// <param name="anyPoint">To indicate whether the drawobject should be hit when the rect just intersets with the drawobject that is not inside the rect</param>
        /// <param name="cadScreenConverter"></param>
        /// <returns></returns>
        public virtual bool ObjectInRectangle(CADRect rect, ICADScreenConverter cadScreenConverter, bool anyPoint) => false;


        /// <summary>
        /// Get the bounding rect for the drawobject;
        /// </summary>
        /// <returns></returns>
        public virtual CADRect? GetBoundingRect(ICADScreenConverter screenConverter) => null;
        
        private bool _isSelected;
        /// <summary>
        /// IsSelected;
        /// </summary>
        public bool IsSelected 
        {
            get => _isSelected;
            set
            {
                if(_isSelected == value) 
                {
                    return;
                }
                _isSelected = value;

                var e = new ValueChangedEventArgs<bool>(_isSelected, !_isSelected);
                OnSelectedChanged(e);
                IsSelectedChanged?.Invoke(this, e);

                
                RaiseVisualChanged();
            }
        }
        
        /// <summary>
        /// The protected virtual method invoked while IsSelected changed;
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectedChanged(ValueChangedEventArgs<bool> e) {
            
        }
        
        /// <summary>
        /// The event raised when IsSelected changed;
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<bool>>? IsSelectedChanged;
        
        /// <summary>
        /// The parent layer of the drawobject;
        /// </summary>
        public CADLayer? Layer => InternalLayer;
        internal CADLayer? InternalLayer { get; set; }

    }

    /// <summary>
    /// The interaction of the drawobject (These interactions are availabel only when <see cref="IsSelected"/> is True);
    /// </summary>
    public abstract partial class DrawObject {
        /// <summary>
        /// The method invoked while mouse is moving;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point"></param>
        /// <remarks>This interaction are availabel only when <see cref="IsSelected"/> is True</remarks>
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
        /// The method invoked while mouse is pressed;
        /// </summary>
        /// <param name="canvas"></param>
        /// <remarks>This interaction are availabel only when <see cref="IsSelected"/> is True</remarks>
        public void OnMouseDown(CADMouseButtonEventArgs e) {
            PreviewMouseDown?.Invoke(this, e);
            if (e.Handled) {
                return;
            }

            OnMouseDownCore(e);
        }

        protected virtual void OnMouseDownCore(CADMouseButtonEventArgs e) { }

        /// <summary>
        /// The method invoked while the mouse is released;
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="point"></param>
        /// <param name="snapShape"></param>
        /// <remarks>This interaction are availabel only when <see cref="IsSelected"/> is True</remarks>
        public void OnMouseUp(CADMouseButtonEventArgs e) {
            PreviewMouseUp?.Invoke(this, e);
            if(e.Handled) {
                return;
            }

            OnMouseUpCore(e);
        }


        protected virtual void OnMouseUpCore(CADMouseButtonEventArgs e) { }

        /// <summary>
        /// The method invoked while the mouse is released;
        /// </summary>
        /// <param name="canvas"></param>
        /// <remarks>This interaction are availabel only when <see cref="IsSelected"/> is True</remarks>
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

        public event EventHandler<CADMouseButtonEventArgs>? PreviewMouseDown;
        public event EventHandler<CADMouseEventArgs>? PreviewMouseMove;
        public event EventHandler<CADMouseButtonEventArgs>? PreviewMouseUp;
        public event EventHandler<CADKeyEventArgs>? PreviewKeyDown;
        public event EventHandler<CADKeyEventArgs>? PreviewKeyUp;
        public event EventHandler<TextCompositionEventArgs>? PreviewTextInput;
    }

}
