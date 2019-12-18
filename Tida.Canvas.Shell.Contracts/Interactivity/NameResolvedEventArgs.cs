using System;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000014 RID: 20
    internal sealed class NameResolvedEventArgs : EventArgs {
        // Token: 0x17000026 RID: 38
        // (get) Token: 0x06000091 RID: 145 RVA: 0x000039F5 File Offset: 0x00001BF5
        public object OldObject {
            get {
                return this.oldObject;
            }
        }

        // Token: 0x17000027 RID: 39
        // (get) Token: 0x06000092 RID: 146 RVA: 0x000039FD File Offset: 0x00001BFD
        public object NewObject {
            get {
                return this.newObject;
            }
        }

        // Token: 0x06000093 RID: 147 RVA: 0x00003A05 File Offset: 0x00001C05
        public NameResolvedEventArgs(object oldObject, object newObject) {
            this.oldObject = oldObject;
            this.newObject = newObject;
        }

        // Token: 0x0400002A RID: 42
        private object oldObject;

        // Token: 0x0400002B RID: 43
        private object newObject;
    }
}
