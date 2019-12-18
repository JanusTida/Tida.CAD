using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 无参的事件处理器;
    /// </summary>
    public interface IEventHandler {
        /// <summary>
        /// 处理排序;
        /// </summary>
        int Sort { get; }
        /// <summary>
        /// 处理;
        /// </summary>
        /// <param name="args"></param>
        void Handle();
        /// <summary>
        /// 是否可用;
        /// </summary>
        bool IsEnabled { get; }
    }

    /// <summary>
    /// 附带参数的事件处理器,在使用事件驱动模式时,使事件的订阅者所订阅动作按照排序进行;
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public interface IEventHandler<TEventArgs> {
        /// <summary>
        /// 处理排序;
        /// </summary>
        int Sort { get; }
        /// <summary>
        /// 处理;
        /// </summary>
        /// <param name="args"></param>
        void Handle(TEventArgs args);
        /// <summary>
        /// 是否可用;
        /// </summary>
        bool IsEnabled { get; }
    }


    public abstract class EventHandlerBase<TEventArgs> : IEventHandler<TEventArgs> {
        public virtual bool IsEnabled => true;

        public virtual int Sort => 0;

        public abstract void Handle(TEventArgs args);
    }

}
