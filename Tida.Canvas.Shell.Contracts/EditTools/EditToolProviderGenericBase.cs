using Tida.Canvas.Contracts;
using System;

namespace Tida.Canvas.Shell.Contracts.EditTools {
    /// <summary>
    /// 编辑工具提供者泛型基类;
    /// </summary>
    /// <typeparam name="TEditTool"></typeparam>
    public abstract class EditToolProviderGenericBase<TEditTool> : IEditToolProvider where TEditTool : EditTool {
        public event EventHandler CanCreateChanged;

        public virtual bool CanCreate => true;

        protected void RaiseCanCreateChanged() {
            CanCreateChanged?.Invoke(this, EventArgs.Empty);
        }

        public EditTool CreateEditTool() {
            return OnCreateEditTool();
        }

        public bool ValidateFromThis(EditTool editTool) {
            if(editTool == null) {
                return false;
            }
            return editTool.GetType() == typeof(TEditTool);
        }

        protected abstract TEditTool OnCreateEditTool();
    }
}
