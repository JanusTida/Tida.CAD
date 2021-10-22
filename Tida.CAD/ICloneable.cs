using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD {
    /// <summary>
    /// 可拷贝契约;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<T> {
        /// <summary>
        /// 拷贝为指定类型;
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}
