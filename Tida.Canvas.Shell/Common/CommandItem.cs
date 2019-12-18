using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Common {
    /// <summary>
    /// 命令绑定项,可用于MenuItem等的绑定等;
    /// </summary>
    class CommandItem : ExtensibleBindableBase, ICommandItem, ICustomNotify {
        public CommandItem(ICommand command, string guid, Func<bool> isVisible = null) {
            this.Command = command;
            this.GUID = guid;
            this._isVisibleFunc = isVisible;
        }

        public ICommand Command { get; }
        private Func<bool> _isVisibleFunc;
        public bool IsVisible => _isVisibleFunc?.Invoke() ?? true;

        private string _commandName;
        public virtual string Name {
            get => _commandName;
            set => SetProperty(ref _commandName, value);
        }

        private Uri _icon;
        public Uri Icon {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public IEnumerable<ICommandItem> Children {
            get => _children;
            set {
                if (value is ObservableCollection<ICommandItem> obCommandItems) {
                    _children = obCommandItems;
                }
            }
        }

        private ObservableCollection<ICommandItem> _children { get; set; } = new ObservableCollection<ICommandItem>();


        private bool _isEnabled;
        public bool IsEnabled {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        //排列顺序;
        public int Sort { get; set; }

        public string GUID { get; }
        public bool IsChecked { get; set; }

        public void AddChild(ICommandItem commandItem) => _children.AddOrderBy(commandItem, p => p.Sort);

        public void RemoveChild(ICommandItem commandItem) => _children.Remove(commandItem);

        public void NotifyProperty(string propName) {
            RaisePropertyChanged(nameof(IsVisible));
        }
    }

    [Export(typeof(ICommandItemFactory))]
    class CommandItemFactory : ICommandItemFactory {
        public ICommandItem CreateNew(ICommand command, string guid, Func<bool> isVisible = null) => new CommandItem(command, guid, isVisible);
    }

    
}
