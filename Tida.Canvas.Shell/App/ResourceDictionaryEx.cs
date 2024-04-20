using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.App {
    public class ResourceDictionaryEx: ResourceDictionary {
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string Name { get; set; }
    }
}
