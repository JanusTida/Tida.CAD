using Tida.Geometry.External;
using Tida.Geometry.External.Util;
using Tida.Geometry.Primitives;
using System;
using System.Linq;

namespace Tida.Canvas.Infrastructure.ExtendTools {
    public static class ExtendExtensions
    {
        public static Line2D GetExtendLineWithIntersectPositions(Line2D line2D, Line2D[] extendLines, Vector2D[] intersectPositions, Rectangle2D2 rectIntersect)
        {
            if (extendLines.Length == 0) return null;
            if (rectIntersect == null) return null;
            if (intersectPositions.Length == 0) return null;
            if (rectIntersect.Contains(line2D.Start) && rectIntersect.Contains(line2D.End) && intersectPositions.Length == 2)
            {
                var p1 = intersectPositions[0];
                var p2 = intersectPositions[1];
                if (line2D.Start.IsInLine(Line2D.Create(p1, line2D.End)))
                    return Line2D.Create(p1, p2);
                return Line2D.Create(p2, p1);
            }

            if (rectIntersect.Contains(line2D.Start))
                return Line2D.Create(intersectPositions.First(), line2D.End);
            if (rectIntersect.Contains(line2D.End))
                return Line2D.Create(line2D.Start, intersectPositions.First());
            return null;
        }

        public static Line2D GetExtendMemberWithIntersectPositions(Line2D line2D, Line2D[] extendLines, Vector2D[] intersectPositions, Rectangle2D2 rectIntersect, double width, double gap)
        {
            if (extendLines.Length == 0) return null;
            if (rectIntersect == null) return null;
            if (intersectPositions.Length == 0) return null;
            if (rectIntersect.Contains(line2D.Start) && rectIntersect.Contains(line2D.End) && intersectPositions.Length == 2)
            {
                var p1 = intersectPositions[0];
                var p2 = intersectPositions[1];
                var line1 = extendLines.First(x => p1.IsInLine(x));
                var angle1 = line1.Direction.AngleWith(line2D.Direction);
                var newP1 = GetExtendPoint(p1, -line2D.Direction, angle1, width, gap);
                var line2 = extendLines.First(x => p2.IsInLine(x));
                var angle2 = line2.Direction.AngleWith(line2D.Direction);
                var newP2 = GetExtendPoint(p2, line2D.Direction, angle2, width, gap);
                if (line2D.Start.IsInLine(Line2D.Create(p1, line2D.End)))
                {
                    return Line2D.Create(newP1, newP2);
                }

                return Line2D.Create(newP2, newP1);
            }

            if (rectIntersect.Contains(line2D.Start) && line2D.Start.IsInLine(Line2D.Create(intersectPositions.First(), line2D.End)))
            {
                var angle = line2D.Direction.AngleWith(extendLines[0].Direction);
                var newEndPoint = GetExtendPoint(intersectPositions.First(), -line2D.Direction, angle, width, gap);
                return Line2D.Create(newEndPoint, line2D.End);
            }

            if (rectIntersect.Contains(line2D.End) && line2D.End.IsInLine(Line2D.Create(intersectPositions.First(), line2D.Start)))
            {
                var angle = line2D.Direction.AngleWith(extendLines[0].Direction);
                var newEndPoint = GetExtendPoint(intersectPositions.First(), line2D.Direction, angle, width, gap);
                return Line2D.Create(line2D.Start, newEndPoint);
            }
            return null;
        }

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
