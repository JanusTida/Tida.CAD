using Tida.Geometry.Primitives;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace Tida.Canvas.Base.NativePresentation.Models {
    public class NumberBoxModel : BindableBase,INumberBox {
        public event EventHandler TabConfirmed;
        public event EventHandler EnterConfirmed;

        internal event EventHandler FocusRequest;
        internal event EventHandler<(int selectionStart,int selectionLength)> SelectRequest;
        internal event EventHandler<TextCompositionEventArgs> PreviewTextInput;
        internal event EventHandler<int> CaretIndexChanged;
        
        internal bool IsFocused { get; set; }
        
        public bool Visible {
            get => Visibility == Visibility.Visible; 
            set {
                if(Visible == value) {
                    return;
                }

                if (value) {
                    Visibility = Visibility.Visible;
                }
                else {
                    Visibility = Visibility.Hidden;
                }
            }
        }


        private Visibility _visibility;
        public Visibility Visibility {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }


        private Thickness _margin;
        public Thickness Margin {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }
        
        public bool IsError => Number == null;
        
        private string _text;
        /// <summary>
        /// 显示的文本;
        /// </summary>
        public string Text {
            get { return _text; }
            set {
                if(_text == value) {
                    return;
                }

                SetTextCore(value);
                _number = null;
                if(double.TryParse(value, out var number)) {
                    _number = number;
                }
                
                RaisePropertyChanged(nameof(IsError));
            }
        }

        /// <summary>
        /// 设定文字核心;
        /// </summary>
        /// <param name="text"></param>
        private void SetTextCore(string text) {
            SetProperty(ref _text, text, nameof(Text));

            RaisePropertyChanged(nameof(IsError));
        }

        private double? _number;

        public double? Number {
            get {
                if(_number != null) {
                    return _number.Value;
                }

                if(double.TryParse(Text,out var number)) {
                    return number;
                }

                return null;
            }
            set {
                if(Number == value) {
                    return;
                }

                _number = value;
                string text = null;
                
                if (value == null) {
                    text = string.Empty;
                }
                else {
                    text = ((double)(int)(value.Value * _numberToDevide) / _numberToDevide).ToString();
                }

                SetTextCore(text);

                if (IsReadOnly) {
                    return;
                }

                SelectAll();
                
                if (FocusOnParent == null) {
                    return;
                }

                Focus();
                FocusOnParent();
            }
        }



        /// <summary>
        /// 因为WPF<see cref="System.Windows.Controls.TextBox"/> 需要提前聚焦一遍,
        /// 才能高亮选中的内容,(即使设定了<see cref="System.Windows.Controls.TextBox.IsInactiveSelectionHighlightEnabled"/> 为真),
        /// 这里由内部处理进行聚焦一遍后再还原回聚焦的父元素;：(
        /// </summary>
        internal Action FocusOnParent { get; set; }


        public Vector2D Position {
            get => new Vector2D(Margin.Left, Margin.Top);
            set => Margin = new Thickness(value.X, value.Y, 0, 0);
        }

        
        private bool _isReadOnly;
        public bool IsReadOnly {
            get => _isReadOnly; 
            set => SetProperty(ref _isReadOnly, value);
        }



        private const int DefaultSaveBits = 4;
        private int _saveBits = DefaultSaveBits;
        ///<summary>
        /// 保留小数点小数的位数;
        /// </summary>
        public int SavedBits {
            get => _saveBits;
            set {
                if(value < 0) {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} can't be less than zero.");
                }

                _saveBits = value;
                _numberToDevide = GetNumberToDevide(_saveBits);
            }
        }

        /// <summary>
        /// 用于计算输入数字显示被整除的数;
        /// </summary>
        private int _numberToDevide = GetNumberToDevide(DefaultSaveBits);
        private static int GetNumberToDevide(int saveBits) {
            if(saveBits < 0) {
                throw new ArgumentOutOfRangeException(nameof(saveBits));
            }

            var number = 1;
            for (int i = 0; i < saveBits; i++) {
                number *= 10;
            }

            return number;
        }
        
        internal void TabConfirm() {
            if(Number == null) {
                return;
            }
            TabConfirmed?.Invoke(this, EventArgs.Empty);
        }

        internal void EnterConfirm() {
            if(Number == null) {
                return;
            }

            EnterConfirmed?.Invoke(this, EventArgs.Empty);
        }

        internal void RaisePreviewText(TextCompositionEventArgs e) {
            PreviewTextInput?.Invoke(this, e);
        }

        public void Select(int selectionStart,int selectionLength) {
            if (string.IsNullOrEmpty(Text)) {
                return;
            }
            
            if(selectionStart < 0 || selectionLength + selectionStart > Text.Length) {
                return;
            }

            SelectRequest?.Invoke(this, (selectionStart, selectionLength));
        }

        public void SelectAll() {
            if (string.IsNullOrEmpty(Text)) {
                return;
            }
            Select(0, Text.Length);
        }
        
        public void Focus() {
            
            FocusRequest?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 设定修改符(即"|")偏移;
        /// </summary>
        public void SetCaretIndex(int caretIndex) {
            if(caretIndex > (Text?.Length ?? 0) || caretIndex < 0) {
                return;
            }

            CaretIndexChanged?.Invoke(this, caretIndex);
        }

#if DEBUG
        ~NumberBoxModel() {

        }

#endif
    }
}
