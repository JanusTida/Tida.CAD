using Tida.Canvas.Contracts;

namespace Tida.Canvas.Infrastructure.ExtendTools {
    /// <summary>
    /// 绘制对象延伸工具泛型基类;
    /// </summary>
    public abstract class DrawObjectExtendToolGenericBase<TDrawObject> : IDrawObjectExtendTool  where TDrawObject:DrawObject{
        public bool CheckIsValidDrawObject(DrawObject drawObject) {
            return drawObject is TDrawObject;   
        }

        public DrawObject ExtendDrawObject(DrawObjectExtendInfo extendInfo) {
            if (extendInfo.ExtendedDrawObject is TDrawObject tDrawObject) {
                return ExtendDrawObject(tDrawObject, extendInfo);
            }

            return null;
        }

        protected abstract DrawObject ExtendDrawObject(TDrawObject drawObject, DrawObjectExtendInfo objectExtendInfo);
    }
}
