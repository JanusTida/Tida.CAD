using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000003 RID: 3
    public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject where T : DependencyObject, IAttachedObject {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000004 RID: 4 RVA: 0x000020D0 File Offset: 0x000002D0
        protected DependencyObject AssociatedObject {
            get {
                base.ReadPreamble();
                return this.associatedObject;
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000020E0 File Offset: 0x000002E0
        internal AttachableCollection() {
            ((INotifyCollectionChanged)this).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
            this.snapshot = new Collection<T>();
        }

        // Token: 0x06000006 RID: 6
        protected abstract void OnAttached();

        // Token: 0x06000007 RID: 7
        protected abstract void OnDetaching();

        // Token: 0x06000008 RID: 8
        internal abstract void ItemAdded(T item);

        // Token: 0x06000009 RID: 9
        internal abstract void ItemRemoved(T item);

        // Token: 0x0600000A RID: 10 RVA: 0x00002114 File Offset: 0x00000314
        [Conditional("DEBUG")]
        private void VerifySnapshotIntegrity() {
            bool flag = base.Count == this.snapshot.Count;
            if (flag) {
                for (int i = 0; i < base.Count; i++) {
                    if (base[i] != this.snapshot[i]) {
                        return;
                    }
                }
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x0000216C File Offset: 0x0000036C
        private void VerifyAdd(T item) {
            if (this.snapshot.Contains(item)) {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.DuplicateItemInCollectionExceptionMessage, new object[]
                {
                    typeof(T).Name,
                    base.GetType().Name
                }));
            }
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000021C4 File Offset: 0x000003C4
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems) {
                        this.ItemAdded(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (T item in e.OldItems) {
                        this.ItemRemoved(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (T item in e.OldItems) {
                        this.ItemRemoved(item);
                    }
                    foreach (T item in e.NewItems) {
                        this.ItemAdded(item);
                    }
                    break;

                case (NotifyCollectionChangedAction.Replace | NotifyCollectionChangedAction.Remove):
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (T item in this) {
                        this.ItemRemoved(item);
                    }

                    foreach (T item in this) {
                        this.ItemAdded(item);
                    }
                    break;

                default:
                    return;
            }
        }

    

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600000D RID: 13 RVA: 0x00002440 File Offset: 0x00000640
        DependencyObject IAttachedObject.AssociatedObject {
            get {
                return this.AssociatedObject;
            }
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002448 File Offset: 0x00000648
        public void Attach(DependencyObject dependencyObject) {
            if (dependencyObject != this.AssociatedObject) {
                if (this.AssociatedObject != null) {
                    throw new InvalidOperationException();
                }
                if (Interaction.ShouldRunInDesignMode || !(bool)base.GetValue(DesignerProperties.IsInDesignModeProperty)) {
                    base.WritePreamble();
                    this.associatedObject = dependencyObject;
                    base.WritePostscript();
                }
                this.OnAttached();
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x0000249E File Offset: 0x0000069E
        public void Detach() {
            this.OnDetaching();
            base.WritePreamble();
            this.associatedObject = null;
            base.WritePostscript();
        }

        // Token: 0x04000001 RID: 1
        private Collection<T> snapshot;

        // Token: 0x04000002 RID: 2
        private DependencyObject associatedObject;
    }
}
