using System;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000010 RID: 16
    public static class Interaction {
        // Token: 0x1700001B RID: 27
        // (get) Token: 0x0600006C RID: 108 RVA: 0x000034E3 File Offset: 0x000016E3
        // (set) Token: 0x0600006D RID: 109 RVA: 0x000034EA File Offset: 0x000016EA
        internal static bool ShouldRunInDesignMode {
            get;
            set;
        }

        // Token: 0x0600006E RID: 110 RVA: 0x000034F4 File Offset: 0x000016F4
        public static TriggerCollection GetTriggers(DependencyObject obj) {
            TriggerCollection triggerCollection = (TriggerCollection)obj.GetValue(Interaction.TriggersProperty);
            if (triggerCollection == null) {
                triggerCollection = new TriggerCollection();
                obj.SetValue(Interaction.TriggersProperty, triggerCollection);
            }
            return triggerCollection;
        }

        // Token: 0x0600006F RID: 111 RVA: 0x00003528 File Offset: 0x00001728
        public static BehaviorCollection GetBehaviors(DependencyObject obj) {
            BehaviorCollection behaviorCollection = (BehaviorCollection)obj.GetValue(Interaction.BehaviorsProperty);
            if (behaviorCollection == null) {
                behaviorCollection = new BehaviorCollection();
                obj.SetValue(Interaction.BehaviorsProperty, behaviorCollection);
            }
            return behaviorCollection;
        }

        // Token: 0x06000070 RID: 112 RVA: 0x0000355C File Offset: 0x0000175C
        private static void OnBehaviorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            BehaviorCollection behaviorCollection = (BehaviorCollection)args.OldValue;
            BehaviorCollection behaviorCollection2 = (BehaviorCollection)args.NewValue;
            if (behaviorCollection != behaviorCollection2) {
                if (behaviorCollection != null && ((IAttachedObject)behaviorCollection).AssociatedObject != null) {
                    behaviorCollection.Detach();
                }
                if (behaviorCollection2 != null && obj != null) {
                    if (((IAttachedObject)behaviorCollection2).AssociatedObject != null) {
                        throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
                    }
                    behaviorCollection2.Attach(obj);
                }
            }
        }

        // Token: 0x06000071 RID: 113 RVA: 0x000035B8 File Offset: 0x000017B8
        private static void OnTriggersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) {
            TriggerCollection triggerCollection = args.OldValue as TriggerCollection;
            TriggerCollection triggerCollection2 = args.NewValue as TriggerCollection;
            if (triggerCollection != triggerCollection2) {
                if (triggerCollection != null && ((IAttachedObject)triggerCollection).AssociatedObject != null) {
                    triggerCollection.Detach();
                }
                if (triggerCollection2 != null && obj != null) {
                    if (((IAttachedObject)triggerCollection2).AssociatedObject != null) {
                        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
                    }
                    triggerCollection2.Attach(obj);
                }
            }
        }

        // Token: 0x06000072 RID: 114 RVA: 0x00003614 File Offset: 0x00001814
        internal static bool IsElementLoaded(FrameworkElement element) {
            return element.IsLoaded;
        }

        // Token: 0x04000020 RID: 32
        private static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("ShadowTriggers", typeof(TriggerCollection), typeof(Interaction), new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnTriggersChanged)));

        // Token: 0x04000021 RID: 33
        private static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("ShadowBehaviors", typeof(BehaviorCollection), typeof(Interaction), new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));
    }
}
