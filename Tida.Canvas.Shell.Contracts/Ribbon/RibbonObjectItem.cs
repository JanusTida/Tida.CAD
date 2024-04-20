using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// Ribbon项(自定义UI);
    /// </summary>
    public class RibbonObjectItem : IRibbonObjectItem {
        public object UIObject { get; set; }
    }
}
