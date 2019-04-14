using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    /// <summary>
    /// 笔的线段样式;(未实现);
    /// </summary>
    public class DashStyle {
        /// <summary>
        /// 偏移;
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// 虚线点分布;
        /// </summary>
        public IEnumerable<double> Dashes { get; set; }

    }
}
