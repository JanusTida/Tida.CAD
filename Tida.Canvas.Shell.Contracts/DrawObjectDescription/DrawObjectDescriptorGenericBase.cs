using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.ComponentModel;
using SDrawObjectDescription = Tida.Canvas.Infrastructure.ComponentModel.DrawObjectDescription;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {

    /// <summary>
    /// 绘制对象描述器泛型基类;
    /// </summary>
    public abstract class DrawObjectDescriptorGenericBase<TDrawObject> : IDrawObjectDescriptor where TDrawObject : DrawObject {
        

        public SDrawObjectDescription GetDescription(DrawObject drawObject) {
            if (!(drawObject is TDrawObject tDrawObject)) {
                return null;
            } 

            return new SDrawObjectDescription(TypeName); 
        }
        
        
        protected abstract string TypeName { get; }
    }
}
