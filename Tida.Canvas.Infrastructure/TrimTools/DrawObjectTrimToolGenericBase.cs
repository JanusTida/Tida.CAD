using Tida.Canvas.Contracts;

namespace Tida.Canvas.Infrastructure.TrimTools {
    /// <summary>
    /// 绘制对象裁剪工具泛型基类;
    /// </summary>
    public abstract class DrawObjectTrimToolBase<TDrawObject> : IDrawObjectTrimTool where TDrawObject : DrawObject {
        public bool CheckIsValidDrawObject(DrawObject drawObject) {
            return drawObject is TDrawObject;
        }

     
        public DrawObject[] TrimDrawObject(DrawObjectTrimingInfo trimingInfo) {
            if (trimingInfo.TrimedDrawObject is TDrawObject tDrawObject) {
                return TrimDrawObject(tDrawObject, trimingInfo);
            }

            return null;
        }

        protected abstract DrawObject[] TrimDrawObject(TDrawObject drawObject, DrawObjectTrimingInfo trimingInfo);
    }
}
