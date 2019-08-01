using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Geometry.External;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Shell.DWG {
    /// <summary>
    /// <see cref="CadArc"/>到<see cref="Arc"/>的转换器;
    /// </summary>
    [Export(typeof(ICADBaseToDrawObjectConverter))]
    class CadArcToArcConverter : CADBaseToDrawObjectConverterGenericBase<CadArc> {
        protected override DrawObject Convert(CadArc cadArc) {
            return new Arc(
                new Arc2D(ConvertUtils.Cad3DPointToVector2D(cadArc.CenterPoint)) {
                    StartAngle = Extension.DegToRad(cadArc.StartAngle),
                    Radius = cadArc.Radius,
                    Angle = Extension.DegToRad(cadArc.EndAngle - cadArc.StartAngle)
                }
            );
        }
    }
}
