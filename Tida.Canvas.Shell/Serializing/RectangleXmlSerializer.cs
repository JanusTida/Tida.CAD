
using System;
using System.Xml.Linq;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.Serializing;
using Tida.Application.Contracts.Common;
using System.ComponentModel.Composition;
using Tida.Xml;

namespace Tida.Canvas.Shell.Serializing {
    /// <summary>
    /// 矩形的序列化器;
    /// </summary>
    [Export(typeof(IDrawObjectXmlSerializer))]
    public class RectangleXmlSerializer : DrawObjectXmlSerializerBase<Rectangle> {
        public RectangleXmlSerializer():base(XElemName_Rectangle) {

        }

        private const string XElemName_Rectangle = "rectangle";
        private const string XElemName_RectangleMiddleLine = "middleLine";
        private const string XElemName_RectangleWidth = "width";
        private const string XElemName_RectangleMiddleLineStart = "start";
        private const string XElemName_RectangleMiddleLineEnd = "end";
        private const string XPropName_X = "x";
        private const string XPropName_Y = "y";
        
        protected override Rectangle OnDeserialize(XElement xElem) {
            try {
                var middleLineElem = xElem.Element(XElemName_RectangleMiddleLine);
                var startElem = middleLineElem.Element(XElemName_RectangleMiddleLineStart);
                var endElem = middleLineElem.Element(XElemName_RectangleMiddleLineEnd);

                var startX = double.Parse(startElem.Attribute(XPropName_X).Value);
                var startY = double.Parse(startElem.Attribute(XPropName_Y).Value);

                var endX = double.Parse(endElem.Attribute(XPropName_X).Value);
                var endY = double.Parse(endElem.Attribute(XPropName_Y).Value);

                var width = double.Parse(xElem.GetXElemValue(XElemName_RectangleWidth));

                return new Rectangle(new Rectangle2D2(
                    new Line2D(
                        new Vector2D(startX,startY),
                        new Vector2D(endX,endY)
                    ),
                    width
                ));
            }
            catch(Exception ex) {
                LoggerService.WriteException(ex);
            }

            return null;
        }

        protected override void OnSerialize(Rectangle rectangle, XElement xElement) {
            if(rectangle == null) {
                throw new ArgumentNullException(nameof(rectangle));
            }
            if(xElement == null) {
                return;
            }

            if(rectangle.Rectangle2D == null) {
                return;
            }

            if (rectangle.Rectangle2D.MiddleLine2D == null) {
                return;
            }

            var middleLineElem = new XElement(XElemName_RectangleMiddleLine);
            var startElem = new XElement(XElemName_RectangleMiddleLineStart);
            var endElem = new XElement(XElemName_RectangleMiddleLineEnd);

            startElem.SetAttributeValue(XPropName_X, rectangle.Rectangle2D.MiddleLine2D.Start.X);
            startElem.SetAttributeValue(XPropName_Y, rectangle.Rectangle2D.MiddleLine2D.Start.Y);

            endElem.SetAttributeValue(XPropName_X, rectangle.Rectangle2D.MiddleLine2D.End.X);
            endElem.SetAttributeValue(XPropName_Y, rectangle.Rectangle2D.MiddleLine2D.End.Y);

            middleLineElem.Add(startElem);
            middleLineElem.Add(endElem);

            xElement.SetXElemValue(rectangle.Rectangle2D.Width, XElemName_RectangleWidth);

            xElement.Add(middleLineElem);
        }
    }
}
