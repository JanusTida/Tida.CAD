using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tida.CAD.Extensions
{

    /// <summary>
    /// Some extended methods for <see cref="ICADControl"/>
    /// </summary>
    public static class CADControlExtensions
    {
        /// <summary>
        /// Get all drawobjects of the a specified cad control;
        /// </summary>
        /// <param name="cadControl"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllDrawObjects(this ICADControl cadControl)
        {

            if (cadControl == null)
            {
                throw new ArgumentNullException(nameof(cadControl));
            }

            return cadControl.Layers?.SelectMany(p => p.DrawObjects) ?? Enumerable.Empty<DrawObject>();
        }

        /// <summary>
        ///  Get all visible drawobjects in a cad control;
        /// </summary>
        /// <param name="cadControl"></param>
        /// <returns></returns>
        public static IEnumerable<DrawObject> GetAllVisibleDrawObjects(this ICADControl cadControl)
        {

            if (cadControl == null)
            {
                throw new ArgumentNullException(nameof(cadControl));
            }

            return cadControl.Layers?.
                Where(p => p.IsVisible)?.
                SelectMany(p => p.DrawObjects)?.
                Where(p => p.IsVisible) ?? Enumerable.Empty<DrawObject>();
        }

        /// <summary>
        /// Get all visible drawobjects of the specified type in the control;
        /// </summary>
        /// <param name="cadContext"></param>
        /// <returns></returns>
        public static IEnumerable<TDrawObject> GetAllVisibleDrawObjects<TDrawObject>(this ICADControl cadContext) where TDrawObject : DrawObject
        {
            return GetAllDrawObjects(cadContext).Select(p => p as TDrawObject).Where(p => p != null);
        }

        /// <summary>
        /// Get all visible layers in the control;
        /// </summary>
        /// <param name="cadControl"></param>
        /// <returns></returns>
        public static IEnumerable<CADLayer> GetVisibleLayers(this ICADControl cadControl)
        {
            if (cadControl == null)
            {
                throw new ArgumentNullException(nameof(cadControl));
            }

            return cadControl.Layers?.Where(p => p.IsVisible) ?? Enumerable.Empty<CADLayer>();
        }
       
    }

}
