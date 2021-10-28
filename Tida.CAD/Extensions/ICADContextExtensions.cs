using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tida.CAD.Extensions
{

    /// <summary>
    /// 画布上下文拓展;
    /// </summary>
    public static class ICADControlExtensions
    {
        /// <summary>
        /// 获取画布上下文中所有绘制对象;
        /// </summary>
        /// <param name="cadContext"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllDrawObjects(this ICADControl cadContext)
        {

            if (cadContext == null)
            {
                throw new ArgumentNullException(nameof(cadContext));
            }

            return cadContext.Layers?.SelectMany(p => p.DrawObjects) ?? Enumerable.Empty<DrawObject>();
        }

        /// <summary>
        /// 获取画布上下文中所有可见绘制对象;
        /// </summary>
        /// <param name="cadContext"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllVisibleDrawObjects(this ICADContext cadContext)
        {

            if (cadContext == null)
            {
                throw new ArgumentNullException(nameof(cadContext));
            }

            return cadContext.Layers?.
                Where(p => p.IsVisible)?.
                SelectMany(p => p.DrawObjects)?.
                Where(p => p.IsVisible) ?? Enumerable.Empty<DrawObject>();
        }

        /// <summary>
        /// 获取指定类型的画布上下文中可见的绘制对象;
        /// </summary>
        /// <param name="cadContext"></param>
        /// <returns></returns>
        public static IEnumerable<TDrawObject> GetAllVisibleDrawObjects<TDrawObject>(this ICADContext cadContext) where TDrawObject : DrawObject
        {
            return GetAllDrawObjects(cadContext).Select(p => p as TDrawObject).Where(p => p != null);
        }

        /// <summary>
        /// 获得画布上下文中所有可见的图层;
        /// </summary>
        /// <param name="cadContext"></param>
        /// <returns></returns>
        public static IEnumerable<CADLayer> GetVisibleLayers(this ICADContext cadContext)
        {
            if (cadContext == null)
            {
                throw new ArgumentNullException(nameof(cadContext));
            }

            return cadContext.Layers?.Where(p => p.IsVisible) ?? Enumerable.Empty<CADLayer>();
        }

        /// <summary>
        /// 获得画布上下文中所有可以被交互的图层;
        /// </summary>
        /// <param name="cadContext"></param>
        /// <returns></returns>
        public static IEnumerable<CADLayer> GetInteractionableLayers(this ICADContext cadContext)
        {
            if (cadContext == null)
            {
                throw new ArgumentNullException(nameof(cadContext));
            }

            return cadContext.Layers?.Where(p => !p.IsLocked)?.
                Where(p => p.IsVisible) ?? Enumerable.Empty<CADLayer>();
        }

       
    }

}
