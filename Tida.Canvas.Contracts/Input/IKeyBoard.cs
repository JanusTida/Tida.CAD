using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 键盘服务抽象;
    /// </summary>
    public interface IKeyBoard {
        /// <summary>
        /// 获取按键是否按下;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsKeyDown(Key key);


        /// <summary>
        /// 获取按键是否弹起;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsKeyUp(Key key);

        /// <summary>
        /// 获取按键是否Toggle;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsKeyToggled(Key key);

        /// <summary>
        /// 修饰键状态;
        /// </summary>
        ModifierKeys ModifierKeys { get; }

    }
}
