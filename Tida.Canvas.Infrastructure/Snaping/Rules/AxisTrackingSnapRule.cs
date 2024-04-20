using System;
using Tida.Geometry.Primitives;
using Tida.Geometry.External;
using Tida.Canvas.Infrastructure.Snaping.Shapes;
using Tida.Canvas.Infrastructure.Snaping;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Infrastructure.Snaping.Rules {

    /// <summary>
    /// 辅助规则,极轴追踪;
    /// </summary>
    public class AxisTrackingSnapRule : ISnapShapeRule {
      
        public ISnapShape MatchSnapShape(DrawObject[] drawObjects, Vector2D position, ICanvasContext canvasContext) {
            if (position == null) {
                throw new ArgumentNullException(nameof(position));
            }
            
            var lastEditPosition = canvasContext.LastEditPosition;
            if(lastEditPosition == null) {
                return null;
            }

            if (!IsEnabled) {
                return null;
            }

            var snapEndPosition = GetSnapEndPosition(lastEditPosition, position);

            if(snapEndPosition == null) {
                return null;
            }
            
            var axisSnapLine = new RaySnapLine(lastEditPosition,snapEndPosition);

            return axisSnapLine;
        }

        /// <summary>
        /// 容错角度大小,单位为弧度;
        /// </summary>
        private const double Alpha = 1 / (180 / Math.PI);
        private static readonly double CosAlpha = Math.Cos(Alpha);

        /// <summary>
        /// 极轴追踪功能是否被启用;
        /// </summary>
        public static bool IsEnabled { get; set; } = true;

        //var section = SettingsService.GetOrCreateSection(SettingSectionGUID_Canvas);
        //var axisTrackingEnabled = section?.GetAttribute<bool>(SettingName_AxisTrackingEnabled) ?? false;
        //        return axisTrackingEnabled;

        /// <summary>
        /// 根据两个点的位置,获取与极轴相关的射线的延伸点;
        /// </summary>
        private static Vector2D GetSnapEndPosition(
            Vector2D startPosition,
            Vector2D position
        ) {

            if(startPosition.IsAlmostEqualTo(position)) {
                return null;
            }
            
            var relativePos = position - startPosition;

            var relativePosModulus = relativePos.Modulus();
            if (relativePosModulus == 0) {
                return null;
            }

            foreach (var unitVector2D in _unitVector2Ds) {
                //根据余弦定理,得到余弦值,根据余弦值判断,与对应轴的夹角是否小于容错范围;
                var dot = unitVector2D.Dot(relativePos);
                var cos = Math.Abs(dot / (relativePos.Modulus() * unitVector2D.Modulus()));
                //若余弦大于指定值,则角度在容错范围内;
                if(cos > CosAlpha) {
                    //返回该点在"轴"上的投影点;
                    return position.ProjectOn(new Line2D(startPosition,startPosition + unitVector2D));
                }
            }

            return null; 
        }

        /// <summary>
        /// 延四个极轴方向的单位向量;
        /// </summary>
        private static readonly Vector2D[]  _unitVector2Ds = new Vector2D[] {
            new Vector2D(0,1),
            new Vector2D(0,-1),
            new Vector2D(1,0),
            new Vector2D(-1,0),
        };
    }
}
