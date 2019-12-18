using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000012 RID: 18
    public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject {
        // Token: 0x06000084 RID: 132 RVA: 0x00003831 File Offset: 0x00001A31
        protected TriggerAction() : base(typeof(T)) {
        }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x06000085 RID: 133 RVA: 0x00003843 File Offset: 0x00001A43
        protected new T AssociatedObject {
            get {
                return (T)((object)base.AssociatedObject);
            }
        }

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x06000086 RID: 134 RVA: 0x00003850 File Offset: 0x00001A50
        protected sealed override Type AssociatedObjectTypeConstraint {
            get {
                return base.AssociatedObjectTypeConstraint;
            }
        }
    }
}
