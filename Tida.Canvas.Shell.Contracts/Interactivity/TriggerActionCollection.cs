using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000018 RID: 24
    public class TriggerActionCollection : AttachableCollection<TriggerAction> {
        // Token: 0x060000C3 RID: 195 RVA: 0x000040C0 File Offset: 0x000022C0
        internal TriggerActionCollection() {
        }

        // Token: 0x060000C4 RID: 196 RVA: 0x000040C8 File Offset: 0x000022C8
        protected override void OnAttached() {
            foreach (TriggerAction current in this) {
                current.Attach(base.AssociatedObject);
            }
        }

        // Token: 0x060000C5 RID: 197 RVA: 0x0000411C File Offset: 0x0000231C
        protected override void OnDetaching() {
            foreach (TriggerAction current in this) {
                current.Detach();
            }
        }

        // Token: 0x060000C6 RID: 198 RVA: 0x0000416C File Offset: 0x0000236C
        internal override void ItemAdded(TriggerAction item) {
            if (item.IsHosted) {
                throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
            }
            if (base.AssociatedObject != null) {
                item.Attach(base.AssociatedObject);
            }
            item.IsHosted = true;
        }

        // Token: 0x060000C7 RID: 199 RVA: 0x0000419C File Offset: 0x0000239C
        internal override void ItemRemoved(TriggerAction item) {
            if (((IAttachedObject)item).AssociatedObject != null) {
                item.Detach();
            }
            item.IsHosted = false;
        }

        // Token: 0x060000C8 RID: 200 RVA: 0x000041B3 File Offset: 0x000023B3
        protected override Freezable CreateInstanceCore() {
            return new TriggerActionCollection();
        }
    }
}
