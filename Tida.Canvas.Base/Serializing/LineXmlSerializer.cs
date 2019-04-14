using System;
using System.Xml.Linq;
using Tida.Geometry.Primitives;
using Tida.Canvas.Base.DrawObjects;
using Tida.Canvas.Infrastructure.Serializing;
using Tida.Application.Contracts.Common;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Base.Serializing {
    /// <summary>
    /// 线段的序列化器;
    /// </summary>
    [Export(typeof(IDrawObjectXmlSerializer))]
    public class LineXmlSerializer : DrawObjectXmlSerializerBase<Line> {
        public LineXmlSerializer():base(XElemName_Line) {

        }

        private const string XElemName_Line = "line";

        private const string XElemName_Start = "Start";
        private const string XElemName_End = "End";
        private const string XPropName_X = "X";
        private const string XPropName_Y = "Y";
        
        protected override Line OnDeserialize(XElement xElem) {
            try {
                var startElem = xElem.Element(XElemName_Start);
                var endElem = xElem.Element(XElemName_End);

                var startX = double.Parse(startElem.Attribute(XPropName_X).Value);
                var startY = double.Parse(startElem.Attribute(XPropName_Y).Value);
                var endX = double.Parse(endElem.Attribute(XPropName_X).Value);
                var endY = double.Parse(endElem.Attribute(XPropName_Y).Value);

                var line2D = new Line2D(new Vector2D(startX, startY), new Vector2D(endX, endY));

                return new Line(line2D);
            }
            catch (Exception ex) {
                LoggerService.WriteException(ex);
            }

            return null;
        }

        protected override void OnSerialize(Line line, XElement xElem) {
            var startElem = new XElement(XElemName_Start);
            var endElem = new XElement(XElemName_End);

            startElem.SetAttributeValue(XPropName_X, line.Line2D.Start.X);
            startElem.SetAttributeValue(XPropName_Y, line.Line2D.Start.Y);
            endElem.SetAttributeValue(XPropName_X, line.Line2D.End.X);
            endElem.SetAttributeValue(XPropName_Y, line.Line2D.End.Y);

            xElem.Add(startElem);
            xElem.Add(endElem);
        }
    }
}
