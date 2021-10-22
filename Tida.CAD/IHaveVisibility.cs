using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD {
    /// <summary>
    /// 具备可见状态的契约;
    /// </summary>
    public interface IHaveVisibility:IDrawable {
        /// <summary>
        /// 是否可见;
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// 可见状态发生了变化契约;
        /// </summary>
        event EventHandler IsVisibleChanged;
    }
}
