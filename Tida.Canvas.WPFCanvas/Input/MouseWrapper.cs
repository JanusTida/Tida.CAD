using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Geometry.Primitives;
using Tida.Canvas.Input;
using SystemInput = System.Windows.Input;
using SystemWindows = System.Windows;
using Tida.Canvas.WPFCanvas.Geometry;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.WPFCanvas.Input {
    /// <summary>
    /// 鼠标服务WPF封装;
    /// </summary>
    class MouseWrapper : IMouse {
        public MouseWrapper(SystemWindows.IInputElement inputElement) {
            _inputElement = inputElement ?? throw new ArgumentNullException(nameof(inputElement));
        }
        
        private readonly SystemWindows.IInputElement _inputElement;
        
        public event EventHandler<MouseDownEventArgs> PreviewMouseDown;
        public event EventHandler<MouseMoveEventArgs> PreviewMouseMove;
        public event EventHandler<MouseUpEventArgs> PreviewMouseUp;

       
        public void RaisePreviewMouseDown(MouseDownEventArgs e) {
            PreviewMouseDown?.Invoke(this, e);
        }

        public void RaisePreviewMouseMove(MouseMoveEventArgs e) {
            PreviewMouseMove?.Invoke(this, e); 
        }

        public void RaisePreviewMouseUp(MouseUpEventArgs e) {
            PreviewMouseUp?.Invoke(this, e);
        }


        public MouseButtonState XButton2 => MouseButtonStateAdapter.ConvertToMouseButtonState(SystemInput.Mouse.XButton2);

        public MouseButtonState MiddleButton => MouseButtonStateAdapter.ConvertToMouseButtonState(SystemInput.Mouse.MiddleButton);

        public MouseButtonState RightButton => MouseButtonStateAdapter.ConvertToMouseButtonState(SystemInput.Mouse.RightButton);

        public MouseButtonState LeftButton => MouseButtonStateAdapter.ConvertToMouseButtonState(SystemInput.Mouse.LeftButton);

        public MouseButtonState XButton1 => MouseButtonStateAdapter.ConvertToMouseButtonState(SystemInput.Mouse.XButton1);

        public Vector2D GetNativePosition() {
           
            return Vector2DAdapter.ConverterToVector2D(
                SystemInput.Mouse.GetPosition(_inputElement)
            );
        }
    }
}
