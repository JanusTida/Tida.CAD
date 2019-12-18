using System;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200001A RID: 26
    public class PreviewInvokeEventArgs : EventArgs {
        // Token: 0x1700003A RID: 58
        // (get) Token: 0x060000CC RID: 204 RVA: 0x000041E1 File Offset: 0x000023E1
        // (set) Token: 0x060000CD RID: 205 RVA: 0x000041E9 File Offset: 0x000023E9
        public bool Cancelling {
            get;
            set;
        }
    }
}
