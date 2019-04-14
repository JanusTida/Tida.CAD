using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Contracts {
    
    /// <summary>
    /// 可以在视图坐标和工程数学坐标间进行转化的契约;
    /// </summary>
    public interface ICanvasScreenConvertable {

        /// <summary>
        /// 实际视图宽度;
        /// </summary>
        double ActualWidth { get; }

        /// <summary>
        /// 实际视图高度;
        /// </summary>
        double ActualHeight { get; }
        
        /// <summary>
        /// 将工程数学坐标转化为视图(以左上为原点)坐标;
        /// </summary>
        /// <param name="unitpoint"></param>
        /// <returns></returns>
        void ToScreen(Vector2D unitpoint,Vector2D screenPoint);

        /// <summary>
        /// 将工程数学长度转化为视图长度;
        /// </summary>
        /// <param name="unitvalue"></param>
        /// <returns></returns>
        double ToScreen(double unitvalue);

        /// <summary>
        /// 将视图长度转化为工程数学长度
        /// </summary>
        /// <param name="screenvalue"></param>
        /// <returns></returns>
        double ToUnit(double screenvalue);

        /// <summary>
        /// 将视图坐标转化为工程数学坐标;
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        void ToUnit(Vector2D screenPoint,Vector2D unitPoint);

        /// <summary>
        /// 获取单个字符在视图上的单位大小;
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        Size GetCharScreenSize(char ch); 


        /// <summary>
        /// 当前缩放比例;
        /// </summary>
        double Zoom { get;  }
    }

    /// <summary>
    /// <see cref="ICanvasScreenConvertable"/>拓展;
    /// </summary>
    public static class CanvasScreenConvertableExtension {
        public static Vector2D GetBottomRightUnitPoint(this ICanvasScreenConvertable canvasProxy) {

            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            return canvasProxy.ToUnit(new Vector2D(canvasProxy.ActualWidth, canvasProxy.ActualHeight));
        }

        public static Vector2D GetTopLeftUnitPoint(this ICanvasScreenConvertable canvasProxy) {
            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            return canvasProxy.ToUnit(new Vector2D(0, 0));
        }

        /// <summary>
        /// 将工程数学坐标转化为视图(以左上为原点)坐标;
        /// </summary>
        /// <param name="unitPoint"></param>
        /// <returns></returns>
        public static Vector2D ToScreen(this ICanvasScreenConvertable canvasProxy,Vector2D unitPoint) {
            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            if (unitPoint == null) {
                throw new ArgumentNullException(nameof(unitPoint));
            }

            var screenPoint = new Vector2D();
            canvasProxy.ToScreen(unitPoint, screenPoint);
            return screenPoint;
        }

        /// <summary>
        /// 将视图坐标转化为工程数学坐标;
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        public static Vector2D ToUnit(this ICanvasScreenConvertable canvasProxy,Vector2D screenPoint) {
            if (canvasProxy == null) {
                throw new ArgumentNullException(nameof(canvasProxy));
            }

            if (screenPoint == null) {
                throw new ArgumentNullException(nameof(screenPoint));
            }

            var unitPoint = new Vector2D();
            canvasProxy.ToUnit(screenPoint, unitPoint);
            return unitPoint;
        }
    }
}
