using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Shell.DWG {
    static class CADBaseToDrawObjectConverters {
        static CADBaseToDrawObjectConverters() {
            _converters = new ICADBaseToDrawObjectConverter[] {
                new CADLineToLineConverter(),
                new CadCircleToEllipseConverter()
            };
        }

        public static IReadOnlyList<ICADBaseToDrawObjectConverter> Converters => _converters;
        private static ICADBaseToDrawObjectConverter[] _converters;


        const string Prefix = "pack://application:,,,/Tida.Canvas.Shell;component/Resources/";




    }
}
