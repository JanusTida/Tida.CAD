using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    public class CommandBehaviorBase<T> where T : UIElement {
        // Token: 0x06000340 RID: 832 RVA: 0x00008868 File Offset: 0x00006A68
        public CommandBehaviorBase(T targetObject) {
            this._targetObject = new WeakReference(targetObject);
            this._commandCanExecuteChangedHandler = new EventHandler(this.CommandCanExecuteChanged);
        }

        // Token: 0x170000BE RID: 190
        // (get) Token: 0x06000341 RID: 833 RVA: 0x0000889A File Offset: 0x00006A9A
        // (set) Token: 0x06000342 RID: 834 RVA: 0x000088A2 File Offset: 0x00006AA2
        public bool AutoEnable {
            get {
                return this._autoEnabled;
            }
            set {
                this._autoEnabled = value;
                this.UpdateEnabledState();
            }
        }

        // Token: 0x170000BF RID: 191
        // (get) Token: 0x06000343 RID: 835 RVA: 0x000088B1 File Offset: 0x00006AB1
        // (set) Token: 0x06000344 RID: 836 RVA: 0x000088BC File Offset: 0x00006ABC
        public ICommand Command {
            get {
                return this._command;
            }
            set {
                if (this._command != null) {
                    this._command.CanExecuteChanged -= this._commandCanExecuteChangedHandler;
                }
                this._command = value;
                if (this._command != null) {
                    this._command.CanExecuteChanged += this._commandCanExecuteChangedHandler;
                    this.UpdateEnabledState();
                }
            }
        }

        // Token: 0x170000C0 RID: 192
        // (get) Token: 0x06000345 RID: 837 RVA: 0x00008908 File Offset: 0x00006B08
        // (set) Token: 0x06000346 RID: 838 RVA: 0x00008910 File Offset: 0x00006B10
        public object CommandParameter {
            get {
                return this._commandParameter;
            }
            set {
                if (this._commandParameter != value) {
                    this._commandParameter = value;
                    this.UpdateEnabledState();
                }
            }
        }

        // Token: 0x170000C1 RID: 193
        // (get) Token: 0x06000347 RID: 839 RVA: 0x00008928 File Offset: 0x00006B28
        protected T TargetObject {
            get {
                return this._targetObject.Target as T;
            }
        }

        // Token: 0x06000348 RID: 840 RVA: 0x00008940 File Offset: 0x00006B40
        protected virtual void UpdateEnabledState() {
            if (this.TargetObject == null) {
                this.Command = null;
                this.CommandParameter = null;
                return;
            }
            if (this.Command != null && this.AutoEnable) {
                this.TargetObject.IsEnabled = this.Command.CanExecute(this.CommandParameter);
            }
        }

        // Token: 0x06000349 RID: 841 RVA: 0x0000899A File Offset: 0x00006B9A
        private void CommandCanExecuteChanged(object sender, EventArgs e) {
            this.UpdateEnabledState();
        }

        // Token: 0x0600034A RID: 842 RVA: 0x000089A2 File Offset: 0x00006BA2
        protected virtual void ExecuteCommand(object parameter) {
            if (this.Command != null) {
                this.Command.Execute(this.CommandParameter ?? parameter);
            }
        }

        // Token: 0x04000098 RID: 152
        private ICommand _command;

        // Token: 0x04000099 RID: 153
        private object _commandParameter;

        // Token: 0x0400009A RID: 154
        private readonly WeakReference _targetObject;

        // Token: 0x0400009B RID: 155
        private readonly EventHandler _commandCanExecuteChangedHandler;

        // Token: 0x0400009C RID: 156
        private bool _autoEnabled = true;
    }
}
