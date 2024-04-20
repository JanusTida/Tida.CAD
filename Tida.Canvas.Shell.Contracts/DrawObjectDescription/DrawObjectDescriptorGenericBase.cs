using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {

    /// <summary>
    /// 绘制对象描述器泛型基类;
    /// </summary>
    public abstract class DrawObjectDescriptorGenericBase<TDrawObject> : IDrawObjectDescriptor where TDrawObject : DrawObject {
        

        public DrawObjectDescription GetDescription(DrawObject drawObject) {
            if (!(drawObject is TDrawObject tDrawObject)) {
                return null;
            } 

            return new DrawObjectDescription(TypeName); 
        }
        
        
        protected abstract string TypeName { get; }
    }
}
