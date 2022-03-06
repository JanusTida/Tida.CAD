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
    /// Interaction logic for ClickSelectTest.xaml
    /// </summary>
    public partial class ClickSelectTest : Window
    {
        public ClickSelectTest()
        {
            InitializeComponent();

            _cadLayer = new CADLayer();
            cadControl.Layers = new CADLayer[] { _cadLayer };
            var rectPen = new Pen(Brushes.White, 2);
            var rectBackground = Brushes.Orange;
            rectPen.Freeze();

            var rect = new Rectangle(new CADRect(new Point(-2, -2), new Size(4, 4)))
            {
                Pen = rectPen,
                Background = rectBackground
            };

            _cadLayer.AddDrawObject(rect);

            var rect2 = new Rectangle(new CADRect(new Point(4, -2), new Size(4, 4)))
            {
                Pen = rectPen,
                Background = rectBackground
            };

            _cadLayer.AddDrawObject(rect2);
            this.DataContext = new ViewModel();
        }
        private readonly CADLayer _cadLayer;

        class ViewModel:BindableBase
        {
            public ViewModel()
            {
                
            }
            public ClickSelectMode[] ClickSelectModes { get; } = Enum.GetValues<ClickSelectMode>();


            private ClickSelectMode _selectedClickMode;
            public ClickSelectMode SelectedClickMode
            {
                get => _selectedClickMode;
                set => SetProperty(ref _selectedClickMode, value);
            }

        }
    }
}
