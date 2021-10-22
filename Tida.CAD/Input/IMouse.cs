using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 鼠标封装;
    /// </summary>
    public interface IMouse {
        //
        // Summary:
        //     Gets the state of the first extended button.
        //
        // Returns:
        //     The state of the first extended mouse button.
        MouseButtonState XButton1 { get; }

        //
        // Summary:
        //     Gets the state of the second extended button.
        //
        // Returns:
        //     The state of the second extended mouse button.
        MouseButtonState XButton2 { get; }

        //
        // Summary:
        //     Gets the state of the middle button of the mouse.
        //
        // Returns:
        //     The state of the middle mouse button.
        MouseButtonState MiddleButton { get; }
        //
        // Summary:
        //     Gets the state of the right button.
        //
        // Returns:
        //     The state of the right mouse button.
        MouseButtonState RightButton { get; }
        //
        // Summary:
        //     Gets the state of the left button of the mouse.
        //
        // Returns:
        //     The state of the left mouse button.
        MouseButtonState LeftButton { get; }

        /// <summary>
        /// 获取鼠标的视图坐标位置;
        /// </summary>
        /// <returns></returns>
        Point GetNativePosition();


    }
}
