using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000015 RID: 21
    internal sealed class NameResolver {
        // Token: 0x14000003 RID: 3
        // (add) Token: 0x06000094 RID: 148 RVA: 0x00003A1C File Offset: 0x00001C1C
        // (remove) Token: 0x06000095 RID: 149 RVA: 0x00003A54 File Offset: 0x00001C54
        public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x06000096 RID: 150 RVA: 0x00003A89 File Offset: 0x00001C89
        // (set) Token: 0x06000097 RID: 151 RVA: 0x00003A94 File Offset: 0x00001C94
        public string Name {
            get {
                return this.name;
            }
            set {
                DependencyObject @object = this.Object;
                this.name = value;
                this.UpdateObjectFromName(@object);
            }
        }

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x06000098 RID: 152 RVA: 0x00003AB6 File Offset: 0x00001CB6
        public DependencyObject Object {
            get {
                if (string.IsNullOrEmpty(this.Name) && this.HasAttempedResolve) {
                    return this.NameScopeReferenceElement;
                }
                return this.ResolvedObject;
            }
        }

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x06000099 RID: 153 RVA: 0x00003ADA File Offset: 0x00001CDA
        // (set) Token: 0x0600009A RID: 154 RVA: 0x00003AE4 File Offset: 0x00001CE4
        public FrameworkElement NameScopeReferenceElement {
            get {
                return this.nameScopeReferenceElement;
            }
            set {
                FrameworkElement oldNameScopeReference = this.NameScopeReferenceElement;
                this.nameScopeReferenceElement = value;
                this.OnNameScopeReferenceElementChanged(oldNameScopeReference);
            }
        }

        // Token: 0x1700002B RID: 43
        // (get) Token: 0x0600009B RID: 155 RVA: 0x00003B06 File Offset: 0x00001D06
        private FrameworkElement ActualNameScopeReferenceElement {
            get {
                if (this.NameScopeReferenceElement == null || !Interaction.IsElementLoaded(this.NameScopeReferenceElement)) {
                    return null;
                }
                return this.GetActualNameScopeReference(this.NameScopeReferenceElement);
            }
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x0600009C RID: 156 RVA: 0x00003B2B File Offset: 0x00001D2B
        // (set) Token: 0x0600009D RID: 157 RVA: 0x00003B33 File Offset: 0x00001D33
        private DependencyObject ResolvedObject {
            get;
            set;
        }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x0600009E RID: 158 RVA: 0x00003B3C File Offset: 0x00001D3C
        // (set) Token: 0x0600009F RID: 159 RVA: 0x00003B44 File Offset: 0x00001D44
        private bool PendingReferenceElementLoad {
            get;
            set;
        }

        // Token: 0x1700002E RID: 46
        // (get) Token: 0x060000A0 RID: 160 RVA: 0x00003B4D File Offset: 0x00001D4D
        // (set) Token: 0x060000A1 RID: 161 RVA: 0x00003B55 File Offset: 0x00001D55
        private bool HasAttempedResolve {
            get;
            set;
        }

        // Token: 0x060000A2 RID: 162 RVA: 0x00003B5E File Offset: 0x00001D5E
        private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference) {
            if (this.PendingReferenceElementLoad) {
                oldNameScopeReference.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
                this.PendingReferenceElementLoad = false;
            }
            this.HasAttempedResolve = false;
            this.UpdateObjectFromName(this.Object);
        }

        // Token: 0x060000A3 RID: 163 RVA: 0x00003B94 File Offset: 0x00001D94
        private void UpdateObjectFromName(DependencyObject oldObject) {
            DependencyObject resolvedObject = null;
            this.ResolvedObject = null;
            if (this.NameScopeReferenceElement != null) {
                if (!Interaction.IsElementLoaded(this.NameScopeReferenceElement)) {
                    this.NameScopeReferenceElement.Loaded += new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
                    this.PendingReferenceElementLoad = true;
                    return;
                }
                if (!string.IsNullOrEmpty(this.Name)) {
                    FrameworkElement actualNameScopeReferenceElement = this.ActualNameScopeReferenceElement;
                    if (actualNameScopeReferenceElement != null) {
                        resolvedObject = (actualNameScopeReferenceElement.FindName(this.Name) as DependencyObject);
                    }
                }
            }
            this.HasAttempedResolve = true;
            this.ResolvedObject = resolvedObject;
            if (oldObject != this.Object) {
                this.OnObjectChanged(oldObject, this.Object);
            }
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x00003C2D File Offset: 0x00001E2D
        private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget) {
            if (this.ResolvedElementChanged != null) {
                this.ResolvedElementChanged(this, new NameResolvedEventArgs(oldTarget, newTarget));
            }
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x00003C4C File Offset: 0x00001E4C
        private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement) {
            FrameworkElement frameworkElement = initialReferenceElement;
            if (this.IsNameScope(initialReferenceElement)) {
                frameworkElement = ((initialReferenceElement.Parent as FrameworkElement) ?? frameworkElement);
            }
            return frameworkElement;
        }

        // Token: 0x060000A6 RID: 166 RVA: 0x00003C78 File Offset: 0x00001E78
        private bool IsNameScope(FrameworkElement frameworkElement) {
            FrameworkElement frameworkElement2 = frameworkElement.Parent as FrameworkElement;
            if (frameworkElement2 != null) {
                object obj = frameworkElement2.FindName(this.Name);
                return obj != null;
            }
            return false;
        }

        // Token: 0x060000A7 RID: 167 RVA: 0x00003CAA File Offset: 0x00001EAA
        private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e) {
            this.PendingReferenceElementLoad = false;
            this.NameScopeReferenceElement.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
            this.UpdateObjectFromName(this.Object);
        }

        // Token: 0x0400002C RID: 44
        private string name;

        // Token: 0x0400002D RID: 45
        private FrameworkElement nameScopeReferenceElement;
    }
}
