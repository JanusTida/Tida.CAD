
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.ComponentModel;
using static Tida.Canvas.Shell.ComponentModel.Constants;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;


namespace Tida.Canvas.Shell.ComponentModel {
    [ExportPropertyDescriptor(Inheritable = true,CategoryNameKey = CategoryName_Geometry,DescriptionNameKey = DescriptionName_PointPosition,DisplayNameKey = DisplayName_PointPosition,IsReadOnly = false,EditorTypeGUID = EditorType_Vector2D,EditorStyle = EditorStyle.DropDown,EditorTargetProperty = EditorTargetProperty_Vector2D)]
    class PositionPropertyDescriptor : PropertyDescriptor {
        public PositionPropertyDescriptor() : base(typeof(PointBase), nameof(PointBase.Position)) {
        }
    }

    [ExportPropertyDescriptor(Inheritable = true, CategoryNameKey = CategoryName_Appearance, DescriptionNameKey = DescriptionName_ScreenRadius, DisplayNameKey = DisplayName_ScreenRadius, IsReadOnly = true)]
    class ScreenRadiusPropertyDescriptor : PropertyDescriptor {
        public ScreenRadiusPropertyDescriptor() : base(typeof(PointBase), nameof(PointBase.ScreenRadius)) {
        }
    }

    [ExportIgnoredPropertyDescriptor(Inheritable = true)]
    class PointIgnoredPropertieDescriptor : IgnoredPropertyDescriptor {
        public PointIgnoredPropertieDescriptor() : base(
            typeof(PointBase),
            nameof(PointBase.SelectedBackground),
            nameof(PointBase.NormalBackground)
        ) {

        }
    }
}
