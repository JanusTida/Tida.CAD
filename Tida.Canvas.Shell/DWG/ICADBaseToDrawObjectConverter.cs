using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.DWG {
    /// <summary>
    /// 将<see cref="CadBase"/>转换为<see cref="DrawObject"/>的转换器契约;
    /// </summary>
    public interface ICADBaseToDrawObjectConverter {
        DrawObject Convert(CadBase cadBase);
    }


}
