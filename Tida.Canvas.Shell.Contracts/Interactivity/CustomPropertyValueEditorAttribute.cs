using System;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x02000008 RID: 8
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CustomPropertyValueEditorAttribute : Attribute {
        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000024 RID: 36 RVA: 0x0000275D File Offset: 0x0000095D
        // (set) Token: 0x06000025 RID: 37 RVA: 0x00002765 File Offset: 0x00000965
        public CustomPropertyValueEditor CustomPropertyValueEditor {
            get;
            private set;
        }

        // Token: 0x06000026 RID: 38 RVA: 0x0000276E File Offset: 0x0000096E
        public CustomPropertyValueEditorAttribute(CustomPropertyValueEditor customPropertyValueEditor) {
            this.CustomPropertyValueEditor = customPropertyValueEditor;
        }
    }
}
