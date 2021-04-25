using System.ComponentModel.Composition;
using System.Windows.Controls;
using Tida.Canvas.Shell.CommandOutput.IViews;

namespace Tida.Canvas.Shell.CommandOutput.Views {
    /// <summary>
    /// Interaction logic for CommandOutput.xaml
    /// </summary>
    [Export,Export(typeof(ICommandOutput))]
    public partial class CommandOutput : UserControl,ICommandOutput {
        public CommandOutput() {
            InitializeComponent();
        }

        public void ScrollToEnd() => _txb.ScrollToEnd();
    }
}
