using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 输入元素契约;
    /// </summary>
    public interface ICanvasInputElement {

        /// <summary>
        /// 通知将执行<see cref="IEditTool.OnPreviewMouseDown(ICanvasContextEx, MouseDownEventArgs)"/>动作;
        /// </summary>
        event EventHandler<MouseDownEventArgs> CanvasPreviewMouseDown;

        /// <summary>
        /// 通知将执行<see cref="IEditTool.OnPreviewMouseMove(ICanvasContextEx, MouseMoveEventArgs)"/>动作;
        /// </summary>
        event EventHandler<MouseMoveEventArgs> CanvasPreviewMouseMove;

        /// <summary>
        /// 通知将执行<see cref="IEditTool.OnPreviewMouseUp(ICanvasContextEx, MouseUpEventArgs)"/>动作;
        /// </summary>
        event EventHandler<MouseUpEventArgs> CanvasPreviewMouseUp;

        /// <summary>
        /// 通知将执行<see cref="IEditTool.OnPreviewKeyDown(ICanvasContextEx, KeyDownEventArgs)"/>动作;
        /// </summary>
        event EventHandler<KeyDownEventArgs> CanvasPreviewKeyDown;

        /// <summary>
        /// 通知将执行<see cref="IEditTool.OnPreviewKeyUp(ICanvasContextEx, KeyUpEventArgs)"/>动作;
        /// </summary>
        event EventHandler<KeyUpEventArgs> CanvasPreviewKeyUp;

        /// <summary>
        /// 键入了文字事件;
        /// </summary>
        event EventHandler<TextInputEventArgs> CanvasPreviewTextInput;

        /// <summary>
        /// 触发鼠标按下动作;
        /// </summary>
        /// <param name="e"></param>
        void RaisePreviewMouseDown(MouseDownEventArgs e);

        /// <summary>
        /// 触发鼠标移动动作;
        /// </summary>
        /// <param name="e"></param>
        void RaisePreviewMouseMove(MouseMoveEventArgs e);

        /// <summary>
        /// 触发鼠标弹起动作;
        /// </summary>
        /// <param name="e"></param>
        void RaisePreviewMouseUp(MouseUpEventArgs e);

        /// <summary>
        /// 触发按键按下动作;
        /// </summary>
        /// <param name="e"></param>
        void RaisePreviewKeyDown(KeyDownEventArgs e);

        /// <summary>
        /// 触发按键弹起动作;
        /// </summary>
        /// <param name="e"></param>
        void RaisePreviewKeyUp(KeyUpEventArgs e);

        /// <summary>
        /// 触发文本输入动作;
        /// </summary>
        /// <param name="e"></param>
        void RaisePreviewTextInput(TextInputEventArgs e);
    }
}
