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
using System.Windows.Shapes;

namespace Tida.CAD.WPF.SimpleSample.Views
{
    /// <summary>
    /// Interaction logic for UIObjectTest.xaml
    /// </summary>
    public partial class UIObjectTest : Window
    {
        public UIObjectTest()
        {
            InitializeComponent();
        }

        private void AddTxb_Click(object sender, RoutedEventArgs e)
        {
            cadControl.AddUIElement
            (
                new TextBox() 
                { 
                    Text = "This is a TextBox",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left 
                }
            );
        }

        private void AddTxbl_Click(object sender, RoutedEventArgs e)
        {
            cadControl.AddUIElement
            (
                new TextBlock()
                {
                    Text = "This is a TextBlock",
                    Foreground = Brushes.White,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            );
        }
    }
}
