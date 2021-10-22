using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tida.CAD.Extensions
{

    /// <summary>
    /// 画布上下文拓展;
    /// </summary>
    public static class ICanvasContextExtensions
    {
        /// <summary>
        /// 获取画布上下文中所有绘制对象;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllDrawObjects(this ICanvasContext canvasContext)
        {

            if (canvasContext == null)
            {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            return canvasContext.Layers?.SelectMany(p => p.DrawObjects) ?? Enumerable.Empty<DrawObject>();
        }

        /// <summary>
        /// 获取画布上下文中所有可见绘制对象;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllVisibleDrawObjects(this ICanvasContext canvasContext)
        {

            if (canvasContext == null)
            {
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
        public static IEnumerable<TDrawObject> GetAllVisibleDrawObjects<TDrawObject>(this ICanvasContext canvasContext) where TDrawObject : DrawObject
        {
            return GetAllDrawObjects(canvasContext).Select(p => p as TDrawObject).Where(p => p != null);
        }

        /// <summary>
        /// 获得画布上下文中所有可见的图层;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<CanvasLayer> GetVisibleLayers(this ICanvasContext canvasContext)
        {
            if (canvasContext == null)
            {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            return canvasContext.Layers?.Where(p => p.IsVisible) ?? Enumerable.Empty<CanvasLayer>();
        }

        /// <summary>
        /// 获得画布上下文中所有可以被交互的图层;
        /// </summary>
        /// <param name="canvasContext"></param>
        /// <returns></returns>
        public static IEnumerable<CanvasLayer> GetInteractionableLayers(this ICanvasContext canvasContext)
        {
            if (canvasContext == null)
            {
                throw new ArgumentNullException(nameof(canvasContext));
            }

            return canvasContext.Layers?.Where(p => !p.IsLocked)?.
                Where(p => p.IsVisible) ?? Enumerable.Empty<CanvasLayer>();
        }

       
    }

}
