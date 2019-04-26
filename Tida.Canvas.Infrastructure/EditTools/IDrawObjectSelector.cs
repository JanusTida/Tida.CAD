using Tida.Canvas.Contracts;
using System.Collections.Generic;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 绘制对象选择器;
    /// </summary>
    public interface IDrawObjectSelector {
        /// <summary>
        /// 在一个或多个绘制对象集合中选择一个绘制对象;
        /// </summary>
        /// <param name="drawObjects"></param>
        /// <returns></returns>
        DrawObject SelectOneDrawObject(IEnumerable<DrawObject> drawObjects);
    }
}
