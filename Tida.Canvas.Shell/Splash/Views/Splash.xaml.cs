using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Splash.Views {
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    [Export]
    public partial class Splash : Window {
        public Splash() {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }

        //public string LoadingText {
        //    get {
        //        return loadingText.Text;
        //    }
        //    set {
        //        loadingText.Text = value;
        //    }
        //}
    }
}
