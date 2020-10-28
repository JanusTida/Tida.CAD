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
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace SimpleSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            canvasControl.Layers = new CanvasLayer[]
            {
                _canvasLayer = new CanvasLayer()
            };
        }

        private readonly CanvasLayer _canvasLayer;

        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            var line = new Tida.Canvas.Infrastructure.DrawObjects.Line(new Vector2D(-3,3),new Vector2D(3,-3));
            _canvasLayer.AddDrawObject(line);
        }

        private void AddCustomObject_Click(object sender, RoutedEventArgs e)
        {
            var customObject = new CustomObject();
            _canvasLayer.AddDrawObject(customObject);
        }

        private void RemovedSelected_Click(object sender, RoutedEventArgs e)
        {
            _canvasLayer.RemoveDrawObjects(_canvasLayer.DrawObjects.Where(p => p.IsSelected).ToArray());
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _canvasLayer.Clear();
        }
    }
}
