using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Shell.Contracts.DrawObjectDescription;
using static Tida.Canvas.Base.DrawObjectDescription.Constants;

namespace Tida.Canvas.Base.DrawObjectDescription {
    [ExportDrawObjectDescriptor]
    public class LineDescriptor : LanguageDrawObjectDescriptorGenericBase<Line> {
        public override string TypeLanguageKey => DrawObjectTypeName_Line;
    }

    [ExportDrawObjectDescriptor]
    public class EllipseDescriptor : LanguageDrawObjectDescriptorGenericBase<Ellipse> {
        public override string TypeLanguageKey => DrawObjectTypeName_Ellipse;
    }

    [ExportDrawObjectDescriptor]
    public class RectangleDescriptor : LanguageDrawObjectDescriptorGenericBase<Rectangle> {
        public override string TypeLanguageKey => DrawObjectTypeName_Rectangle;
    }

    [ExportDrawObjectDescriptor]
    public class PointDescriptor : LanguageDrawObjectDescriptorGenericBase<Point> {
        public override string TypeLanguageKey => DrawObjectTypeName_Point;
    }
}
