using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Application.Contracts.Controls;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.StatusBar {
    /// <summary>
    /// 状态栏-复选框的一个默认实现;
    /// </summary>
    public abstract class StatusBarCheckBoxItem:StatusBarItemBase {
        public StatusBarCheckBoxItem(string guid):base(guid) {
            _checkBox.Checked += CheckBox_Checked;
            _checkBox.Unchecked += CheckBox_Unchecked;
            _checkBox.Margin = new Thickness(24, 0, 0, 0);
            _checkBox.VerticalAlignment = VerticalAlignment.Center;
            _checkBox.Foreground = Constants.StatusBarItemForeground_Default;
            
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            OnIsCheckedChanged();
        }

        public bool? IsChecked {
            get {
                return _checkBox.IsChecked;
            }
            set {
                if(_checkBox.IsChecked == value) {
                    return;
                }
                _checkBox.IsChecked = value;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {
            OnIsCheckedChanged();
        }

        /// <summary>
        /// 是否可空;
        /// </summary>
        public bool IsThreeState {
            get {
                return _checkBox.IsThreeState;
            }
            set {
                _checkBox.IsThreeState = value;
            }
        }

        protected virtual void OnIsCheckedChanged() {

        }
        

        private readonly CheckBox _checkBox = new CheckBox();
        
        public Brush Foreground {
            get { return _checkBox.Foreground; }
            set{
                _checkBox.Foreground = value;
            }
        }
        
        public override sealed object UIObject => _checkBox;

        public Thickness Margin {
            get {
                return _checkBox.Margin;
            }

            set {
                _checkBox.Margin = value;
            }
        }

        public object Content {
            get {
                return _checkBox.Content;
            }

            set {
                _checkBox.Content = value;
            }
        }
    }
}
