using System;
using System.Reflection;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200000B RID: 11
    public sealed class EventObserver : IDisposable {
        // Token: 0x0600002E RID: 46 RVA: 0x0000295C File Offset: 0x00000B5C
        public EventObserver(EventInfo eventInfo, object target, Delegate handler) {
            if (eventInfo == null) {
                throw new ArgumentNullException("eventInfo");
            }
            if (handler == null) {
                throw new ArgumentNullException("handler");
            }
            this.eventInfo = eventInfo;
            this.target = target;
            this.handler = handler;
            this.eventInfo.AddEventHandler(this.target, handler);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000029B8 File Offset: 0x00000BB8
        public void Dispose() {
            this.eventInfo.RemoveEventHandler(this.target, this.handler);
        }

        // Token: 0x04000010 RID: 16
        private EventInfo eventInfo;

        // Token: 0x04000011 RID: 17
        private object target;

        // Token: 0x04000012 RID: 18
        private Delegate handler;
    }
}
