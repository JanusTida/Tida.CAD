using Tida.Canvas.Contracts;
using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemWindows = System.Windows;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.WPFCanvas.Input {

    /// <summary>
    /// 输入设备WPF封装;
    /// </summary>
    class InputDeviceWrapper : IInputDevice {
        public InputDeviceWrapper(SystemWindows.IInputElement inputElement) {
            _inputElement = inputElement ?? throw new ArgumentNullException(nameof(inputElement));
        }

        private SystemWindows.IInputElement _inputElement;
        
        ///// <summary>
        ///// 静态实例;
        ///// </summary>
        //private static InputDeviceWrapper _staticInstance;
        //public static InputDeviceWrapper StaticInstance => _staticInstance ?? (_staticInstance = new InputDeviceWrapper());

        public IKeyBoard KeyBoard => KeyBoardWrapper.StaticInstance;

        private IMouse _mouse;
        public IMouse Mouse => _mouse??(_mouse = new MouseWrapper(_inputElement));

        
    }
}
