using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000019 RID: 25
    public abstract class TriggerBase<T> : TriggerBase where T : DependencyObject {
        // Token: 0x060000C9 RID: 201 RVA: 0x000041BA File Offset: 0x000023BA
        protected TriggerBase() : base(typeof(T)) {
        }

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x060000CA RID: 202 RVA: 0x000041CC File Offset: 0x000023CC
        protected new T AssociatedObject {
            get {
                return (T)((object)base.AssociatedObject);
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x060000CB RID: 203 RVA: 0x000041D9 File Offset: 0x000023D9
        protected sealed override Type AssociatedObjectTypeConstraint {
            get {
                return base.AssociatedObjectTypeConstraint;
            }
        }
    }
}
