using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Animation;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000004 RID: 4
    public abstract class Behavior : Animatable, IAttachedObject {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000010 RID: 16 RVA: 0x000024BC File Offset: 0x000006BC
        // (remove) Token: 0x06000011 RID: 17 RVA: 0x000024F4 File Offset: 0x000006F4
        internal event EventHandler AssociatedObjectChanged;

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000012 RID: 18 RVA: 0x00002529 File Offset: 0x00000729
        protected Type AssociatedType {
            get {
                base.ReadPreamble();
                return this.associatedType;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000013 RID: 19 RVA: 0x00002537 File Offset: 0x00000737
        protected DependencyObject AssociatedObject {
            get {
                base.ReadPreamble();
                return this.associatedObject;
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002545 File Offset: 0x00000745
        internal Behavior(Type associatedType) {
            this.associatedType = associatedType;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002554 File Offset: 0x00000754
        protected virtual void OnAttached() {
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002556 File Offset: 0x00000756
        protected virtual void OnDetaching() {
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002558 File Offset: 0x00000758
        protected override Freezable CreateInstanceCore() {
            Type type = base.GetType();
            return (Freezable)Activator.CreateInstance(type);
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002577 File Offset: 0x00000777
        private void OnAssociatedObjectChanged() {
            if (this.AssociatedObjectChanged != null) {
                this.AssociatedObjectChanged(this, new EventArgs());
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000019 RID: 25 RVA: 0x00002592 File Offset: 0x00000792
        DependencyObject IAttachedObject.AssociatedObject {
            get {
                return this.AssociatedObject;
            }
        }

        // Token: 0x0600001A RID: 26 RVA: 0x0000259C File Offset: 0x0000079C
        public void Attach(DependencyObject dependencyObject) {
            if (dependencyObject != this.AssociatedObject) {
                if (this.AssociatedObject != null) {
                    throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorMultipleTimesExceptionMessage);
                }
                if (dependencyObject != null && !this.AssociatedType.IsAssignableFrom(dependencyObject.GetType())) {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[]
                    {
                        base.GetType().Name,
                        dependencyObject.GetType().Name,
                        this.AssociatedType.Name
                    }));
                }
                base.WritePreamble();
                this.associatedObject = dependencyObject;
                base.WritePostscript();
                this.OnAssociatedObjectChanged();
                this.OnAttached();
            }
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002644 File Offset: 0x00000844
        public void Detach() {
            this.OnDetaching();
            base.WritePreamble();
            this.associatedObject = null;
            base.WritePostscript();
            this.OnAssociatedObjectChanged();
        }

        // Token: 0x04000003 RID: 3
        private Type associatedType;

        // Token: 0x04000004 RID: 4
        private DependencyObject associatedObject;
    }
}
