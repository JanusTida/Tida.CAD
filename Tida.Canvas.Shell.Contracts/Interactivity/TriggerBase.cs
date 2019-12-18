using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200000C RID: 12
    [ContentProperty("Actions")]
    public abstract class TriggerBase : Animatable, IAttachedObject {
        // Token: 0x06000030 RID: 48 RVA: 0x000029D4 File Offset: 0x00000BD4
        internal TriggerBase(Type associatedObjectTypeConstraint) {
            this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
            TriggerActionCollection value = new TriggerActionCollection();
            base.SetValue(TriggerBase.ActionsPropertyKey, value);
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000031 RID: 49 RVA: 0x00002A00 File Offset: 0x00000C00
        protected DependencyObject AssociatedObject {
            get {
                base.ReadPreamble();
                return this.associatedObject;
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000032 RID: 50 RVA: 0x00002A0E File Offset: 0x00000C0E
        protected virtual Type AssociatedObjectTypeConstraint {
            get {
                base.ReadPreamble();
                return this.associatedObjectTypeConstraint;
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000033 RID: 51 RVA: 0x00002A1C File Offset: 0x00000C1C
        public TriggerActionCollection Actions {
            get {
                return (TriggerActionCollection)base.GetValue(TriggerBase.ActionsProperty);
            }
        }

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000034 RID: 52 RVA: 0x00002A30 File Offset: 0x00000C30
        // (remove) Token: 0x06000035 RID: 53 RVA: 0x00002A68 File Offset: 0x00000C68
        public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

        // Token: 0x06000036 RID: 54 RVA: 0x00002AA0 File Offset: 0x00000CA0
        protected void InvokeActions(object parameter) {
            if (this.PreviewInvoke != null) {
                PreviewInvokeEventArgs previewInvokeEventArgs = new PreviewInvokeEventArgs();
                this.PreviewInvoke(this, previewInvokeEventArgs);
                if (previewInvokeEventArgs.Cancelling) {
                    return;
                }
            }
            foreach (TriggerAction current in this.Actions) {
                current.CallInvoke(parameter);
            }
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00002B18 File Offset: 0x00000D18
        protected virtual void OnAttached() {
        }

        // Token: 0x06000038 RID: 56 RVA: 0x00002B1A File Offset: 0x00000D1A
        protected virtual void OnDetaching() {
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00002B1C File Offset: 0x00000D1C
        protected override Freezable CreateInstanceCore() {
            Type type = base.GetType();
            return (Freezable)Activator.CreateInstance(type);
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x0600003A RID: 58 RVA: 0x00002B3B File Offset: 0x00000D3B
        DependencyObject IAttachedObject.AssociatedObject {
            get {
                return this.AssociatedObject;
            }
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00002B44 File Offset: 0x00000D44
        public void Attach(DependencyObject dependencyObject) {
            if (dependencyObject != this.AssociatedObject) {
                if (this.AssociatedObject != null) {
                    throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerMultipleTimesExceptionMessage);
                }
                if (dependencyObject != null && !this.AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType())) {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[]
                    {
                        base.GetType().Name,
                        dependencyObject.GetType().Name,
                        this.AssociatedObjectTypeConstraint.Name
                    }));
                }
                base.WritePreamble();
                this.associatedObject = dependencyObject;
                base.WritePostscript();
                this.Actions.Attach(dependencyObject);
                this.OnAttached();
            }
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00002BF2 File Offset: 0x00000DF2
        public void Detach() {
            this.OnDetaching();
            base.WritePreamble();
            this.associatedObject = null;
            base.WritePostscript();
            this.Actions.Detach();
        }

        // Token: 0x04000013 RID: 19
        private DependencyObject associatedObject;

        // Token: 0x04000014 RID: 20
        private Type associatedObjectTypeConstraint;

        // Token: 0x04000015 RID: 21
        private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly("Actions", typeof(TriggerActionCollection), typeof(TriggerBase), new FrameworkPropertyMetadata());

        // Token: 0x04000016 RID: 22
        public static readonly DependencyProperty ActionsProperty = TriggerBase.ActionsPropertyKey.DependencyProperty;
    }
}
