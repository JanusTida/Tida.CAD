using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Shell.DWG {
    class CadCircleToEllipseConverter : CADBaseToDrawObjectConverterGenericBase<CadCircle> {
        protected override DrawObject Convert(CadCircle cadCircle) {
            return new Ellipse(
                new Ellipse2D(
                    ConvertUtils.Cad3DPointToVector2D(cadCircle.CenterPoint),
                    cadCircle.Radius,
                    cadCircle.Radius
                )
            );
        }
    }
}
