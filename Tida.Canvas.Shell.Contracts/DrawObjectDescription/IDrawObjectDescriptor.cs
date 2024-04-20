using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {
    /// <summary>
    /// 绘制对象描述器;
    /// </summary>
    public interface IDrawObjectDescriptor {
        /// <summary>
        /// 获取特定绘制对象的类别名;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        DrawObjectDescription GetDescription(DrawObject drawObject);
    }

}
