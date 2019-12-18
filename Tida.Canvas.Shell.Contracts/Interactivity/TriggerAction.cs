using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000011 RID: 17
    [DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown"), DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
    public abstract class TriggerAction : Animatable, IAttachedObject {
        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000074 RID: 116 RVA: 0x00003691 File Offset: 0x00001891
        // (set) Token: 0x06000075 RID: 117 RVA: 0x000036A3 File Offset: 0x000018A3
        public bool IsEnabled {
            get {
                return (bool)base.GetValue(TriggerAction.IsEnabledProperty);
            }
            set {
                base.SetValue(TriggerAction.IsEnabledProperty, value);
            }
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x06000076 RID: 118 RVA: 0x000036B6 File Offset: 0x000018B6
        protected DependencyObject AssociatedObject {
            get {
                base.ReadPreamble();
                return this.associatedObject;
            }
        }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x06000077 RID: 119 RVA: 0x000036C4 File Offset: 0x000018C4
        protected virtual Type AssociatedObjectTypeConstraint {
            get {
                base.ReadPreamble();
                return this.associatedObjectTypeConstraint;
            }
        }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x06000078 RID: 120 RVA: 0x000036D2 File Offset: 0x000018D2
        // (set) Token: 0x06000079 RID: 121 RVA: 0x000036E0 File Offset: 0x000018E0
        internal bool IsHosted {
            get {
                base.ReadPreamble();
                return this.isHosted;
            }
            set {
                base.WritePreamble();
                this.isHosted = value;
                base.WritePostscript();
            }
        }

        // Token: 0x0600007A RID: 122 RVA: 0x000036F5 File Offset: 0x000018F5
        internal TriggerAction(Type associatedObjectTypeConstraint) {
            this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
        }

        // Token: 0x0600007B RID: 123 RVA: 0x00003704 File Offset: 0x00001904
        internal void CallInvoke(object parameter) {
            if (this.IsEnabled) {
                this.Invoke(parameter);
            }
        }

        // Token: 0x0600007C RID: 124
        protected abstract void Invoke(object parameter);

        // Token: 0x0600007D RID: 125 RVA: 0x00003715 File Offset: 0x00001915
        protected virtual void OnAttached() {
        }

        // Token: 0x0600007E RID: 126 RVA: 0x00003717 File Offset: 0x00001917
        protected virtual void OnDetaching() {
        }

        // Token: 0x0600007F RID: 127 RVA: 0x0000371C File Offset: 0x0000191C
        protected override Freezable CreateInstanceCore() {
            Type type = base.GetType();
            return (Freezable)Activator.CreateInstance(type);
        }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x06000080 RID: 128 RVA: 0x0000373B File Offset: 0x0000193B
        DependencyObject IAttachedObject.AssociatedObject {
            get {
                return this.AssociatedObject;
            }
        }

        // Token: 0x06000081 RID: 129 RVA: 0x00003744 File Offset: 0x00001944
        public void Attach(DependencyObject dependencyObject) {
            if (dependencyObject != this.AssociatedObject) {
                if (this.AssociatedObject != null) {
                    throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
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
                this.OnAttached();
            }
        }

        // Token: 0x06000082 RID: 130 RVA: 0x000037E6 File Offset: 0x000019E6
        public void Detach() {
            this.OnDetaching();
            base.WritePreamble();
            this.associatedObject = null;
            base.WritePostscript();
        }

        // Token: 0x04000023 RID: 35
        private bool isHosted;

        // Token: 0x04000024 RID: 36
        private DependencyObject associatedObject;

        // Token: 0x04000025 RID: 37
        private Type associatedObjectTypeConstraint;

        // Token: 0x04000026 RID: 38
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(TriggerAction), new FrameworkPropertyMetadata(true));
    }
}
