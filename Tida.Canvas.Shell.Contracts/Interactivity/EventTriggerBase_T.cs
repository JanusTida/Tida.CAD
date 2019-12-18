using System;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200000E RID: 14
    public abstract class EventTriggerBase<T> : EventTriggerBase where T : class {
        // Token: 0x06000061 RID: 97 RVA: 0x000033FD File Offset: 0x000015FD
        protected EventTriggerBase() : base(typeof(T)) {
        }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x06000062 RID: 98 RVA: 0x0000340F File Offset: 0x0000160F
        public new T Source {
            get {
                return (T)((object)base.Source);
            }
        }

        // Token: 0x06000063 RID: 99 RVA: 0x0000341C File Offset: 0x0000161C
        internal sealed override void OnSourceChangedImpl(object oldSource, object newSource) {
            base.OnSourceChangedImpl(oldSource, newSource);
            this.OnSourceChanged(oldSource as T, newSource as T);
        }

        // Token: 0x06000064 RID: 100 RVA: 0x00003442 File Offset: 0x00001642
        protected virtual void OnSourceChanged(T oldSource, T newSource) {
        }
    }
}
