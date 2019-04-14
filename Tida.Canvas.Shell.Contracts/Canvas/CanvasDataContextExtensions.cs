using CDO.Common.Canvas.Contracts;
using CDO.Common.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDO.Common.Canvas.Shell.Contracts.Canvas {

    /// <summary>
    /// 业务层的画布的上下文数据拓展;
    /// </summary>
    public static class CanvasDataContextExtensions {
        /// <summary>
        /// 调整画布位置和缩放比例,以使得所有绘制对象在可见的范围内;
        /// </summary>
        public static void ViewAllDrawObjects(this ICanvasDataContext canvasDataContext) {
            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            if (canvasDataContext.CanvasProxy == null) {
                return;
            }

            if (canvasDataContext.Layers == null) {
                return;
            }

            //获取所有绘制对象所在的矩形;
            var rects = canvasDataContext.Layers.
                SelectMany(p => p.DrawObjects).
                Select(p => p.GetBoundingRect()).Where(p => p != null);

            var allVertexes = rects.SelectMany(p => p.GetVertexes()).ToArray();

            if (allVertexes.Length == 0) {
                return;
            }

            //取所有矩形的最小/大的横/纵坐标;
            //获得关注区域的信息,这将是一个矩形;(长度/宽度可能为零);
            var minX = allVertexes.Min(p => p.X);
            var maxX = allVertexes.Max(p => p.X);
            var minY = allVertexes.Min(p => p.Y);
            var maxY = allVertexes.Max(p => p.Y);

            var canvasProxy = canvasDataContext.CanvasProxy;
            var actualWidth = canvasProxy.ActualWidth;
            var actualHeight = canvasProxy.ActualHeight;

            //计算该矩形区域的中点位置;
            var middleX = (minX + maxX) / 2;
            var middleY = (minY + maxY) / 2;

            //计算该矩形区域的长宽;加上一个常数是为了防止该矩形区域中任意一边为零的情况,导致出现除以零的异常;
            var newWidth = canvasProxy.ToUnit(canvasProxy.ToScreen(maxX - minX) + 200);
            var newHeight = canvasProxy.ToUnit(canvasProxy.ToScreen(maxY - minY) + 200);

            //与当前放大比例下的矩形区域大小进行比例计算,以计算缩放应该乘以的倍数;
            var timeX = canvasProxy.ToUnit(actualWidth) / newWidth;
            var timeY = canvasProxy.ToUnit(actualHeight) / newHeight;

            canvasDataContext.Zoom *= Math.Min(timeX, timeY);

            //通过改变原点所在的视图坐标,将该矩形区域的中点移动至视图中心位置;
            //计算该中点与原点的视图偏移;
            var middlePointToPanOffset =
                canvasProxy.ToScreen(new Vector2D(middleX, middleY)) -
                canvasProxy.ToScreen(Vector2D.Zero);

            //以下操作等效于将原点平移至视图中心位置后再将其向矩形区域中点相反的方向进行平移;
            canvasDataContext.PanScreenPosition = new Vector2D(actualWidth / 2, actualHeight / 2) - middlePointToPanOffset;
        }

        /// <summary>
        /// 在画布上下文中,获取所有特定类型的可见绘制对象;
        /// </summary>
        /// <typeparam name="TDrawObject"></typeparam>
        /// <param name="canvasDataContext"></param>
        /// <returns></returns>
        public static IEnumerable<TDrawObject> GetAllVisibleDrawObject<TDrawObject>(this ICanvasDataContext canvasDataContext) where TDrawObject : DrawObject {

            if (canvasDataContext == null) {
                throw new ArgumentNullException(nameof(canvasDataContext));
            }

            return canvasDataContext?.Layers?.SelectMany(p => p.DrawObjects)?.Select(p => p as TDrawObject)?.Where(p => p != null && p.IsVisible);
        }
    }
}
