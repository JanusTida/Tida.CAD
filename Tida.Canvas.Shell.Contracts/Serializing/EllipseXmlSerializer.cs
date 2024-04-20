using System;
using System.Xml.Linq;
using Tida.Geometry.Primitives;
using Tida.Canvas.Infrastructure.DrawObjects;

using Tida.Xml;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Serializing {
    /// <summary>
    /// 绘制对象——椭圆(圆)的序列化器;
    /// </summary>
    public class EllipseXmlSerializer : DrawObjectXmlSerializerBase<Ellipse> {
        public EllipseXmlSerializer():base(XElemName_Ellipse) {

        }

        private const string XElemName_Ellipse = "ellipse";
        private const string XElemName_Center = "center";
        private const string XPropName_X = "X";
        private const string XPropName_Y = "Y";
        private const string XElemName_RadiusX = "radiusX";
        private const string XElemName_RadiusY = "radiusY";
        

        protected override Ellipse OnDeserialize(XElement xElem) {
            if (xElem == null) {
                throw new ArgumentNullException(nameof(xElem));
            }

            if (xElem.Name != XElemName_Ellipse) {
                return null;
            }

            try {
                var radiusX = double.Parse(xElem.GetXElemValue(XElemName_RadiusX));
                var radiusY = double.Parse(xElem.GetXElemValue(XElemName_RadiusY));

                var centerElem = xElem.Element(XElemName_Center);
                var centerX = double.Parse(centerElem.Attribute(XPropName_X).Value);
                var centerY = double.Parse(centerElem.Attribute(XPropName_Y).Value);

                return new Ellipse(new Ellipse2D {
                    RadiusX = radiusX,
                    RadiusY = radiusY,
                    Center = new Vector2D {
                        X = centerX,
                        Y = centerY
                    }
                });
            }
            catch(Exception ex) {
                LoggerService.WriteException(ex);
            }

            return null;
        }

        protected override void OnSerialize(Ellipse ellipse,XElement xElem) {
            if(ellipse == null) {
                return;
            }
            
            var centerElem = new XElement(XElemName_Center);

            if(ellipse.Ellipse2D != null) {
                xElem.SetXElemValue(ellipse.Ellipse2D.RadiusX,XElemName_RadiusX);
                xElem.SetXElemValue(ellipse.Ellipse2D.RadiusY,XElemName_RadiusY);

                if (ellipse.Ellipse2D.Center != null) {
                    centerElem.SetAttributeValue(XPropName_X, ellipse.Ellipse2D.Center.X);
                    centerElem.SetAttributeValue(XPropName_Y, ellipse.Ellipse2D.Center.Y);
                }    
                
            }
            xElem.Add(centerElem);
            
        }
    }
}
