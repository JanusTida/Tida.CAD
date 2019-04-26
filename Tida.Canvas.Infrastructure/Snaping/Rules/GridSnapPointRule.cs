using System;
using System.Linq;
using Tida.Geometry.Primitives;
using Tida.Geometry.External.Util;
using Tida.Canvas.Infrastructure.Utils;
using Tida.Canvas.Infrastructure.Snaping.Shapes;
using Tida.Canvas.Infrastructure.Snaping;
using static Tida.Canvas.Infrastructure.Constants;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Infrastructure.Snaping.Rules {
    /// <summary>
    /// 网格线的整数点与关注点关系的辅助规则;
    /// </summary>
    public class GridSnapPointRule : ISnapShapeRule {
        public ISnapShape MatchSnapShape(DrawObject[] drawObjects, Vector2D position, ICanvasContext canvasContext) {
            if (position == null) {
                throw new ArgumentNullException(nameof(position));
            }


            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            //若当前网格的视图长度过小,可能画布缩放比例过大,不能匹配;
            var screenUnitLength = canvasContext.CanvasProxy.ToScreen(1);
            if (screenUnitLength < TolerantedScreenLength * 2) {
                return null;
            }

            var screenPosition = canvasContext.CanvasProxy.ToScreen(position);

            int gridX = (int)Math.Round(position.X);
            int gridY = (int)Math.Round(position.Y);
            var gridPointPosition = new Vector2D(gridX, gridY);
            var gridPointScreenPosition = canvasContext.CanvasProxy.ToScreen(gridPointPosition);

            var screenRect = NativeGeometryExtensions.GetNativeSuroundingScreenRect(
                gridPointScreenPosition,
                TolerantedScreenLength, TolerantedScreenLength
            );

            if (screenRect.Contains(screenPosition)) {
                return new StandardSnapPoint(gridPointPosition);
            }

            return null;
        }
    }
}
