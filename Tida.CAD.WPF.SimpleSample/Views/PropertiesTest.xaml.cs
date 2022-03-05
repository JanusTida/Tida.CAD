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
    /// Interaction logic for DragSelect.xaml
    /// </summary>
    public partial class PropertiesTest
    {
        public PropertiesTest()
        {
            InitializeComponent();

            var cadLayer = new CADLayer();
            cadControl.Layers = new CADLayer[] { cadLayer };
            var line = new Line { Start = new Point(0, 0), End = new Point(10, 10)};
            line.Pen = new Pen { Thickness = 2, Brush = Brushes.White };
            cadLayer.AddDrawObject(line);
        }

       
    }
}
