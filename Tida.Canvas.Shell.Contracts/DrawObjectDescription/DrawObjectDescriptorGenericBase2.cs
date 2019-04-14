using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.ComponentModel;
using SDrawObjectDescription = Tida.Canvas.Infrastructure.ComponentModel.DrawObjectDescription;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {

    /// <summary>
    /// 绘制对象描述器泛型基类2;
    /// </summary>
    public abstract class DrawObjectDescriptorGenericBase2<TDrawObject> : IDrawObjectDescriptor where TDrawObject : DrawObject {
        public SDrawObjectDescription GetDescription(DrawObject drawObject) {
            if (!(drawObject is TDrawObject tDrawObject)) {
                return null;
            }

            if (!CheckIsValidDrawObject(tDrawObject)) {
                return null;
            }

            return new SDrawObjectDescription(GetTypeName(tDrawObject));
        }

        /// <summary>
        /// 判断特定的绘制对象实例是否可用;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        protected abstract bool CheckIsValidDrawObject(TDrawObject drawObject);

        protected abstract string GetTypeName(TDrawObject drawObject);
    }
}
