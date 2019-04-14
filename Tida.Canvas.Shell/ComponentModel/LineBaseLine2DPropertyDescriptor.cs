using System;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.ComponentModel;
using static Tida.Canvas.Shell.ComponentModel.Constants;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;

namespace Tida.Canvas.Shell.ComponentModel {
    [ExportPropertyDescriptor(Inheritable = true, CategoryNameKey = CategoryName_Geometry, DescriptionNameKey = DescriptionName_Line2D, DisplayNameKey = DisplayName_Line2D, IsReadOnly = false, EditorTypeGUID = EditorType_Line2D, EditorStyle = EditorStyle.DropDown, EditorTargetProperty = EditorTargetProperty_Line2D)]
    class LineBaseLine2DPropertyDescriptor : PropertyDescriptor {
        public LineBaseLine2DPropertyDescriptor() : base(typeof(LineBase), nameof(LineBase.Line2D)) {
        }
    }

    [ExportIgnoredPropertyDescriptor(Inheritable = true)]
    class LineBaseIgnoredPropertyDescriptors : IgnoredPropertyDescriptor {
        public LineBaseIgnoredPropertyDescriptors() : base(typeof(LineBase),new string[] { nameof(LineBase.Pen),nameof(LineBase.SelectionPen)}) {
        }
    }
}
