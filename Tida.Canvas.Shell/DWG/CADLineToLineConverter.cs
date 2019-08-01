using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Infrastructure.DrawObjects;

namespace Tida.Canvas.Shell.DWG {
    class CADLineToLineConverter : CADBaseToDrawObjectConverterGenericBase<CadLine> {
        protected override Tida.Canvas.Contracts.DrawObject Convert(CadLine cadLine) {
            return new Line(
                ConvertUtils.Cad3DPointToVector2D(cadLine.FirstPoint),
                ConvertUtils.Cad3DPointToVector2D(cadLine.SecondPoint)
            );
        }
    }
}
