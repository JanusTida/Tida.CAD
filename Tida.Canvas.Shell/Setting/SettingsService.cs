using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Setting;
using Tida.Canvas.Shell.Contracts.Setting.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Setting {
    [Export(typeof(ISettingsService))]
    public class SettingsService : ISettingsService {
        private readonly List<ISettingsSection> _sections = new List<ISettingsSection>();
        /// <summary>
        /// 控制多线程安全的锁对象;
        /// </summary>
        //private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

        public IEnumerable<ISettingsSection> Sections => _sections;

        public ISettingsSection GetOrCreateSection(string guid) {
            if (guid == null) {
                throw new ArgumentNullException(nameof(guid));
            }
            
            
            var section = _sections.FirstOrDefault(p => p.GUID == guid);

            if(section == null) {
                section = new SettingsSection(guid);
                
                _sections.Add(section);
                
            }

            return section;
        }

        public void Initialize() {
            var dir = AppDomainService.ExecutingAssemblyDirectory;
            try {
                CommonEventHelper.Publish<SettingsServiceInitializeEvent,ISettingsService>(this);
                CommonEventHelper.PublishEventToHandlers<ISettingsServiceInitializeEventHandler,ISettingsService>(this);
            }
            catch(Exception ex) {

            }
        }

        public void RemoveSection(ISettingsSection settingsSection) {

            if (settingsSection == null) {
                throw new ArgumentNullException(nameof(settingsSection));
            }

            _sections.Remove(settingsSection);
        }
    }
}
