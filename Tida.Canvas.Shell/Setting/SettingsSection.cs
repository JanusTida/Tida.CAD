using Tida.Canvas.Shell.Contracts.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Setting {
    public class SettingsSection : ISettingsSection {
        public SettingsSection(string guid) {
            this.GUID = guid;
        }

        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();

        public IEnumerable<Tuple<string, string>> Attributes => _attributes.Select(p => new Tuple<string,string>(p.Key,p.Value));

        public string GUID { get; }

        public ISettingsSection Clone() {
            var section = new SettingsSection(this.GUID);
            foreach (var attr in _attributes) {
                section._attributes[attr.Key] = attr.Value;
            }
            
            return section;
        }

        public T GetAttribute<T>(string name) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            string stringValue = null;
            if (!_attributes.TryGetValue(name, out stringValue)) {
                return default(T);
            }

            if (converter == null) {
                return default(T);
            }

            try {
                return (T)converter.ConvertFromInvariantString(stringValue);
            }
            catch(Exception ex) {
                return default(T); 
            }
        }

        public bool RemoveAttribute(string name) {

            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            return _attributes.Remove(name);
        }

        public bool SetAttribute<T>(string name, T value) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }
            
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if(converter == null) {
                return false;
            }

            try {
                _attributes[name] = converter.ConvertToInvariantString(value);
                return true;
            }
            catch(Exception ex) {
                return false;
            }

        }
    }
}
