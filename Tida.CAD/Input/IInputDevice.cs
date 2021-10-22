using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 输入设备服务封装;
    /// </summary>
    public interface IInputDevice {

        /// <summary>
        /// 键盘服务;
        /// </summary>
        IKeyBoard KeyBoard { get; }

        /// <summary>
        /// 鼠标服务;
        /// </summary>
        IMouse Mouse { get; }
    }
}
