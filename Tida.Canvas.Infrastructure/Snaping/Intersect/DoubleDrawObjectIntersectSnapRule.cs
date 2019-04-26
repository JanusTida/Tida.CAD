using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Infrastructure.Snaping.Shapes;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Geometry.External.Util;
using Tida.Geometry.Primitives;
using System;
using System.Linq;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Snaping.Intersect {

    /// <summary>
    /// 双对象相交辅助规则,本类不可被导出,由<see cref="DoubleDrawObjectIntersectRuleProvider"/>动态构成;
    /// </summary>
    public class DoubleDrawObjectIntersectSnapRule : ISnapShapeRule {
        public DoubleDrawObjectIntersectSnapRule(IDrawObjectIntersectRule drawObjectIntersectRule) {
            this.DrawObjectIntersectRule = drawObjectIntersectRule ?? throw new ArgumentNullException(nameof(drawObjectIntersectRule));
        }
        public IDrawObjectIntersectRule DrawObjectIntersectRule { get; }
        public ISnapShape MatchSnapShape(DrawObject[] drawObjects, Vector2D position, ICanvasContext canvasContext) {
            if (drawObjects == null) {
                return null;
            }

#if DEBUG
            //position = new Vector2D(0, 0);
#endif

            var screenPosition = canvasContext.CanvasProxy.ToScreen(position);
            var rect = Rectangle2D2.CreateEmpty();

            //双游标两两遍历;
            for (int i = 0; i < drawObjects.Length - 1; i++) {
                for (int j = i + 1; j < drawObjects.Length; j++) {
                    var intersectPositions = DrawObjectIntersectRule.GetIntersectPositions(drawObjects[i], drawObjects[j]);
                    if (intersectPositions == null) {
                        continue;
                    }

                    foreach (var intersectPosition in intersectPositions) {
                        NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                            canvasContext.CanvasProxy.ToScreen(intersectPosition),
                            TolerantedScreenLength,
                            TolerantedScreenLength,
                            rect
                        );


                        if (rect.Contains(screenPosition)) {
                            return new IntersectSnapPoint(intersectPosition);
                        }

                    }
                }

            }

            return null;
        }
        
    }
}
