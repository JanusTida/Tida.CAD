using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000016 RID: 22
    public abstract class TargetedTriggerAction : TriggerAction {
        // Token: 0x1700002F RID: 47
        // (get) Token: 0x060000A9 RID: 169 RVA: 0x00003CDE File Offset: 0x00001EDE
        // (set) Token: 0x060000AA RID: 170 RVA: 0x00003CEB File Offset: 0x00001EEB
        public object TargetObject {
            get {
                return base.GetValue(TargetedTriggerAction.TargetObjectProperty);
            }
            set {
                base.SetValue(TargetedTriggerAction.TargetObjectProperty, value);
            }
        }

        // Token: 0x17000030 RID: 48
        // (get) Token: 0x060000AB RID: 171 RVA: 0x00003CF9 File Offset: 0x00001EF9
        // (set) Token: 0x060000AC RID: 172 RVA: 0x00003D0B File Offset: 0x00001F0B
        public string TargetName {
            get {
                return (string)base.GetValue(TargetedTriggerAction.TargetNameProperty);
            }
            set {
                base.SetValue(TargetedTriggerAction.TargetNameProperty, value);
            }
        }

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x060000AD RID: 173 RVA: 0x00003D1C File Offset: 0x00001F1C
        protected object Target {
            get {
                object obj = base.AssociatedObject;
                if (this.TargetObject != null) {
                    obj = this.TargetObject;
                }
                else if (this.IsTargetNameSet) {
                    obj = this.TargetResolver.Object;
                }
                if (obj != null && !this.TargetTypeConstraint.IsAssignableFrom(obj.GetType())) {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage, new object[]
                    {
                        base.GetType().Name,
                        obj.GetType(),
                        this.TargetTypeConstraint,
                        "Target"
                    }));
                }
                return obj;
            }
        }

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x060000AE RID: 174 RVA: 0x00003DB4 File Offset: 0x00001FB4
        protected sealed override Type AssociatedObjectTypeConstraint {
            get {
                AttributeCollection attributes = TypeDescriptor.GetAttributes(base.GetType());
                TypeConstraintAttribute typeConstraintAttribute = attributes[typeof(TypeConstraintAttribute)] as TypeConstraintAttribute;
                if (typeConstraintAttribute != null) {
                    return typeConstraintAttribute.Constraint;
                }
                return typeof(DependencyObject);
            }
        }

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x060000AF RID: 175 RVA: 0x00003DF7 File Offset: 0x00001FF7
        protected Type TargetTypeConstraint {
            get {
                base.ReadPreamble();
                return this.targetTypeConstraint;
            }
        }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x060000B0 RID: 176 RVA: 0x00003E05 File Offset: 0x00002005
        private bool IsTargetNameSet {
            get {
                return !string.IsNullOrEmpty(this.TargetName) || base.ReadLocalValue(TargetedTriggerAction.TargetNameProperty) != DependencyProperty.UnsetValue;
            }
        }

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x060000B1 RID: 177 RVA: 0x00003E2B File Offset: 0x0000202B
        private NameResolver TargetResolver {
            get {
                return this.targetResolver;
            }
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x060000B2 RID: 178 RVA: 0x00003E33 File Offset: 0x00002033
        // (set) Token: 0x060000B3 RID: 179 RVA: 0x00003E3B File Offset: 0x0000203B
        private bool IsTargetChangedRegistered {
            get {
                return this.isTargetChangedRegistered;
            }
            set {
                this.isTargetChangedRegistered = value;
            }
        }

        // Token: 0x060000B4 RID: 180 RVA: 0x00003E44 File Offset: 0x00002044
        internal TargetedTriggerAction(Type targetTypeConstraint) : base(typeof(DependencyObject)) {
            this.targetTypeConstraint = targetTypeConstraint;
            this.targetResolver = new NameResolver();
            this.RegisterTargetChanged();
        }

        // Token: 0x060000B5 RID: 181 RVA: 0x00003E6E File Offset: 0x0000206E
        internal virtual void OnTargetChangedImpl(object oldTarget, object newTarget) {
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x00003E70 File Offset: 0x00002070
        protected override void OnAttached() {
            base.OnAttached();
            DependencyObject associatedObject = base.AssociatedObject;
            Behavior behavior = associatedObject as Behavior;
            this.RegisterTargetChanged();
            if (behavior != null) {
                associatedObject = ((IAttachedObject)behavior).AssociatedObject;
                behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
            }
            this.TargetResolver.NameScopeReferenceElement = (associatedObject as FrameworkElement);
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x00003EC4 File Offset: 0x000020C4
        protected override void OnDetaching() {
            Behavior behavior = base.AssociatedObject as Behavior;
            base.OnDetaching();
            this.OnTargetChangedImpl(this.TargetResolver.Object, null);
            this.UnregisterTargetChanged();
            if (behavior != null) {
                behavior.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
            }
            this.TargetResolver.NameScopeReferenceElement = null;
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x00003F1C File Offset: 0x0000211C
        private void OnBehaviorHostChanged(object sender, EventArgs e) {
            this.TargetResolver.NameScopeReferenceElement = (((IAttachedObject)sender).AssociatedObject as FrameworkElement);
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x00003F39 File Offset: 0x00002139
        private void RegisterTargetChanged() {
            if (!this.IsTargetChangedRegistered) {
                this.TargetResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
                this.IsTargetChangedRegistered = true;
            }
        }

        // Token: 0x060000BA RID: 186 RVA: 0x00003F61 File Offset: 0x00002161
        private void UnregisterTargetChanged() {
            if (this.IsTargetChangedRegistered) {
                this.TargetResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
                this.IsTargetChangedRegistered = false;
            }
        }

        // Token: 0x060000BB RID: 187 RVA: 0x00003F8C File Offset: 0x0000218C
        private static void OnTargetObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
            targetedTriggerAction.OnTargetChanged(obj, new NameResolvedEventArgs(args.OldValue, args.NewValue));
        }

        // Token: 0x060000BC RID: 188 RVA: 0x00003FBC File Offset: 0x000021BC
        private static void OnTargetNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
            targetedTriggerAction.TargetResolver.Name = (string)args.NewValue;
        }

        // Token: 0x060000BD RID: 189 RVA: 0x00003FE7 File Offset: 0x000021E7
        private void OnTargetChanged(object sender, NameResolvedEventArgs e) {
            if (base.AssociatedObject != null) {
                this.OnTargetChangedImpl(e.OldObject, e.NewObject);
            }
        }

        // Token: 0x04000032 RID: 50
        private Type targetTypeConstraint;

        // Token: 0x04000033 RID: 51
        private bool isTargetChangedRegistered;

        // Token: 0x04000034 RID: 52
        private NameResolver targetResolver;

        // Token: 0x04000035 RID: 53
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetObjectChanged)));

        // Token: 0x04000036 RID: 54
        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetNameChanged)));
    }
}
