using Tida.Canvas.Events;
using Tida.Canvas.Input;
using Tida.Geometry.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tida.Canvas.Contracts {
    /// <summary>
    /// 画布数据协约;
    /// </summary>
    public interface ICanvasContext {
        /// <summary>
        /// 所有图层(内容图层);
        /// </summary>
        IEnumerable<CanvasLayer> Layers { get; set; }

        /// <summary>
        /// 坐标间进行转化契约实例;
        /// </summary>
        ICanvasScreenConvertable CanvasProxy { get; }

        /// <summary>
        /// 当前选定的活动图层;
        /// </summary>
        CanvasLayer ActiveLayer { get; set; }

        /// <summary>
        /// 活动图层发生变化时的事件;
        /// </summary>
        event EventHandler<ValueChangedEventArgs<CanvasLayer>> ActiveLayerChanged;

        /// <summary>
        /// 放大比例;
        /// </summary>
        double Zoom { get; set; }


        /// <summary>
        /// 上次编辑的标识位置;以工程数学坐标为准;
        /// </summary>
        Vector2D LastEditPosition { get;  }


        /// <summary>
        /// 原点所在的视图坐标位置;
        /// </summary>
        Vector2D PanScreenPosition { get; set; }

        /// <summary>
        /// 输入设备服务封装;
        /// </summary>
        IInputDevice InputDevice { get; }

        /// <summary>
        /// 拖拽选择事件;
        /// </summary>
        event EventHandler<DragSelectEventArgs> DragSelect;

        /// <summary>
        /// 拖拽选择鼠标移动事件;
        /// </summary>
        event EventHandler<DragSelectMouseMoveEventArgs> DrawSelectMouseMove;

        ///// <summary>
        ///// 当前的鼠标所在的工程数学坐标;
        ///// </summary>
        //Vector2D CurrentMousePosition { get; }

        ///// <summary>
        ///// 当前的鼠标所在的工程数学坐标发生变化事件;
        ///// </summary>
        //event EventHandler<ValueChangedEventArgs<Vector2D>> CurrentMousePositionChanged;

        /// <summary>
        /// 通知外部,将要针对指定的绘制对象集合,将要进行某种的类型输入交互的预处理事件;
        /// </summary>
        event EventHandler<PreviewDrawObjectsInteractionEventArgs> PreviewInteractionWithDrawObjects;

        /// <summary>
        /// 点击选取事件;
        /// </summary>
        event EventHandler<ClickSelectEventArgs> ClickSelect;
    }

    /// <summary>
    /// 画布上下文拓展;
    /// </summary>
    public static class ICanvasContextExtensions {
        /// <summary>
        /// 获取画布上下文中所有绘制对象;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllDrawObjects(this ICanvasContext canvasContext) {

            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            return canvasContext.Layers?.SelectMany(p => p.DrawObjects)??Enumerable.Empty<DrawObject>();
        }

        /// <summary>
        /// 获取画布上下文中所有可见绘制对象;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllVisibleDrawObjects(this ICanvasContext canvasContext) {

            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            return canvasContext.Layers?.
                Where(p => p.IsVisible)?.
                SelectMany(p => p.DrawObjects)?.
                Where(p => p.IsVisible) ?? Enumerable.Empty<DrawObject>();
        }

        /// <summary>
        /// 获取指定类型的画布上下文中可见的绘制对象;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<TDrawObject> GetAllVisibleDrawObjects<TDrawObject>(this ICanvasContext canvasContext) where TDrawObject:DrawObject {
            return GetAllDrawObjects(canvasContext).Select(p => p as TDrawObject).Where(p => p != null);
        }

        /// <summary>
        /// 获得画布上下文中所有可见的图层;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<CanvasLayer> GetVisibleLayers(this ICanvasContext canvasContext) {
            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            return canvasContext.Layers?.Where(p => p.IsVisible) ?? Enumerable.Empty<CanvasLayer>();
        }

        /// <summary>
        /// 获得画布上下文中所有可以被交互的图层;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<CanvasLayer> GetInteractionableLayers(this ICanvasContext canvasContext) {
            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            return canvasContext.Layers?.Where(p => !p.IsLocked)?.
                Where(p => p.IsVisible) ?? Enumerable.Empty<CanvasLayer>();
        }

        /// <summary>
        /// 调整画布位置和缩放比例,以使得所有绘制对象在可见的范围内;
        /// </summary>
        public static void ViewAllDrawObjects(this ICanvasContext canvasContext) {
            if (canvasContext == null) {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            if (canvasContext.CanvasProxy == null) {
                return;
            }

            if (canvasContext.Layers == null) {
                return;
            }

            //获取所有绘制对象所在的矩形;
            var rects = canvasContext.Layers.
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

            var canvasProxy = canvasContext.CanvasProxy;
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

            canvasContext.Zoom *= Math.Min(timeX, timeY);

            //通过改变原点所在的视图坐标,将该矩形区域的中点移动至视图中心位置;
            //计算该中点与原点的视图偏移;
            var middlePointToPanOffset =
                canvasProxy.ToScreen(new Vector2D(middleX, middleY)) -
                canvasProxy.ToScreen(Vector2D.Zero);

            //以下操作等效于将原点平移至视图中心位置后再将其向矩形区域中点相反的方向进行平移;
            canvasContext.PanScreenPosition = new Vector2D(actualWidth / 2, actualHeight / 2) - middlePointToPanOffset;
        }

        //public static Rectangle2D2 GetAllDrawObjectsRectangle2D(this ICanvasContext canvasContext) {

        //}
    }

}
