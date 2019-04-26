using static Tida.Canvas.Shell.DrawObjectDescription.Constants;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.DrawObjectDescription;

namespace Tida.Canvas.Shell.DrawObjectDescription {
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
