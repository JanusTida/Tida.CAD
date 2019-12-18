using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.TreeView;

namespace Tida.Canvas.Shell.TreeView {
    public class TreeUnit : ExtensibleBindableBase, ITreeUnit, IInternalNode<ITreeUnit> {
        public TreeUnit(string typeGuid) {
            this.TypeGuid = typeGuid;
            _children = new ObservableCollectionEx<ITreeUnit, TreeUnit>(this);
        }

        //类型GUID;
        public string TypeGuid { get; }

        public ITreeUnit InternalParent { get; set; }
        public ITreeUnit Parent => InternalParent;

        //节点文字;
        public string Label { get; set; }


        //节点级别;
        public int Level {
            get {
                var level = 0;
                var node = Parent;
                while (node != null) {
                    node = node.Parent;
                    level++;
                }
                return level;
            }
        }

        //子节点;
        private ObservableCollectionEx<ITreeUnit, TreeUnit> _children;
        public ICollection<ITreeUnit> Children {
            get => _children;
            set {
                if (value is ObservableCollectionEx<ITreeUnit, TreeUnit> cCollection) {
                    _children = cCollection;
                }
            }
        }

        //节点图标;
        private Uri _icon;
        public Uri Icon {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private bool _isExpanded;
        public bool IsExpanded {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

    }

    interface IInternalNode<TNode> {
        TNode InternalParent { get; set; }
    }

    /// <summary>
    /// 在进行添加时会自行使新添加的项指向所有者的集合,移除则会置空;
    /// </summary>
    /// <typeparam name="TInternalNode"></typeparam>
    class ObservableCollectionEx<TNode, TChildNode> :
        ObservableCollection<TNode> where TChildNode : IInternalNode<TNode> {
        public ObservableCollectionEx(TNode owner) {
            this._owner = owner;
        }

        private TNode _owner;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            base.OnCollectionChanged(e);
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null) {
                        foreach (var item in e.NewItems) {
                            if (item is TChildNode node) {
                                node.InternalParent = _owner;
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.NewItems != null) {
                        foreach (var item in e.NewItems) {
                            if (item is TChildNode node) {
                                node.InternalParent = default(TNode);
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

    }
}
