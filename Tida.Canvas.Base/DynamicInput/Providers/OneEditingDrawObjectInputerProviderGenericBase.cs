using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Contracts;
using System.Linq;

namespace Tida.Canvas.Base.DynamicInput.Providers {
    /// <summary>
    /// 某个特定类型的编辑对象在被编辑时所用的动态输入处理器;
    /// </summary>
    /// <typeparam name="TDrawObject"></typeparam>
    public abstract class OneEditingDrawObjectInputerProviderGenericBase<TDrawObject> : ICanvasControlDynamicInputerProvider where TDrawObject : DrawObject {
        /// <summary>
        /// 当且仅当画布中的特定类型的一个且只有一个编辑对象在被编辑时,
        /// 本方法才可能返回特定的动态输入处理器;
        /// 返回的动态输入处理器具体类型,将在<see cref="OnCreateInputer(TDrawObject, ICanvasControl)"/>中实现;
        /// </summary>
        /// <param name="canvasControl"></param>
        /// <returns></returns>
        public IDynamicInputer CreateInputer(ICanvasControl canvasControl) {
            //编辑工具需为空;
            if (canvasControl.CurrentEditTool != null) {
                return null;
            }

            //获取所有正在被编辑的绘制对象;
            var allEditingDrawObjects = canvasControl.GetAllVisibleDrawObjects().
                Where(p => p.IsSelected && p.IsEditing).
                Select(p => p as TDrawObject).Where(p => p != null);

            //当且仅当存在一个时,才能返回;
            if (allEditingDrawObjects.Count() != 1) {
                return null;
            }

            var editingDrawObject = allEditingDrawObjects.First();

            return OnCreateInputer(editingDrawObject, canvasControl);
        }

        protected abstract IDynamicInputer OnCreateInputer(TDrawObject drawObject, ICanvasControl canvasControl);
    }
}
