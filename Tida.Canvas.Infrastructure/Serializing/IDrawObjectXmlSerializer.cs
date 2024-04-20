using System.Xml.Linq;
using Tida.Canvas.Contracts;

namespace Tida.Canvas.Infrastructure.Serializing {
    /// <summary>
    /// 绘制对象的XML持久化序列器;
    /// </summary>
    public interface IDrawObjectXmlSerializer {
        /// <summary>
        /// 所针对的绘制对象的GUID;
        /// </summary>
        //string DrawObjectGUID { get; }

        /// <summary>
        /// 将某个满足条件的绘制对象序列化为<see cref="XElement"/>;
        /// </summary>
        /// <param name="drawObject"></param>
        /// <returns></returns>
        XElement Serialize(DrawObject drawObject);


        /// <summary>
        /// 将某个满足条件的XElement反序列化为绘制对象;
        /// </summary>
        /// <param name="xElem"></param>
        /// <returns></returns>
        DrawObject Deserialize(XElement xElem);
    }
}
