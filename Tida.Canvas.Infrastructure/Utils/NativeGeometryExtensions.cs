using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Infrastructure.Utils {
    /// <summary>
    /// 以视图坐标为准,进行某些几何运算;
    /// </summary>
    public static class NativeGeometryExtensions {
        /// <summary>
        /// 得到以某视图坐标为中心的视图矩形;
        /// </summary>
        /// <param name="nativeCenterRectPoint">以视图为准的中心坐标</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static Rectangle2D2 GetNativeSuroundingScreenRect(Vector2D nativeCenterRectPoint, double width, double height) {
            if (nativeCenterRectPoint == null) {
                throw new ArgumentNullException(nameof(nativeCenterRectPoint));
            }

            var surroundingRect = Rectangle2D2.CreateEmpty();
            GetNativeSuroundingScreenRect(nativeCenterRectPoint, width, height, surroundingRect);
            return surroundingRect;
        }

        /// <summary>
        /// 得到以某视图坐标为中心的视图矩形;
        /// </summary>
        /// <param name="nativeCenterRectPoint">以视图为准的中心坐标</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="rectangle2D2">用于写入的矩形实例</param>
        /// <returns></returns>
        public static void GetNativeSuroundingScreenRect(Vector2D nativeCenterRectPoint, double width, double height,Rectangle2D2 rectangle2D2) {
            if (nativeCenterRectPoint == null) {
                throw new ArgumentNullException(nameof(nativeCenterRectPoint));
            }

            if (rectangle2D2 == null) {
                throw new ArgumentNullException(nameof(rectangle2D2));
            }

            rectangle2D2.MiddleLine2D = new Line2D(
                new Vector2D(nativeCenterRectPoint.X - width / 2, nativeCenterRectPoint.Y),
                new Vector2D(nativeCenterRectPoint.X + width / 2, nativeCenterRectPoint.Y)
            );
            rectangle2D2.Width = height;
        }
    }
}
