using Tida.Canvas.Shell.Contracts.Common;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Setting.Events {
    /// <summary>
    /// 设定服务初始化事件;
    /// </summary>
    public class SettingsServiceInitializeEvent:PubSubEvent<ISettingsService> {
    }

    public interface ISettingsServiceInitializeEventHandler:IEventHandler<ISettingsService> {

    }
}
