using System;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200001C RID: 28
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TypeConstraintAttribute : Attribute {
        // Token: 0x1700003B RID: 59
        // (get) Token: 0x060000D5 RID: 213 RVA: 0x000042D5 File Offset: 0x000024D5
        // (set) Token: 0x060000D6 RID: 214 RVA: 0x000042DD File Offset: 0x000024DD
        public Type Constraint {
            get;
            private set;
        }

        // Token: 0x060000D7 RID: 215 RVA: 0x000042E6 File Offset: 0x000024E6
        public TypeConstraintAttribute(Type constraint) {
            this.Constraint = constraint;
        }
    }
}
