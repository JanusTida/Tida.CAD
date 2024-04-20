using Tida.Geometry.Primitives;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.ComponentModel.Views {
    /// <summary>
    /// Interaction logic for Line2DEditor.xaml
    /// </summary>
    public partial class Line2DEditor : UserControl {
        public Line2DEditor() {
            InitializeComponent();
        }

        /// <summary>
        /// 线段信息是否正在被刷新,防止因为事件的原因导致的无限循环调用;
        /// </summary>
        private bool _line2DRefreshing;
        public event EventHandler Line2DChanged;

        /// <summary>
        /// 对应的Line2D数据;
        /// </summary>
        public Line2D  Line2D {
            get { return (Line2D)GetValue(Line2DProperty); }
            set { SetValue(Line2DProperty, value); }
        }
        
        public static readonly DependencyProperty Line2DProperty =
            DependencyProperty.Register(nameof(Line2D), typeof(Line2D), typeof(Line2DEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,Line2D_PropertyChanged));

        private static void Line2D_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (!(d is Line2DEditor line2DEditor)) {
                return;
            }

            if (!(e.NewValue is Line2D newLine2D)) {
                return;
            }

            line2DEditor.ApplyLine2DToEditors(newLine2D);
        }

        private void VectorEditor_Vector2DChanged(object sender, EventArgs e) => RefreshLine2D();

        /// <summary>
        /// 刷新<see cref="Line2D"/>属性
        /// </summary>
        /// <returns></returns>
        private void RefreshLine2D() {
            if (_line2DRefreshing) {
                return;
            }

            _line2DRefreshing = true;

            var newLine2D = GetInputLine2D();
            if(newLine2D != null) {
                Line2D = newLine2D;
                Line2DChanged?.Invoke(this, EventArgs.Empty);
            }
            else {
                ApplyLine2DToEditors(Line2D);
            }

            _line2DRefreshing = false;
        }
        
        /// <summary>
        /// 根据输入条件,获取线段;
        /// </summary>
        /// <returns></returns>
        private Line2D GetInputLine2D() {
            if(startVectorEditor.Vector2D == null) {
                return null;
            }

            if(endVectorEditor.Vector2D == null) {
                return null;
            }

            return new Line2D(startVectorEditor.Vector2D, endVectorEditor.Vector2D);
        }

        /// <summary>
        /// 将指定line2D数据同步至UI上;
        /// </summary>
        /// <param name="line2D"></param>
        private void ApplyLine2DToEditors(Line2D line2D) {
            if(line2D == null) {
                return;
            }

            startVectorEditor.Vector2D = line2D.Start;
            endVectorEditor.Vector2D = line2D.End;
        }
    }
}
