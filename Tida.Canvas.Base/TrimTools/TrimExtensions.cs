using CDO.Common.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDO.Common.Canvas.Shell.TrimTools {
    /// <summary>
    /// 裁剪的相关拓展;
    /// </summary>
    public static class TrimExtensions {
        /// <summary>
        /// 根据交点以及与裁剪区域的交点得到裁剪后的线段集合;
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="intersectPositions">裁剪区域与线段的交点集合</param>
        /// <param name="rectIntersectPosition">关注的交点集合</param>
        /// <returns></returns>
        public static Line2D[] GetTrimedLineWithIntersectPositions(Line2D line2D, Vector2D[] intersectPositions, Vector2D[] rectIntersectPositions) {
            //若筛选后为空,则不能裁剪;
            if (rectIntersectPositions.Length == 0) {
                return null;
            }
            if (intersectPositions.Length == 0) {
                return null;
            }

            ///沿线段起始到终止方向
            ///检查所有的线段与裁剪区域的交点是否均在<param name="intersectPositions">相邻两点之中</param>
            ///适用与需要将原线段裁切为两条线段的情况,即"-----/-----";

            for (int i = 0; i < intersectPositions.Length - 1; i++) {
                var thisMinPosition = intersectPositions[i];
                var thisMaxPosition = intersectPositions[i + 1];

                var thisMinDistance = thisMinPosition.Distance(line2D.Start);
                var thisMaxDistance = thisMaxPosition.Distance(line2D.Start);

                var isIncluded = rectIntersectPositions.All(p => {
                    var thisDistance = p.Distance(line2D.Start);
                    return thisDistance > thisMinDistance && thisDistance < thisMaxDistance;
                });

                //若包含在之中,则将原线段一分为二,返回分离后的线段;
                if (isIncluded) {
                    return new Line2D[] {
                        new Line2D(new Vector2D(line2D.Start),new Vector2D(thisMinPosition)),
                        new Line2D(new Vector2D(thisMaxPosition),new Vector2D(line2D.End))
                    };
                }
            }

            ///若所有的线段与裁剪区域的交点是否均在<param name="intersectPositions">一边;
            ///适用于裁剪原线段一段指定距离的情况,及"口----"或"----口";

            var minDistance = intersectPositions[0].Distance(line2D.Start);
            var maxDistance = intersectPositions[intersectPositions.Length - 1].Distance(line2D.Start);

            if (rectIntersectPositions.All(p => p.Distance(line2D.Start) < minDistance)) {
                return new Line2D[] { new Line2D(intersectPositions[0], new Vector2D(line2D.End)) };
            }
            else if (rectIntersectPositions.All(p => p.Distance(line2D.Start) > maxDistance)) {
                return new Line2D[] { new Line2D(new Vector2D(line2D.Start), intersectPositions[intersectPositions.Length - 1]) };
            }

            return null;
        }
    }
}
