using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
