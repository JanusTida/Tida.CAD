using Tida.Canvas.Shell.Contracts.ComponentModel;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;

namespace Tida.Canvas.Shell.ComponentModel {
    [ExportEditorDescriptor(EditorType = typeof(Views.Vector2DEditor),TypeGUID = EditorType_Vector2D)]
    class Vector2DEditorDescriptor : IEditorDescriptor {
    }
}
