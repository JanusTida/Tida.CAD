using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tida.Canvas.Shell.ComponentModel.Views {
    class StandardInputTextBox : TextBox {
        private bool _textChanged;
        protected override void OnTextChanged(TextChangedEventArgs e) {
            _textChanged = true;
            base.OnTextChanged(e);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.Key != Key.Enter) {
                return;
            }

            if (_textChanged) {
                TextInputChanged?.Invoke(this, EventArgs.Empty);
            }

            _textChanged = false;

            base.OnKeyDown(e);
        }

        public event EventHandler TextInputChanged;

        protected override void OnLostFocus(RoutedEventArgs e) {
            if (_textChanged) {
                TextInputChanged?.Invoke(this, EventArgs.Empty);
            }

            _textChanged = false;
            base.OnLostFocus(e);
        }
    }

}
