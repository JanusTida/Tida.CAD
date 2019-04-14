using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Geometry.External;

namespace Tida.Canvas.Infrastructure.TrimTools
{
    /// <summary>
    /// 裁剪的相关拓展;
    /// </summary>
    public static class TrimExtensions
    {
        /// <summary>
        /// 根据交点以及与裁剪区域的交点得到裁剪后的线段集合;
        /// </summary>
        /// <param name="line2D">被剪切线</param>
        /// <param name="intersectPositions">裁剪区域与线段的交点集合</param>
        /// <param name="rectIntersectPosition">关注的交点集合</param>
        /// <returns></returns>
        public static Line2D[] GetTrimedLineWithIntersectPositions(Line2D line2D, Vector2D[] intersectPositions, Vector2D[] rectIntersectPositions)
        {
            //若筛选后为空,则不能裁剪;
            if (rectIntersectPositions.Length == 0)
            {
                return null;
            }
            if (intersectPositions.Length == 0)
            {
                return null;
            }

            ///沿线段起始到终止方向
            ///检查所有的线段与裁剪区域的交点是否均在<param name="intersectPositions">相邻两点之中</param>
            ///适用与需要将原线段裁切为两条线段的情况,即"-----/-----";

            for (int i = 0; i < intersectPositions.Length - 1; i++)
            {
                var thisMinPosition = intersectPositions[i];
                var thisMaxPosition = intersectPositions[i + 1];

                var thisMinDistance = thisMinPosition.Distance(line2D.Start);
                var thisMaxDistance = thisMaxPosition.Distance(line2D.Start);

                var isIncluded = rectIntersectPositions.All(p =>
                {
                    var thisDistance = p.Distance(line2D.Start);
                    return thisDistance > thisMinDistance && thisDistance < thisMaxDistance;
                });

                //若包含在之中,则将原线段一分为二,返回分离后的线段;
                if (isIncluded)
                {
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

            if (rectIntersectPositions.All(p => p.Distance(line2D.Start) < minDistance))
            {
                return new Line2D[] { new Line2D(intersectPositions[0], new Vector2D(line2D.End)) };
            }
            else if (rectIntersectPositions.All(p => p.Distance(line2D.Start) > maxDistance))
            {
                return new Line2D[] { new Line2D(new Vector2D(line2D.Start), intersectPositions[intersectPositions.Length - 1]) };
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="intersectPositions"></param>
        /// <param name="rectIntersectPositions"></param>
        /// <returns></returns>
        public static Line2D[] GetTrimedMemberWithIntersectPositions(Line2D line2D, Line2D[] trimLines, Vector2D[] intersectPositions, Vector2D[] rectIntersectPositions, double width, double gap)
        {
            //若筛选后为空,则不能裁剪;
            if (rectIntersectPositions.Length == 0)
            {
                return null;
            }
            if (intersectPositions.Length == 0)
            {
                return null;
            }

            if (trimLines == null || trimLines.Length == 0)
            {
                return null;
            }

            ///沿线段起始到终止方向
            ///检查所有的线段与裁剪区域的交点是否均在<param name="intersectPositions">相邻两点之中</param>
            ///适用与需要将原线段裁切为两条线段的情况,即"-----/-----";
            for (int i = 0; i < intersectPositions.Length - 1; i++)
            {
                var thisMinPosition = intersectPositions[i];
                var thisMaxPosition = intersectPositions[i + 1];

                var thisMinDistance = thisMinPosition.Distance(line2D.Start);
                var thisMaxDistance = thisMaxPosition.Distance(line2D.Start);

                var isIncluded = rectIntersectPositions.All(p =>
                {
                    var thisDistance = p.Distance(line2D.Start);
                    return thisDistance > thisMinDistance && thisDistance < thisMaxDistance;
                });

                //若包含在之中,则将原线段一分为二,返回分离后的线段;
                if (isIncluded)
                {
                    var line1 = new Line2D(new Vector2D(line2D.Start), new Vector2D(thisMinPosition));
                    var trimLine1 = trimLines.Where(x => thisMinPosition.IsInLine(x)).FirstOrDefault();
                    var angle1 = trimLine1.Direction.AngleWith(line1.Direction);
                    var newEndPoint1 = GetExtendPoint(thisMinPosition, line1.Direction, angle1, width, gap);

                    var line2 = new Line2D(new Vector2D(thisMaxPosition), new Vector2D(line2D.End));
                    var trimLine2 = trimLines.Where(x => thisMaxPosition.IsInLine(x)).FirstOrDefault();
                    var angle2 = trimLine2.Direction.AngleWith(line2.Direction);
                    var newEndPoint2 = GetExtendPoint(thisMaxPosition, -line2.Direction, angle2, width, gap);

                    return new Line2D[] {
                        new Line2D(new Vector2D(line2D.Start),new Vector2D(newEndPoint1)),
                        new Line2D(new Vector2D(newEndPoint2),new Vector2D(line2D.End))
                    };
                }
            }

            ///若所有的线段与裁剪区域的交点是否均在<param name="intersectPositions">一边;
            ///适用于裁剪原线段一段指定距离的情况,及"口----"或"----口";

            var minDistance = intersectPositions[0].Distance(line2D.Start);
            var maxDistance = intersectPositions[intersectPositions.Length - 1].Distance(line2D.Start);
            var trimLine = trimLines.OrderBy(x => x.Distance(rectIntersectPositions[0])).FirstOrDefault();
            if (rectIntersectPositions.All(p => p.Distance(line2D.Start) < minDistance))
            {
                var line = new Line2D(intersectPositions[0], new Vector2D(line2D.End));
                var angle = trimLine.Direction.AngleWith(line.Direction);
                var newEndPoint = GetExtendPoint(intersectPositions[0], -line.Direction, angle, width, gap);
                return new Line2D[] { new Line2D(newEndPoint, new Vector2D(line2D.End)) };
            }
            else if (rectIntersectPositions.All(p => p.Distance(line2D.Start) > maxDistance))
            {
                var line = new Line2D(new Vector2D(line2D.Start), intersectPositions[intersectPositions.Length - 1]);
                var angle = trimLine.Direction.AngleWith(line.Direction);
                var newEndPoint = GetExtendPoint(intersectPositions[intersectPositions.Length - 1], line.Direction, angle, width, gap);
                return new Line2D[] { new Line2D(new Vector2D(line2D.Start), newEndPoint) };
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intersectPoint">杆件的相交点</param>
        /// <param name="dir">被剪切杆件的方向,以相交点为终点的方向</param>
        /// <param name="angle">剪切杆件与被剪切杆件的夹角(锐角)</param>
        /// <param name="width">杆件的宽度</param>
        /// <param name="gap">杆件之间的缝隙</param>
        /// <returns></returns>
        private static Vector2D GetExtendPoint(Vector2D intersectPoint, Vector2D dir, double angle, double width, double gap)
        {
            if (angle < Extension.SMALL_NUMBER) throw new Exception("杆件之间夹角过小,无法剪切");
            var d1 = width / 2 / Math.Sin(angle);
            var d2 = Math.Abs(angle - Math.PI / 2) < Extension.SMALL_NUMBER ?
                0 : width / 2 / Math.Tan(angle);
            var dis = d1 - d2 - gap;
            if (dis <= 12) dis = 12;//2.5d，d为螺钉孔径， 一般为4.8mm
            return intersectPoint.Offset(dir * dis);
        }
    }

}
