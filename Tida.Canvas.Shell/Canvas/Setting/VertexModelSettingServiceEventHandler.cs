using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Shell.Contracts.Setting;
using Tida.Canvas.Shell.Contracts.Setting.Events;

namespace Tida.Canvas.Shell.Canvas.Setting {
    /// <summary>
    /// 设定服务初始化时,正交模式初始化;
    /// </summary>
    [Export(typeof(ISettingsServiceInitializeEventHandler))]
    class VertexModelSettingsServiceEventHandler : ISettingsServiceInitializeEventHandler {
        
        public bool IsEnabled => true;

        public int Sort => 256;

        public void Handle(ISettingsService settingsService) {

            if (settingsService == null) {
                throw new ArgumentNullException(nameof(settingsService));
            }

            
        }
    }
}
