using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Infrastructure.ExtendTools;
using Tida.Canvas.Contracts;
using Tida.Geometry.External;
using Tida.Geometry.External.Util;
using Tida.Geometry.Primitives;
using System.Linq;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Base.ExtendTools {
    /// <summary>
    /// 线段的延伸工具;
    /// </summary>
    [Export(typeof(IDrawObjectExtendTool))]
    public class LineExtendTool : DrawObjectExtendToolGenericBase<Line> {
        protected override DrawObject ExtendDrawObject(Line line, DrawObjectExtendInfo objectExtendInfo) {
            var intersectPoints = objectExtendInfo.IntersectPositions;
            var extendArea = objectExtendInfo.ExtendArea;
            if (line == null) return null;
            if (intersectPoints == null) return null;
            if (extendArea == null) return null;
            
            var startIsInArea = objectExtendInfo.ExtendArea.Contains(line.Line2D.Start);
            var endIsInArea = objectExtendInfo.ExtendArea.Contains(line.Line2D.End);
            if (startIsInArea == endIsInArea) {
                return null;
            }
            
            (Vector2D rayStartPos,Vector2D rayEndPos) = startIsInArea ? (line.Line2D.End,line.Line2D.Start) : (line.Line2D.Start ,line.Line2D.End);
            var rayVector = rayEndPos - rayStartPos;

            //检查是否在射线的延长线上;
            intersectPoints = intersectPoints.Where(p => {
                if (p.IsInLine(line.Line2D)){
                    return false;
                }

                var rayStartToPointVector = p - rayStartPos;
                var cross = rayStartToPointVector.Cross(rayVector);
                var dot = rayStartToPointVector.Dot(rayVector);
                if (cross.AreEqual(0) && dot.AreEqual(rayStartToPointVector.Modulus() * rayVector.Modulus())) {
                    return true;
                }

                return false;
            }).OrderBy(p => p.Distance(line.Line2D.Start)).ToArray();

            
            if(intersectPoints.Length != 1) {
                return null;
            }
            
            //寻找距离射线起点最近的点;
            Vector2D closestPoint = null;
            double shortestDis = 0;
            
            foreach (var point in intersectPoints) {
                var thisDis = point.Distance(rayStartPos);
                if(closestPoint == null) {
                    closestPoint = point;
                    shortestDis = thisDis;
                    continue;
                }
                
                if(thisDis < shortestDis) {
                    closestPoint = point;
                    shortestDis = thisDis;
                }
            }

            return new Line(new Line2D(closestPoint, rayStartPos));
        }
    }
}
