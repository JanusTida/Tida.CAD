using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.TrimTools;


namespace Tida.Canvas.Infrastructure.TrimTools {
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
