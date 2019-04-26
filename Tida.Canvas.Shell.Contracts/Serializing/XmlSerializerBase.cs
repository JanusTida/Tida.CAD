using System;
using System.Xml.Linq;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Shell.Contracts.Serializing {
    /// <summary>
    /// 绘制对象序列化泛型基类;
    /// </summary>
    /// <typeparam name="TDrawObject">对应的序列化器所序列化和反序列化的绘制对象类型,该类型应为绘制对象的最终类型</typeparam>
    public abstract class DrawObjectXmlSerializerBase<TDrawObject> : IDrawObjectXmlSerializer where TDrawObject:DrawObject{
        public DrawObjectXmlSerializerBase(string xElemName_DrawObject) {
            if (string.IsNullOrEmpty(xElemName_DrawObject)) {
                throw new ArgumentNullException(nameof(xElemName_DrawObject));
            }

            this._xElemName_DrawObject = xElemName_DrawObject;
        }

        /// <summary>
        /// 关联的绘制对象的xml元素名,由子类决定;
        /// </summary>
        private readonly string _xElemName_DrawObject;

        public virtual DrawObject Deserialize(XElement xElem) {
            if(xElem == null) {
                return null;
            }

            if(xElem.Name != _xElemName_DrawObject) {
                return null;
            }

            return OnDeserialize(xElem);
        }

        /// <summary>
        /// 子类进行反序列化的操作，对已经得到的xml元素进行判断;
        /// </summary>
        /// <param name="xElem"></param>
        /// <returns></returns>
        protected abstract TDrawObject OnDeserialize(XElement xElem);
        
        /// <summary>
        /// 进行序列化;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        public XElement Serialize(DrawObject drawObject) {
            if(drawObject == null) {
                throw new ArgumentNullException(nameof(drawObject));
            }

            //因为绘制对象可能存在继承关系,所以需判断最终类型是否匹配;
            if(drawObject.GetType() != typeof(TDrawObject)) {
                return null;
            }

            var tDrawObject = drawObject as TDrawObject;

            var xElem = new XElement(_xElemName_DrawObject);
            OnSerialize(tDrawObject, xElem);
            return xElem;
        }

        /// <summary>
        /// 子类进行序列化的操作，对已经创建完成的xml元素进行编辑;
        /// </summary>
        /// <param name="xElem"></param>
        /// <returns></returns>
        protected abstract void OnSerialize(TDrawObject drawObject, XElement xElement);
    }
}
