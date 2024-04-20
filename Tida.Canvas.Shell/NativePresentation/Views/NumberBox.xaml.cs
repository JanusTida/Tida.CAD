using Tida.Canvas.Shell.NativePresentation.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tida.Canvas.Shell.NativePresentation.Views {
    /// <summary>
    /// Interaction logic for NumberBox.xaml
    /// </summary>
    public partial class NumberBox : ContentControl {
        public NumberBox() {
            InitializeComponent();
            this.DataContextChanged += NumberBox_DataContextChanged;
            this.Loaded += NumberBox_Loaded;

            txb.LostFocus += Txb_LostFocus;
            txb.GotFocus += Txb_GotFocus;
        }

        private void Txb_GotFocus(object sender, RoutedEventArgs e) {
            if(this.DataContext is NumberBoxModel model) {
                model.IsFocused = true;
            }
        }

        private void Txb_LostFocus(object sender, RoutedEventArgs e) {
            if(this.DataContext is NumberBoxModel model) {
                model.IsFocused = false;
            }
        }

        private void NumberBox_Loaded(object sender, RoutedEventArgs e) {
            if (_askForFocused) {
                txb.Focus();
                txb.SelectAll();
            }
            
        }

        private bool _askForFocused;

        private void NumberBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if(e.NewValue is NumberBoxModel numberBox) {
                SetupDataContext(numberBox);
            }
            if(e.OldValue is NumberBoxModel oldNumberBox) {
                UnSetupDataContext(oldNumberBox);
            }
        }

        private void SetupDataContext(NumberBoxModel model) {
            model.FocusRequest += Model_FocusRequest;
            model.SelectRequest += Model_SelectRequest;
            model.CaretIndexChanged += Model_CaretIndexChanged;
            model.PreviewTextInput += Model_PreviewTextInput;
        }

        private void UnSetupDataContext(NumberBoxModel model) {
            model.FocusRequest -= Model_FocusRequest;
            model.SelectRequest -= Model_SelectRequest;
            model.CaretIndexChanged -= Model_CaretIndexChanged;
            model.PreviewTextInput -= Model_PreviewTextInput;
        }

        private void Model_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            txb.RaiseEvent(e);
        }

        private void Model_CaretIndexChanged(object sender, int e) {
            txb.CaretIndex = e;
        }

        private void Model_SelectRequest(object sender, (int selectionStart, int selectionLength) e) {
            txb.SelectionStart = e.selectionStart;
            txb.SelectionLength = e.selectionLength;
        }

        private void Model_FocusRequest(object sender, EventArgs e) {
            txb.Focus();
            txb.SelectAll();
            _askForFocused = true;
        }

#if DEBUG
        ~NumberBox() {

        }
#endif
    }


}
