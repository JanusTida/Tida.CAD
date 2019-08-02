using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Shell.Views {
    [Export]
    public partial class Shell : Window {
        public Shell() {
            InitializeComponent();
        }
        
        //public Grid Grid => grid;
    }
}
