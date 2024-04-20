using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Util.Common.Util;

namespace Tida.Canvas.Shell.Contracts.Setting {
    /// <summary>
    /// 设定节;
    /// </summary>
    public interface ISettingsSection : ICloneable<ISettingsSection> {
        /// <summary>
        /// 唯一标识;
        /// </summary>
        string GUID { get; }

        /// <summary>
        /// 所有设定对;
        /// </summary>
        IEnumerable<Tuple<string,string>> Attributes { get; }

        /// <summary>
        /// 设定或添加新的设定对;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        bool SetAttribute<T>(string name, T value);

        /// <summary>
        /// 返回对应键的值,若不存在则返回默认值;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetAttribute<T>(string name);

        /// <summary>
        /// 移除设定值;
        /// </summary>
        /// <param name="name"></param>
        bool RemoveAttribute(string name);
    }


    
}
