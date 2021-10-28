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
using Tida.CAD.DrawObjects;

namespace Tida.CAD.WPF.SimpleSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _cadLayer = new CADLayer();
            cadControl.Layers = new CADLayer[] { _cadLayer };
        }
        private readonly CADLayer _cadLayer;
        private void Addline_Click(object sender, RoutedEventArgs e)
        {
            var line = new Line(new Point(0, 0), new Point(10, 10));
            line.Pen = new Pen { Thickness = 2, Brush = Brushes.White };
            _cadLayer.AddDrawObject(line);
        }
    }
}
