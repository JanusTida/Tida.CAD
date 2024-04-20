using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Contracts {
    /// <summary>
    /// 根据画布信息,画布的交互信息的预处理器;
    /// 本接口适用范围为在通知编辑工具或绘制对象前，需预处理关注位置的情况,
    /// 如"正交模式"下的关注位置需在编辑工具或绘制对象处理之前进行修改;
    /// 主代码将处理器返回的结果用于通知编辑工具或绘制对象;
    /// </summary>
    public abstract class CanvasInteractionHandler : CanvasElement,IDrawable,IDisposable {
        private ICanvasControl _canvasControl;
        /// <summary>
        /// 对应的画布控件;
        /// 在更改本属性的值时,将会调用<see cref="OnUnLoad(ICanvasControl)"/>以及<see cref="OnLoad(ICanvasControl)"/>
        /// </summary>
        public ICanvasControl CanvasControl {
            get => _canvasControl;
            set {
                if (_canvasControl == value) {
                    return;
                }

                if (_canvasControl != null) {
                    OnUnLoad(_canvasControl);
                }

                _canvasControl = value;

                if(_canvasControl != null) {
                    OnLoad(_canvasControl);
                }
            }
        }
        
        public void Dispose() {
            if (_disposed) {
                throw new ObjectDisposedException("The object has already been disposed.");
            }

            OnDispose();
        }

        /// <summary>
        /// 是否被释放;
        /// </summary>
        private bool _disposed;
        protected virtual void OnDispose() {

        }

        public virtual void HandlePosition(ICanvasControl canvasContext, Vector2D oriPosition) { }

        protected virtual void OnLoad(ICanvasControl canvasControl) { }

        protected virtual void OnUnLoad(ICanvasControl canvasControl) { }
        
    }
}
