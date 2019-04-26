using Tida.Application.Contracts.Common;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon.Events {
    /// <summary>
    /// 当前的选定的RibbonTab发生变化事件参数;
    /// </summary>
    public class SelectedRibbonTabChangedEventArgs : EventArgs {
        public SelectedRibbonTabChangedEventArgs(IRibbonTabMetaData ribbonTabMetaData) {

            RibbonTabMetaData = ribbonTabMetaData ?? throw new ArgumentNullException(nameof(ribbonTabMetaData));

        }


        public IRibbonTabMetaData RibbonTabMetaData { get; }
    }

    /// <summary>
    /// 当前的选定的RibbonTab发生变化事件;
    /// </summary>
    public class SelectedRibbonTabChangedEvent : PubSubEvent<SelectedRibbonTabChangedEventArgs> {

    }

    /// <summary>
    /// 当前的选定的RibbonTab发生变化事件处理器;
    /// </summary>
    public interface ISelectedRibbonTabChangedEventHandler : IEventHandler<SelectedRibbonTabChangedEventArgs> {
    }
    
}
