using Tida.Canvas.Contracts;
using System;

namespace Tida.Canvas.Infrastructure.DynamicInput {
    /// <summary>
    /// 针对特定类型的编辑工具的动态输入处理器提供者泛型基类;
    /// </summary>
    public abstract class EditToolDynamicInputerProviderGenericBase<TEditTool> :
        ICanvasControlDynamicInputerProvider where TEditTool : EditTool {

        public IDynamicInputer CreateInputer(ICanvasControl canvasControl) {
            if (canvasControl == null) {
                throw new ArgumentNullException(nameof(canvasControl));
            }

            if(canvasControl.CurrentEditTool == null) {
                return null;
            }

            if (canvasControl.CurrentEditTool.GetType() != typeof(TEditTool)) {
                return null;
            }

            
            return OnCreateInputer(canvasControl,(TEditTool)canvasControl.CurrentEditTool);
        }

        protected abstract IDynamicInputer OnCreateInputer(ICanvasControl canvasControl,TEditTool editTool);
    }

}
