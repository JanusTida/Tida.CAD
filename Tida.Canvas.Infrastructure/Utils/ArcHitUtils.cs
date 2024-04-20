using System;
using System.Collections.Generic;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;
using static Tida.Canvas.Infrastructure.Constants;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 圆弧判定;
    /// </summary>
    public class ArcHitUtils {

        /// <summary>
        /// 判断画布中某个位置是否在某个圆弧附近;
        /// </summary>
        /// <param name="arc2D"></param>
        /// <param name="point"></param>
        /// <param name="canvasProxy"></param>
        /// <returns></returns>
        public static bool PointInArc(Arc2D arc2D, Vector2D point, ICanvasScreenConvertable canvasProxy) {
            if (canvasProxy == null) {
                return false;
            }

            if (arc2D == null) {
                return false;
            }

            //以视图单位为标准进行判断;
            var screenPoint = canvasProxy.ToScreen(point);
            var arcCenterScreenPoint = canvasProxy.ToScreen(arc2D.Center);
            var screenRadius = canvasProxy.ToScreen(arc2D.Radius);

            if(Math.Abs(arcCenterScreenPoint.Distance(screenPoint) - screenRadius) > TolerantedScreenLength) {
                return false;
            }
            
            var centerToPointDirection = new Line2D(arc2D.Center, point).Direction;
            if (Extension.AreEqual(centerToPointDirection.Modulus(),0)) {
                return false;
            }

            var centerToPointAngle = Extension.AngleFrom(
                new Vector3D(centerToPointDirection.X,centerToPointDirection.Y,0),
                Vector3D.BasisX
            );

            if(arc2D.Angle > 0) {
                return centerToPointAngle < arc2D.StartAngle + arc2D.Angle && centerToPointAngle > arc2D.StartAngle;
            }
            else {
                return centerToPointAngle < arc2D.StartAngle && centerToPointAngle > arc2D.StartAngle + arc2D.Angle;
            }
            
            
        }
    }
}
