using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tida.Canvas.Shell.ComponentModel.Views {
    /// <summary>
    /// Interaction logic for Ellipse2DEditor.xaml
    /// </summary>
    public partial class Ellipse2DEditor : UserControl {
        public Ellipse2DEditor() {
            InitializeComponent();
        }

        /// <summary>
        /// 椭圆(圆)信息是否正在被刷新,防止因为事件的原因导致的无限循环调用;
        /// </summary>
        private bool _ellipse2DRefreshing = false;
        public event EventHandler Ellipse2DChanged;



        /// <summary>
        /// 对应的Ellipse2D数据;
        /// </summary>
        public Ellipse2D Ellipse2D {
            get { return (Ellipse2D)GetValue(Ellipse2DProperty); }
            set { SetValue(Ellipse2DProperty, value); }
        }

        public static readonly DependencyProperty Ellipse2DProperty =
            DependencyProperty.Register(nameof(Ellipse2D), typeof(Ellipse2D), typeof(Ellipse2DEditor), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,Ellipse2D_PropertyChanged));

        private static void Ellipse2D_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(!(d is Ellipse2DEditor eEditor)) {
                return;
            }

            if(!(e.NewValue is Ellipse2D newEllipse2D)) {
                return;
            }

            eEditor.ApplyEllipse2DToEditors(newEllipse2D);
        }


        /// <summary>
        /// 将指定的<see cref="Ellipse2D"/>数据同步至UI;
        /// </summary>
        private void ApplyEllipse2DToEditors(Ellipse2D ellipse2D) {
            if(ellipse2D == null) {
                return;
            }

            positionVector2DEditor.Vector2D = ellipse2D.Center;
            txb_RadiusX.Text = ellipse2D.RadiusX.ToString();
            txb_RadiusY.Text = ellipse2D.RadiusY.ToString();
        }


        private void RefreshEllipse2D() {
            if (_ellipse2DRefreshing) {
                return;
            }

            _ellipse2DRefreshing = true;
            var newEllipse2D = GetInputEllipse2D();
            if(newEllipse2D != null) {
                Ellipse2D = newEllipse2D;
                Ellipse2DChanged?.Invoke(this, EventArgs.Empty);
            }
            else {
                ApplyEllipse2DToEditors(Ellipse2D);
            }
            
            _ellipse2DRefreshing = false;
        }

        /// <summary>
        /// 根据当前UI的输入数据获取Ellipse数据;
        /// </summary>
        /// <returns></returns>
        private Ellipse2D GetInputEllipse2D() {
            if (positionVector2DEditor.Vector2D == null) {
                return null;
            }

            if (!double.TryParse(txb_RadiusX.Text, out var radiusX)) {
                return null;
            }

            if (!double.TryParse(txb_RadiusY.Text, out var radiusY)) {
                return null;
            }

            return new Ellipse2D(positionVector2DEditor.Vector2D, radiusX, radiusY);
        }
        
        private void PositionVector2DEditor_Vector2DChanged(object sender, EventArgs e) => RefreshEllipse2D();

        private void Txb_Radius_TextInputChanged(object sender, EventArgs e) => RefreshEllipse2D();
    }
}
