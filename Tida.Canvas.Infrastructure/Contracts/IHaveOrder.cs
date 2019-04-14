using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.Contracts {
    public interface IHaveOrder {
        /// <summary>
        /// 排序;
        /// </summary>
        int Order { get; }
    }
}
