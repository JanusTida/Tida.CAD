using System;
using System.Collections;
using System.Globalization;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000009 RID: 9
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true), CLSCompliant(false)]
    public sealed class DefaultTriggerAttribute : Attribute {
        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000027 RID: 39 RVA: 0x0000277D File Offset: 0x0000097D
        public Type TargetType {
            get {
                return this.targetType;
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000028 RID: 40 RVA: 0x00002785 File Offset: 0x00000985
        public Type TriggerType {
            get {
                return this.triggerType;
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000029 RID: 41 RVA: 0x0000278D File Offset: 0x0000098D
        public IEnumerable Parameters {
            get {
                return this.parameters;
            }
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002798 File Offset: 0x00000998
        public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter) : this(targetType, triggerType, new object[]
        {
            parameter
        }) {
        }

        // Token: 0x0600002B RID: 43 RVA: 0x000027BC File Offset: 0x000009BC
        public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters) {
            if (!typeof(TriggerBase).IsAssignableFrom(triggerType)) {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage, new object[]
                {
                    triggerType.Name
                }));
            }
            this.targetType = targetType;
            this.triggerType = triggerType;
            this.parameters = parameters;
        }

        // Token: 0x0600002C RID: 44 RVA: 0x0000281C File Offset: 0x00000A1C
        public TriggerBase Instantiate() {
            object obj = null;
            try {
                obj = Activator.CreateInstance(this.TriggerType, this.parameters);
            }
            catch {
            }
            return (TriggerBase)obj;
        }

        // Token: 0x0400000D RID: 13
        private Type targetType;

        // Token: 0x0400000E RID: 14
        private Type triggerType;

        // Token: 0x0400000F RID: 15
        private object[] parameters;
    }
}
