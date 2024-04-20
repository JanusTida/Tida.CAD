using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.TreeView {
    /// <summary>
    /// 树形节点工厂契约(此单位唯一);
    /// </summary>
    public interface ITreeUnitFactory {
        /// <summary>
        /// 创建一个新的树形节点;
        /// </summary>
        /// <param name="typeGUID"></param>
        /// <returns></returns>
        ITreeUnit CreateNew(string typeGUID);
    }

    public class TreeUnitFactory : GenericServiceStaticInstance<ITreeUnitFactory> {
        public static ITreeUnit CreateNew(string typeGuid) => Current?.CreateNew(typeGuid);
    }
}
