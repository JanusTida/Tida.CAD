using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000006 RID: 6
    public sealed class BehaviorCollection : AttachableCollection<Behavior> {
        // Token: 0x0600001E RID: 30 RVA: 0x00002684 File Offset: 0x00000884
        internal BehaviorCollection() {
        }

        // Token: 0x0600001F RID: 31 RVA: 0x0000268C File Offset: 0x0000088C
        protected override void OnAttached() {
            foreach (Behavior current in this) {
                current.Attach(base.AssociatedObject);
            }
        }

        // Token: 0x06000020 RID: 32 RVA: 0x000026E0 File Offset: 0x000008E0
        protected override void OnDetaching() {
            foreach (Behavior current in this) {
                current.Detach();
            }
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002730 File Offset: 0x00000930
        internal override void ItemAdded(Behavior item) {
            if (base.AssociatedObject != null) {
                item.Attach(base.AssociatedObject);
            }
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00002746 File Offset: 0x00000946
        internal override void ItemRemoved(Behavior item) {
            if (((IAttachedObject)item).AssociatedObject != null) {
                item.Detach();
            }
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00002756 File Offset: 0x00000956
        protected override Freezable CreateInstanceCore() {
            return new BehaviorCollection();
        }
    }
}
