using Tida.Canvas.Events;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.Contracts {
    /// <summary>
    /// 具备部分鼠标位置状态管理单元的契约;
    /// </summary>
    public interface IHaveMousePositionTracker {
        /// <summary>
        /// 部分鼠标位置状态记录器;
        /// </summary>
        MousePositionTracker MousePositionTracker { get; }
    }

}
