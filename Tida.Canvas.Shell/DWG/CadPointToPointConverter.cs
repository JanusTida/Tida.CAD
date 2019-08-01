using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Shell.DWG {
    [Export(typeof(ICADBaseToDrawObjectConverter))]
    class CadPointToPointConverter : CADBaseToDrawObjectConverterGenericBase<CadPoint> {
        protected override DrawObject Convert(CadPoint cadPoint) {
            return new Point(ConvertUtils.Cad3DPointToVector2D(cadPoint.CenterPoint));
        }
    }
}
