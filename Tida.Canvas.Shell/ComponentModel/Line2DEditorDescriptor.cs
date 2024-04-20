using Tida.Canvas.Shell.Contracts.ComponentModel;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;

namespace Tida.Canvas.Shell.ComponentModel {
    [ExportEditorDescriptor(EditorType = typeof(Views.Line2DEditor),TypeGUID = EditorType_Line2D)]
    class Line2DEditorDescriptor : IEditorDescriptor {
        
    }
}
