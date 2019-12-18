using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 可从实体外通知属性变更,这对于属性为非CLR属性(比如继承自ICustomTypeDescripter的类型)而言,是有用的;
    /// </summary>
    public interface ICustomNotify {
        /// <summary>
        /// 通知外界某个属性已经发生更改;
        /// </summary>
        /// <param name="propName"></param>
        void NotifyProperty(string propName);
    }
}
