using Tida.Canvas.Infrastructure.Contracts;
using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.Snaping {
    /// <summary>
    /// 两绘制对象相交的规则;
    /// </summary>
    public interface IDrawObjectIntersectRule {
        /// <summary>
        /// 获取两个绘制对象的交点所在的位置;
        /// </summary>
        /// <param name="drawObject0"></param>
        /// <param name="drawObject1"></param>
        /// <param name="extendDrawObject0">是否对<paramref name="drawObject0"/>绘制对象进行延伸,比如对线段进行延伸</param>
        /// <returns></returns>
        Vector2D[] GetIntersectPositions(DrawObject drawObject0, DrawObject drawObject1,bool extendDrawObject0 = false);
    }

    /// <summary>
    /// 两绘制对象相交的规则元数据;
    /// </summary>
    public interface IDrawObjectIntersectRuleMetaData:IHaveOrder {

    }

}
