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
using Tida.CAD.DrawObjects;

namespace Tida.CAD.WPF.SimpleSample.Views
{
    /// <summary>
    /// Interaction logic for DrawObjectSample.xaml
    /// </summary>
    public partial class DrawObjectSample : Window
    {
        public DrawObjectSample()
        {
            InitializeComponent();
            _cadLayer = new CADLayer();
            cadControl.Layers = new CADLayer[] { _cadLayer };
        }
        private readonly CADLayer _cadLayer;
        private void Addline_Click(object sender, RoutedEventArgs e)
        {
            var line = new Line { Start = new Point(0, 0), End = new Point(10, 10) };
            line.Pen = new Pen { Thickness = 2, Brush = Brushes.White };
            _cadLayer.AddDrawObject(line);
        }

        private void AddRect_Click(object sender, RoutedEventArgs e)
        {
            var rectPen = new Pen(Brushes.White, 2);
            var rectBackground = Brushes.Orange;
            rectPen.Freeze();

            var rect = new Rectangle(new CADRect(new Point(-2, -2), new Size(4, 4)))
            {
                Pen = rectPen,
                Background = rectBackground
            };

            _cadLayer.AddDrawObject(rect);
        }


        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _cadLayer.Clear();
        }

        private void ChangeLayerBackground_Click(object sender, RoutedEventArgs e)
        {
            if(_cadLayer.Background == null)
            {
                _cadLayer.Background = Brushes.Blue;
            }
            else
            {
                _cadLayer.Background = null;
            }
        }
    }
}
