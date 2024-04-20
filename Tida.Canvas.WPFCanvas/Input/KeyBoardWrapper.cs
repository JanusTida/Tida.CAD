using Tida.Canvas.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInput = System.Windows.Input;

namespace Tida.Canvas.WPFCanvas.Input {
    /// <summary>
    /// WPF键盘服务封装;
    /// </summary>
    class KeyBoardWrapper : IKeyBoard {
        /// <summary>
        /// 静态实例;
        /// </summary>
        private static KeyBoardWrapper _staticInstance;
        public static KeyBoardWrapper StaticInstance => _staticInstance ?? (_staticInstance = new KeyBoardWrapper());

        private KeyBoardWrapper() {}
        public ModifierKeys ModifierKeys => KeyAdapter.ConvertToModifierKeys(SystemInput.Keyboard.Modifiers);

        public bool IsKeyDown(Key key) => SystemInput.Keyboard.IsKeyDown(KeyAdapter.ConvertToSystemKey(key));

        public bool IsKeyToggled(Key key) => SystemInput.Keyboard.IsKeyToggled(KeyAdapter.ConvertToSystemKey(key));

        public bool IsKeyUp(Key key) => SystemInput.Keyboard.IsKeyUp(KeyAdapter.ConvertToSystemKey(key));


        /// <summary>
        /// 键盘按下事件;
        /// </summary>
        public event EventHandler<KeyDownEventArgs> PreviewKeyDown;

        /// <summary>
        /// 键盘弹起事件;
        /// </summary>
        public event EventHandler<KeyUpEventArgs> PreviewKeyUp;

        /// <summary>
        /// 输入事件;
        /// </summary>
        public event EventHandler<TextInputEventArgs> PreviewTextInput;


    }
}
