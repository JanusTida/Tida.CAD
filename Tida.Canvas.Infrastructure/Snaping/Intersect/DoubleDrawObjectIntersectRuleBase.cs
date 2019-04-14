using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.Snaping.Intersect {
    /// <summary>
    /// 双绘制对象相交判断泛型基类;
    /// </summary>
    /// <typeparam name="TDrawObject0"></typeparam>
    /// <typeparam name="TDrawObject1"></typeparam>
    public abstract class DoubleDrawObjectIntersectRuleBase<TDrawObject0, TDrawObject1> : IDrawObjectIntersectRule
        where TDrawObject0 : DrawObject where TDrawObject1 : DrawObject {
        public Vector2D[] GetIntersectPositions(DrawObject drawObject0, DrawObject drawObject1, bool extendDrawObject0 = false) {
            if (drawObject0 == null || drawObject1 == null) {
                return null;
            }

            if (drawObject0 is TDrawObject0 tDrawObject0 && drawObject1 is TDrawObject1 tDrawObject1) {
                return GetIntersectPositions(tDrawObject0, tDrawObject1, extendDrawObject0);
            }
            else if (drawObject0 is TDrawObject1 tDrawObject1_1 && drawObject1 is TDrawObject0 tDrawObject0_1) {
                return GetIntersectPositions(tDrawObject0_1, tDrawObject1_1, extendDrawObject0);
            }

            return null;
        }

        protected abstract Vector2D[] GetIntersectPositions(TDrawObject0 drawObject0, TDrawObject1 drawObject1, bool extendDrawObject0);
    }
}
