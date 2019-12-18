using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Docking {
    /// <summary>
    /// 停靠区域基类;
    /// </summary>
    public abstract class DockingPaneBase : IDockingPane {

        public event EventHandler HeaderChanged;
        public event EventHandler IsHiddenChanged;
        public event EventHandler PaneHeaderVisibilityChanged;
        
        private string _header;
        public virtual string Header {
            get => _header;
            set {
                if (_header == value) {
                    return;
                }
                _header = value;
                HeaderChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        private Visibility _paneHeaderVisibility;
        public Visibility PaneHeaderVisibility {
            get => _paneHeaderVisibility;
            set {
                if(_paneHeaderVisibility == value) {
                    return;
                }

                _paneHeaderVisibility = value;
                PaneHeaderVisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _isHidden = false;
        public bool IsHidden {
            get => _isHidden;
            set {
                if (_isHidden == value) {
                    return;
                }
                _isHidden = value;
                IsHiddenChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public abstract object UIObject { get; }
    }
}
