using Tida.Canvas.Events;
using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD {
    /// <summary>
    /// 具备编辑工具契约;
    /// </summary>
    public interface  IHaveEditTool {
        /// <summary>
        /// 当前使用的编辑工具;
        /// </summary>
        EditTool CurrentEditTool { get; set; }

        /// <summary>
        /// 当前的编辑工具变化事件;
        /// </summary>
        event EventHandler<ValueChangedEventArgs<EditTool>> CurrentEditToolChanged;

    }
}
