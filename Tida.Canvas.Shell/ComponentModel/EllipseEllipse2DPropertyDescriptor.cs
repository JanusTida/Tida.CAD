using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tida.Canvas.Shell.ComponentModel.Constants;
using static Tida.Canvas.Shell.Contracts.ComponentModel.Constants;

namespace Tida.Canvas.Shell.ComponentModel {
    [ExportPropertyDescriptor(
        Inheritable = true,
        CategoryNameKey = CategoryName_Geometry,
        DescriptionNameKey = DescriptionName_Ellipse2D,
        DisplayNameKey = DisplayName_Ellipse2D, 
        IsReadOnly = false ,
        EditorTypeGUID = EditorType_Ellipse2D,
        EditorStyle = EditorStyle.None,
        EditorTargetProperty = EditorTargetProperty_Ellipse2D
    )]
    class EllipseEllipse2DPropertyDescriptor : PropertyDescriptor {
        public EllipseEllipse2DPropertyDescriptor() : base(typeof(Ellipse), nameof(Ellipse.Ellipse2D)) {
        }
    }
}
