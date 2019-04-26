using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Infrastructure.TrimTools;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using System.Linq;

namespace Tida.Canvas.Infrastructure.TrimTools {
    /// <summary>
    /// 线段的裁剪工具;
    /// </summary>
    public class LineTrimTool : DrawObjectTrimToolBase<Line> {
        protected override DrawObject[] TrimDrawObject(Line line, DrawObjectTrimingInfo trimingInfo) {
            Vector2D[] intersectPositions = trimingInfo.IntersectPositions;
            Rectangle2D2 trimArea = trimingInfo.TrimArea;

            if (line == null) {
                return null;
            }

            if(intersectPositions == null) {
                return null;
            }

            if(trimArea == null) {
                return null;
            }

            //将交点集合筛选(需在线段上,不包括端点),并沿线段起始到终止方向进行排序;
            intersectPositions = intersectPositions.
                Where(p => p.IsInLine(line.Line2D)).
                OrderBy(p => p.Distance(line.Line2D.Start)).
                ToArray();
            
            ///得到矩形与线段的所有交点(最多两个)，通过该交点集合的与<param name="intersectPositions"/>的关系，进行裁剪;
            var rectInserserctPositions = trimArea.GetLines().
                Select(p => p.Intersect(line.Line2D)).
                Where(p => p != null).
                Distinct(Vector2DEqualityComparer.StaticInstance).
                ToArray();

            return GetTrimedLineWithIntersectPositions(line, intersectPositions, rectInserserctPositions);
        }

        /// <summary>
        /// 根据交点以及与裁剪区域的交点得到裁剪后的线段集合;
        /// </summary>
        /// <param name="line"></param>
        /// <param name="intersectPositions">裁剪区域与线段的交点集合</param>
        /// <param name="rectIntersectPosition">关注的交点集合</param>
        /// <returns></returns>
        private static Line[] GetTrimedLineWithIntersectPositions(Line line,Vector2D[] intersectPositions,Vector2D[] rectIntersectPositions) {
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

                var thisMinDistance = thisMinPosition.Distance(line.Line2D.Start);
                var thisMaxDistance = thisMaxPosition.Distance(line.Line2D.Start);

                var isIncluded = rectIntersectPositions.All(p => {
                    var thisDistance = p.Distance(line.Line2D.Start);
                    return thisDistance > thisMinDistance && thisDistance < thisMaxDistance;
                });

                //若包含在之中,则将原线段一分为二,返回分离后的线段;
                if (isIncluded) {
                    return new Line[] {
                        new Line(new Vector2D(line.Line2D.Start),new Vector2D(thisMinPosition)),
                        new Line(new Vector2D(thisMaxPosition),new Vector2D(line.Line2D.End))
                    };
                }
            }

            ///若所有的线段与裁剪区域的交点是否均在<param name="intersectPositions">一边;
            ///适用于裁剪原线段一段指定距离的情况,及"口----"或"----口";

            var minDistance = intersectPositions[0].Distance(line.Line2D.Start);
            var maxDistance = intersectPositions[intersectPositions.Length - 1].Distance(line.Line2D.Start);
            
            if(rectIntersectPositions.All(p => p.Distance(line.Line2D.Start) < minDistance)) {
                return new Line[] { new Line(intersectPositions[0], new Vector2D(line.Line2D.End)) };
            }
            else if(rectIntersectPositions.All(p => p.Distance(line.Line2D.Start) > maxDistance)) {
                return new Line[] { new Line(new Vector2D(line.Line2D.Start),intersectPositions[intersectPositions.Length - 1]) };
            }

            return null;
        }
    }
}
