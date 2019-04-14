using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Contracts {
    /// <summary>
    /// 画布元素协约(图层，绘制对象等);
    /// </summary>
    public abstract class CanvasElement : IHaveVisibility,IDrawable {
        private bool _isVisible = true;
        /// <summary>
        /// 是否可见;
        /// </summary>
        public bool IsVisible {
            get {
                return _isVisible;
            }
            set {
                if(_isVisible == value) {
                    return;
                }
                _isVisible = value;
                IsVisibleChanged?.Invoke(this, EventArgs.Empty);
                RaiseVisualChanged();
            }
        }


        //private bool _isEnabled;
        ///// <summary>
        ///// 是否可用;
        ///// </summary>
        //public bool IsEnabled {
        //    get {
        //        return _isEnabled;
        //    }
        //    set {
        //        if(_isEnabled == value) {
        //            return;
        //        }
        //        _isEnabled = value;
        //        IsEnabledChanged?.Invoke(this, EventArgs.Empty);
        //    }
        //}

        /// <summary>
        /// 可见变化事件;
        /// </summary>
        public event EventHandler IsVisibleChanged;

        /// <summary>
        /// 可用变化事件;
        /// </summary>
        //public event EventHandler IsEnabledChanged;

        /// <summary>
        /// 本身内容已经发生了变化的事件;
        /// </summary>
        public event EventHandler VisualChanged;

        /// <summary>
        /// 通知外部本身视觉内容已经发生了变化;
        /// </summary>
        public void RaiseVisualChanged() {
            VisualChanged?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Draw(ICanvas canvas,ICanvasScreenConvertable canvasProxy) {

        }
    }
}
