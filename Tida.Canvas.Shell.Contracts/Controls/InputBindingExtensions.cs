using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Contracts.Controls {
    
    public static class InputBindingExtensions {
        /// <summary>
        /// /// 添加快捷键绑定;
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="key">案件</param>
        /// <param name="modifier">修饰键</param>
        /// <param name="commandPara">命令参数</param>
        public static void AddKeyBinding(object commandTarget,ICommand command, Key key, ModifierKeys modifier = ModifierKeys.None) {
            var kb = new KeyBinding {
                Modifiers = modifier,
                Key = key,
                Command = command
            };
            var uiElem = commandTarget as UIElement;
            if (uiElem != null) {
                uiElem.InputBindings.Add(kb);
            }
        }
    }
}
