using Tida.Canvas.Shell.Contracts.StatusBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Tida.Canvas.Shell.Contracts.Controls;

namespace Tida.Canvas.Shell.Contracts.StatusBar {
    /// <summary>
    /// 状态栏文字项的一个默认实现;
    /// </summary>
    public abstract class StatusBarTextItem : StatusBarItemBase {
        public StatusBarTextItem(string guid):base(guid) {
            _textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            _textBlock.Foreground = Constants.StatusBarItemForeground_Default;
            _textBlock.Margin = new Thickness(24, 0, 0, 0);
        }

        private readonly TextBlock _textBlock = new TextBlock();
        public string Text {
            get { return _textBlock.Text; }
            set { _textBlock.Text = value; }
        }

        public Brush Foreground {
            get { return _textBlock.Foreground; }
            set { _textBlock.Foreground = value; }
        }
        
        public override object UIObject => _textBlock;

        public Thickness Margin {
            get { return _textBlock.Margin; }
            set { _textBlock.Margin = value; }
        }
    }
}
