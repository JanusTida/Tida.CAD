using Tida.Canvas.Contracts;
using System.Collections.Generic;

namespace Tida.Canvas.Shell.Contracts.CanvasExport {
    /// <summary>
    /// 导出为图像服务;
    /// </summary>
    public interface IExportCanvasAsImgService {
        /// <summary>
        /// 导出指定的绘制对象集合图像到指定流中;
        /// </summary>
        /// <param name="drawObjects"></param>
        void ExportDrawObjectsAsImg(IEnumerable<DrawObject> drawObjects,ExportImgSetting exportImgSetting);
    }
}
