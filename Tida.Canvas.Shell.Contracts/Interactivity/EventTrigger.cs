using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200000F RID: 15
    public class EventTrigger : EventTriggerBase<object> {
        // Token: 0x06000065 RID: 101 RVA: 0x00003444 File Offset: 0x00001644
        public EventTrigger() {
        }

        // Token: 0x06000066 RID: 102 RVA: 0x0000344C File Offset: 0x0000164C
        public EventTrigger(string eventName) {
            this.EventName = eventName;
        }

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x06000067 RID: 103 RVA: 0x0000345B File Offset: 0x0000165B
        // (set) Token: 0x06000068 RID: 104 RVA: 0x0000346D File Offset: 0x0000166D
        public string EventName {
            get {
                return (string)base.GetValue(EventTrigger.EventNameProperty);
            }
            set {
                base.SetValue(EventTrigger.EventNameProperty, value);
            }
        }

        // Token: 0x06000069 RID: 105 RVA: 0x0000347B File Offset: 0x0000167B
        protected override string GetEventName() {
            return this.EventName;
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00003483 File Offset: 0x00001683
        private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args) {
            ((EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
        }

        // Token: 0x0400001F RID: 31
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventTrigger), new FrameworkPropertyMetadata("Loaded", new PropertyChangedCallback(EventTrigger.OnEventNameChanged)));
    }
}
