using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.CommandOutput.Views {
    /// <summary>
    /// Interaction logic for CommandOutput.xaml
    /// </summary>
    [Export]
    public partial class CommandOutput : UserControl {
        public CommandOutput() {
            InitializeComponent();
        }
    }
}
