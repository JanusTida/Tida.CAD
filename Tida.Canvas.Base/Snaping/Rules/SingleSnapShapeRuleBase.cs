using Tida.Canvas.Contracts;
using System;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Base.Snaping.Rules {
    /// <summary>
    /// 单个绘制对象与关注点的辅助规则基类;
    /// </summary>
    /// <typeparam name="TDrawObject"></typeparam>
    public abstract class SingleSnapShapeRuleBase<TDrawObject> : ISnapShapeRule where TDrawObject:DrawObject {
        public ISnapShape MatchSnapShape(DrawObject[] drawObjects, Vector2D position, ICanvasContext canvasContext) {
            if (drawObjects == null) {
                throw new ArgumentNullException(nameof(drawObjects));
            }

            if (position == null) {
                throw new ArgumentNullException(nameof(position));
            }

            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }


            //循环遍历绘制对象;
            foreach (var drawObject in drawObjects) {
                //类型判断,转化为本类指定的类型;
                if (!(drawObject is TDrawObject tDrawObject)) {
                    continue;
                }

                var snapShape = MatchSnapShape(tDrawObject, position, canvasContext);
                if (snapShape != null) {
                    return snapShape;
                }
            }

            return null;
        }

        /// <summary>
        /// 判断指定类型的实例与关注点的辅助关系;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <param name="position"></param>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        protected abstract ISnapShape MatchSnapShape(TDrawObject drawObject, Vector2D position, ICanvasContext canvasContext);
    }
}
