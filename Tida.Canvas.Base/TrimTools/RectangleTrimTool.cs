using Tida.Canvas.Infrastructure.TrimTools;
using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Contracts;


namespace Tida.Canvas.Base.TrimTools {
    /// <summary>
    /// 矩形的裁剪工具;
    /// </summary>
    //[Export(typeof(IDrawObjectTrimTool))]
    class RectangleTrimTool : DrawObjectTrimToolBase<Rectangle> {
        protected override DrawObject[] TrimDrawObject(Rectangle drawObject, DrawObjectTrimingInfo drawObjectTrimingInfo) {
            

            return null;
        }
    }
}
