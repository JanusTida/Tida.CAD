using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Shell.DWG {
    /// <summary>
    /// 转换方法;
    /// </summary>
    static class ConvertUtils {
        public static Vector2D Cad3DPointToVector2D(Cad3DPoint cad3DPoint) {
            return new Vector2D(cad3DPoint.X, cad3DPoint.Y);
        }
    }
} 
