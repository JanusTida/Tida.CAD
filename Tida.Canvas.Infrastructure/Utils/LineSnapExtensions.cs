using Tida.Canvas.Infrastructure.Snaping.Shapes;
using Tida.Canvas.Contracts;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Infrastructure.Constants;
using static Tida.Canvas.Infrastructure.Utils.PositionHitUtils;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 线段的辅助拓展;
    /// </summary>
    public static class LineSnapExtensions {

        /// <summary>
        /// 根据关注点作向线段的投影的辅助图形;
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static ISnapShape GetVertextSnapShape(Vector2D position, Line2D relatedLine2D) {
            var verticalVertextScreenPosition = Extension.ProjectOn(position, relatedLine2D);
            if (verticalVertextScreenPosition != null) {
                return new SnapShapeForLine(verticalVertextScreenPosition, relatedLine2D) {
                    Background = InLineSnapPointBackground
                };
            }

            return null;
        }

        /// <summary>
        /// 获取某个点是否在某个线段上的辅助;
        /// </summary>
        /// <param name="line2D"></param>
        /// <param name="position"></param>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static ISnapShape GetOnLineSnapShape(Line2D line2D,Vector2D position,ICanvasContext canvasContext) {

            if (line2D == null) {
                throw new ArgumentNullException(nameof(line2D));
            }

            if (position == null) {
                throw new ArgumentNullException(nameof(position));
            }


            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }
            
            var screenPosition = canvasContext.CanvasProxy.ToScreen(position);
            /*判断关注点是否"在线段上" */

            if (line2D.Length == 0) {
                return null;
            }

            var screenLine2D = new Line2D(
                canvasContext.CanvasProxy.ToScreen(line2D.Start),
                canvasContext.CanvasProxy.ToScreen(line2D.End)
            );

            //若距离大于指定值,则无辅助点;
            if (screenLine2D.Distance(screenPosition) > TolerantedScreenLength) {
                return null;
            }

            //若上次编辑位置为空,则根据关注点与线段的垂直距离得到一个交点;
            if (canvasContext.LastEditPosition == null) {
                return GetVertextSnapShape(position, line2D);
            }
            ///否则,以<see cref="canvasContext.LastEditPosition"/> 
            ///上次编辑位置到关注点<see cref="position"/>的直线与线段的交点作为辅助点;
            else {
                //计算上次编辑到
                var lastToPositionLine2D = new Line2D(canvasContext.LastEditPosition, position);
                var vertextPosition = lastToPositionLine2D.IntersectStraightLine(line2D);

                //若无交点,则可能平行,无辅助点;
                if (vertextPosition == null) {
                    return null;
                }

                //若关注点与两直线的相交点相距过远,则仍然根据关注点与线段的垂直距离得到一个交点;
                if (canvasContext.CanvasProxy.ToScreen(position.Distance(vertextPosition)) > TolerantedScreenLength) {
                    return LineSnapExtensions.GetVertextSnapShape(position, line2D);
                }

                return new SnapShapeForLine(vertextPosition, line2D) {
                    Background = InLineSnapPointBackground
                };
            }
        }
        
        /// <summary>
        /// 线段的辅助判断;
        /// 依次判断以下情况:1.中点 2.端点 
        /// </summary>
        public static ISnapShape GetLine2DSnapShape(Line2D line2D, Vector2D position, ICanvasScreenConvertable canvasProxy) {

            if (line2D == null) {
                throw new ArgumentNullException(nameof(line2D));
            }


            if (position == null) {
                throw new ArgumentNullException(nameof(position));
            }


            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            /*判断是否是三点中一个附近的点*/

            var startPoint = line2D.Start;
            var endPoint = line2D.End;

            if (startPoint == null) {
                throw new ArgumentNullException(nameof(startPoint));
            }

            if (endPoint == null) {
                throw new ArgumentNullException(nameof(endPoint));
            }

            if (position == null) {
                throw new ArgumentNullException(nameof(position));
            }

            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            var middlePoint = (startPoint + endPoint) / 2;
            Vector2D[] points = { startPoint, endPoint, middlePoint };

            var screenPoint = new Vector2D();
            var screenStart = new Vector2D();
            var screenEnd = new Vector2D();

            var lineDir = (endPoint - startPoint).Normalize();
            var verticalVector = new Vector2D(-lineDir.Y, lineDir.X);

            var screenPosition = canvasProxy.ToScreen(position);

            foreach (var point in points) {
                canvasProxy.ToScreen(point, screenPoint);
                if (!GetIsSurround(screenPosition, screenPoint)) {
                    continue;
                }
                
                var unitLineLength = canvasProxy.ToUnit(4  * TolerantedScreenLength);

                var start = point - verticalVector * unitLineLength;
                var end = point + verticalVector * unitLineLength;

                //作垂直于原线段的垂线;
                canvasProxy.ToScreen(start, screenStart);
                canvasProxy.ToScreen(end, screenEnd);

                var lineSnapShape = new ScreenLineSnapShape(new Line2D(screenStart, screenEnd), point);
                //若命中点为中点,则该垂线为中垂线,则修改颜色;
                if (point == middlePoint) {
                    lineSnapShape.LinePen = MiddleLinePen;
                }

                return lineSnapShape;
            }

            return null;
        }

        
    }
}
