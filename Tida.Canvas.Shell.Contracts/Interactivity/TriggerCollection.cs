using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200001B RID: 27
    public sealed class TriggerCollection : AttachableCollection<TriggerBase> {
        // Token: 0x060000CF RID: 207 RVA: 0x000041FA File Offset: 0x000023FA
        internal TriggerCollection() {
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00004204 File Offset: 0x00002404
        protected override void OnAttached() {
            foreach (TriggerBase current in this) {
                current.Attach(base.AssociatedObject);
            }
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x00004258 File Offset: 0x00002458
        protected override void OnDetaching() {
            foreach (TriggerBase current in this) {
                current.Detach();
            }
        }

        // Token: 0x060000D2 RID: 210 RVA: 0x000042A8 File Offset: 0x000024A8
        internal override void ItemAdded(TriggerBase item) {
            if (base.AssociatedObject != null) {
                item.Attach(base.AssociatedObject);
            }
        }

        // Token: 0x060000D3 RID: 211 RVA: 0x000042BE File Offset: 0x000024BE
        internal override void ItemRemoved(TriggerBase item) {
            if (((IAttachedObject)item).AssociatedObject != null) {
                item.Detach();
            }
        }

        // Token: 0x060000D4 RID: 212 RVA: 0x000042CE File Offset: 0x000024CE
        protected override Freezable CreateInstanceCore() {
            return new TriggerCollection();
        }
    }
}
