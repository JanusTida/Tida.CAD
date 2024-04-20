using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using System.Linq;
using Telerik.Windows.Controls.Docking;

namespace Tida.Canvas.Shell.MainPage.Views {
    /// <summary>
    /// Interaction logic for FileSystem.xaml
    /// </summary>
    [Export]
    public partial class MainPage : UserControl{
        public MainPage() {
            InitializeComponent();
        }

        public RadPaneGroup DocumentPaneGroup => documentPaneGroup;

        public RadDocking RadDocking => docking;
        
    }
}
