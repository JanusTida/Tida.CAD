using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.DWG {
    abstract class CADBaseToDrawObjectConverterGenericBase<TCADBase> : ICADBaseToDrawObjectConverter where TCADBase : CadBase {
        public DrawObject Convert(CadBase cadBase) {
            if (!(cadBase is TCADBase tCADBase)) {
                return null;
            }

            return Convert(tCADBase);
        }

        protected abstract DrawObject Convert(TCADBase cadBase);
    }
}
