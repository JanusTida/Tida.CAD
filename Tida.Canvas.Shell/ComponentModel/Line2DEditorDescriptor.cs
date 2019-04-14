using Tida.Canvas.Shell.Contracts.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;

namespace Tida.Canvas.Shell.ComponentModel {
    [ExportEditorDescriptor(EditorType = typeof(Views.Line2DEditor),TypeGUID = EditorType_Line2D)]
    class Line2DEditorDescriptor : IEditorDescriptor {
        
    }
}
