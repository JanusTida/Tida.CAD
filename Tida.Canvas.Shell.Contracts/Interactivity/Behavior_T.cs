using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000005 RID: 5
    public abstract class Behavior<T> : Behavior where T : DependencyObject {
        // Token: 0x0600001C RID: 28 RVA: 0x00002665 File Offset: 0x00000865
        protected Behavior() : base(typeof(T)) {
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600001D RID: 29 RVA: 0x00002677 File Offset: 0x00000877
        protected new T AssociatedObject {
            get {
                return (T)((object)base.AssociatedObject);
            }
        }
    }
}
