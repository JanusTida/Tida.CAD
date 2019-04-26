using Tida.Geometry.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tida.Canvas.Shell.ComponentModel.Views {
    /// <summary>
    /// Interaction logic for Vector2DEditor.xaml
    /// </summary>
    public partial class Vector2DEditor : UserControl {
        public Vector2DEditor() {
            InitializeComponent();
        }

        /// <summary>
        /// 位置信息是否正在被刷新,防止因为事件的原因导致的无限循环调用;
        /// </summary>
        private bool _vector2DRefreshing;
        private bool _textChanged = false;
        public event EventHandler Vector2DChanged;

        public Vector2D Vector2D {
            get { return (Vector2D)GetValue(Vector2DProperty); }
            set { SetValue(Vector2DProperty, value); }
        }
        
        public static readonly DependencyProperty Vector2DProperty =
            DependencyProperty.Register(nameof(Vector2D), typeof(Vector2D), typeof(Vector2DEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, Vector2D_PropertyChanged));
        
        private static void Vector2D_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(!(d is Vector2DEditor vector2DEditor)) {
                return;
            }

            if(!(e.NewValue is Vector2D newVector2D)) {
                return;
            }

            vector2DEditor.ApplyVector2DToTextBox(newVector2D);
        }
        
        
        /// <summary>
        /// 刷新<see cref="Vector2D"/>属性
        /// </summary>
        /// <returns></returns>
        private void RefreshVector2D() {
            if (_vector2DRefreshing) {
                return;
            }

            _vector2DRefreshing = true;

            var newVector2D = GetInputVector2D();
            if(newVector2D != null) {
                Vector2D = newVector2D;
                Vector2DChanged?.Invoke(this, EventArgs.Empty);
            }
            else {
                ApplyVector2DToTextBox(Vector2D);
            }

            _vector2DRefreshing = false;
        }

        /// <summary>
        /// 根据输入条件,获取位置;
        /// </summary>
        /// <returns></returns>
        private Vector2D GetInputVector2D() {
            if (!(double.TryParse(txb_X.Text, out var x))) {
                return null;
            }

            if (!(double.TryParse(txb_Y.Text, out var y))) {
                return null;
            }

            return new Vector2D(x, y);
        }
        
        private void ApplyVector2DToTextBox(Vector2D vector2D) {
            if (vector2D == null) {
                return;
            }

            txb_X.Text = vector2D.X.ToString();
            txb_Y.Text = vector2D.Y.ToString();
        }

        private void Txb_TextInputChanged(object sender, EventArgs e) => RefreshVector2D();

        ~Vector2DEditor() {

        }
        
    }
}
