using Tida.Canvas.Shell.Contracts.App;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.App {
    /// <summary>
    /// 线程调用者实现器;
    /// </summary>
    [Export(typeof(IThreadInvoker))]
    class ThreadInvokerImpl : IThreadInvoker {
        public void BackInvoke(Action act) {
            if (act == null) {
                throw new ArgumentNullException(nameof(act));
            }
            
            ThreadPool.QueueUserWorkItem(cb => {
                act.Invoke();
            });
        }

        private readonly AutoResetEvent evt = new AutoResetEvent(false);
        public void UIInvoke(Action act) {
            if (act == null) {
                throw new ArgumentNullException(nameof(act));
            }


            System.Windows.Application.Current?.Dispatcher.Invoke(act);
        }
    }
}
