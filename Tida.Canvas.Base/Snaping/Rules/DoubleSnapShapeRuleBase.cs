using Tida.Canvas.Contracts;
using System;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 双绘制对象辅助规则泛型基类;
    /// </summary>
    public abstract class DoubleSnapShapeRuleBase<TDrawObject0, TDrawObject1> : ISnapShapeRule
        where TDrawObject0 : DrawObject
        where TDrawObject1 : DrawObject {

        public ISnapShape MatchSnapShape(
            DrawObject[] drawObjects, 
            Vector2D position,
            ICanvasContext canvasContext
        ) {

            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            //双游标两两遍历;
            for (int i = 0; i < drawObjects.Length - 1; i++) {
                for (int j = i + 1; j < drawObjects.Length; j++) {
                    if (drawObjects[i] is TDrawObject0 tDrawObject0 && drawObjects[j] is TDrawObject1 tDrawObject1) {
                        return MatchSnapShape(tDrawObject0, tDrawObject1,position,canvasContext);
                    }
                    else if (drawObjects[i] is TDrawObject1 tDrawObject1_1 && drawObjects[j] is TDrawObject0 tDrawObject0_1) {
                        return MatchSnapShape(tDrawObject0_1, tDrawObject1_1, position, canvasContext);
                    }
                }
            }
            
            return null;
        }

        protected abstract ISnapShape MatchSnapShape(
            TDrawObject0 drawObject0,
            TDrawObject1 drawObject1,
            Vector2D position,
            ICanvasContext canvasContext
        );
    }
}
