using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 动态输入处理器基类;
    /// </summary>
    public abstract class DynamicInputerBase: CanvasElement,IDynamicInputer {

        public void Dispose() {
            if (_disposed) {
                throw new ObjectDisposedException($"The object has already been disposed.");
            }

            _disposed = true;
            OnDispose();
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDispose() {
            
        }

        private bool _disposed;
        /// <summary>
        /// 已经调用了Dispose
        /// </summary>
        public event EventHandler Disposed;
    }
}
