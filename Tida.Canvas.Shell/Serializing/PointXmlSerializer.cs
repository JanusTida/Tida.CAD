using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Serializing;
using Tida.Geometry.Primitives;


namespace Tida.Canvas.Shell.Serializing {
    /// <summary>
    /// 点的序列化器;
    /// </summary>
    [Export(typeof(IDrawObjectXmlSerializer))]
    public class PointXmlSerializer : DrawObjectXmlSerializerBase<Point> {
        public PointXmlSerializer():base(XElemName_Point) {

        }
        private const string XElemName_Point = "point";
        private const string XElemName_Position = "position";
        private const string XPropName_X = "X";
        private const string XPropName_Y = "Y";
        
        protected override Point OnDeserialize(XElement xElem) {
            if (xElem.Name != XElemName_Point) {
                return null;
            }

            try {
                var positionElem = xElem.Element(XElemName_Position);

                var x = double.Parse(positionElem.Attribute(XPropName_X).Value);
                var y = double.Parse(positionElem.Attribute(XPropName_Y).Value);

                return new Point(new Vector2D(x,y));
            }
            catch(Exception ex) {
                LoggerService.WriteException(ex);
            }

            return null;
        }

        protected override void OnSerialize(Point point,XElement xElem) {
            var positionElem = new XElement(XElemName_Position);
            positionElem.SetAttributeValue(XPropName_X, point.Position.X);
            positionElem.SetAttributeValue(XPropName_Y, point.Position.Y);

            xElem.Add(positionElem);
        }
    }
}
