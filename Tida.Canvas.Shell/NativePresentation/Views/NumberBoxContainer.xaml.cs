using Tida.Canvas.Shell.NativePresentation.Models;
using Tida.Canvas.Events;
using Tida.Canvas.Infrastructure.NativePresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Tida.Canvas.Shell.NativePresentation.Views {
    /// <summary>
    /// Interaction logic for NumberBoxContainer.xaml
    /// </summary>
    public partial class NumberBoxContainer : ContentControl, INumberBoxContainer {
        public NumberBoxContainer() {
            InitializeComponent();
            
            if (System.Windows.Application.Current.MainWindow != null) {
                TextCompositionManager.AddPreviewTextInputHandler(System.Windows.Application.Current.MainWindow, MainWindow_PreviewTextInput);
            }
            
        }

        private void MainWindow_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            var numberBox = GetCurrentWritableNumberBox();
            if(numberBox == null) {
                return;
            }
            
            
            if (numberBox.IsFocused) {
                return;
            }
            numberBox.Focus();

            if (!numberBox.IsFocused) {
                return;
            }
            
            numberBox.RaisePreviewText(e);
            e.Handled = true;
        }
        
        /// <summary>
        /// 输入文字时将正在编辑的输入框聚焦;
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewTextInput(TextCompositionEventArgs e) {
            if (IsInputing) {
                base.OnPreviewTextInput(e);
                return;
            }

            e.Handled = true;

            var currentWritableNumberBox = GetCurrentWritableNumberBox();
            if(currentWritableNumberBox == null) {
                return;
            }

            if (e.Text == "\b") {
                return;
            }
            else {
                SetIsInputingState(true);
                
                currentWritableNumberBox.Text = e.Text;

                if (string.IsNullOrEmpty(e.Text)) {
                    return;
                }

                currentWritableNumberBox.SetCaretIndex(e.Text.Length);
            }
        }

        /// <summary>
        /// 获取当前正在"编辑"的输入框;
        /// </summary>
        /// <returns></returns>
        private NumberBoxModel GetCurrentWritableNumberBox() => _numberBoxes.FirstOrDefault(p => p.IsReadOnly == false);

        protected override void OnPreviewKeyDown(KeyEventArgs e) {
            if(e.Key == Key.Tab) {
                e.Handled = OnTabKeyDown();
            }
            else if(e.Key == Key.Enter) {
                e.Handled = OnEnterKeyDown();
            }
            else if(e.Key == Key.Left) {
                e.Handled = OnLeftKeyDown();
            }
            else if(e.Key == Key.Right) {
                e.Handled = OnRightKeyDown();
            }

            base.OnPreviewKeyDown(e);
        }


        /// <summary>
        /// Tab键按下时的响应;
        /// </summary>
        private bool OnTabKeyDown() {
            var currentWritableNumberBox = GetCurrentWritableNumberBox();
            if (currentWritableNumberBox?.Number == null) {
                return true;
            }

            var index = _numberBoxes.IndexOf(currentWritableNumberBox);
            NumberBoxModel nextNumberBox = null;

            if (index == _numberBoxes.Count - 1) {
                nextNumberBox = _numberBoxes[0];
            }
            else {
                nextNumberBox = _numberBoxes[index + 1];
            }

            currentWritableNumberBox.IsReadOnly = true;
            
            if (IsInputing) {
                IsInputing = false;
                currentWritableNumberBox.TabConfirm();
            }
            
            nextNumberBox.IsReadOnly = false;

            nextNumberBox.SelectAll();
            nextNumberBox.Focus();
            
            return true;
        }

        /// <summary>
        /// 回车键按下时的响应;
        /// </summary>
        /// <returns>是否已处理</returns>
        private bool OnEnterKeyDown() {
            var currentWritableNumberBox = GetCurrentWritableNumberBox();
            if (currentWritableNumberBox?.Number == null) {
                return false;
            }

            currentWritableNumberBox.EnterConfirm();
            return true;
        }

        /// <summary>
        /// 方向左键按下时的响应;
        /// </summary>
        private bool OnLeftKeyDown() {
            var currentWritableNumberBox = GetCurrentWritableNumberBox();
            if (currentWritableNumberBox?.Number == null) {
                return false;
            }
            
            if (IsInputing) {
                return false;
            }

            
            currentWritableNumberBox.Focus();
            currentWritableNumberBox.Select(0, 0);
            currentWritableNumberBox.SetCaretIndex(0);

            SetIsInputingState(true);

            return true;
        }
        
        /// <summary>
        /// 方向右键按下时的响应;
        /// </summary>
        private bool OnRightKeyDown() {
            var currentWritableNumberBox = GetCurrentWritableNumberBox();
            if (currentWritableNumberBox?.Number == null) {
                return false;
            }

            if (IsInputing) {
                return false;
            }
           
            currentWritableNumberBox.Focus();
            currentWritableNumberBox.Select(0, 0);
            currentWritableNumberBox.SetCaretIndex(currentWritableNumberBox.Text.Length);

            SetIsInputingState(true);

            return true;
        }

        public IReadOnlyList<INumberBox> NumberBoxes => _numberBoxes;

        private readonly List<NumberBoxModel> _numberBoxes = new List<NumberBoxModel>();

        public event EventHandler<ValueChangedEventArgs<bool>> IsInputingChanged;

        public object UIObject => this;

        private bool _isInputing;
        public bool IsInputing {
            get => _isInputing;
            private set {
                if(_isInputing == value) {
                    return;
                }
                _isInputing = value;
                IsInputingChanged?.Invoke(this, new ValueChangedEventArgs<bool>(_isInputing, !_isInputing));
            }
        }

        /// <summary>
        /// 设定是否为正在输入状态;
        /// </summary>
        /// <param name="isInputing"></param>
        private void SetIsInputingState(bool isInputing) {
            var currentWritableNumberBox = GetCurrentWritableNumberBox();
            if (currentWritableNumberBox?.Number == null) {
                return;
            }

            if (isInputing) {
                currentWritableNumberBox.Focus();
                IsInputing = true;
            }
            else {
                currentWritableNumberBox.Focus();
                currentWritableNumberBox.SelectAll();
                this.Focus();
                IsInputing = false;
            }
        }

        public void AddNumberBox(INumberBox numberBox) {
            if (!(numberBox is Models.NumberBoxModel model)) {
                throw new InvalidCastException(nameof(numberBox));
            }

            _numberBoxes.Add(model);
            model.FocusOnParent = FocusInternal;
            grid.Children.Add(new NumberBox {
                DataContext = numberBox
            });
        }

        private void FocusInternal() => Focus();

        public void RemoveNumberBox(INumberBox numberBox) {
            if (!(numberBox is Models.NumberBoxModel model)) {
                throw new InvalidCastException(nameof(numberBox));
            }

            _numberBoxes.Remove(model);
            model.FocusOnParent = null;

            NumberBox numberBoxView = null;
            foreach (var child in grid.Children) {
                if(child is NumberBox thisNumberBoxView && thisNumberBoxView.DataContext == numberBox) {
                    numberBoxView = thisNumberBoxView;
                    break;
                }
            }

            if(numberBoxView == null) {
                return;
            }

            grid.Children.Remove(numberBoxView);
        }

        /// <summary>
        /// 复位;
        /// </summary>
        public void Reset() {
            _numberBoxes.ForEach(p => p.IsReadOnly = true);

            if (_numberBoxes.Count == 0) {
                return;
            }
            
            _numberBoxes[0].IsReadOnly = false;

            SetIsInputingState(false);
        }

        public void Dispose() {
            if (System.Windows.Application.Current.MainWindow != null) {
                TextCompositionManager.RemovePreviewTextInputHandler(System.Windows.Application.Current.MainWindow, MainWindow_PreviewTextInput);
            }
        }

#if DEBUG
        ~NumberBoxContainer() {

        }
#endif
    }
}
