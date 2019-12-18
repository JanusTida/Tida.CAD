using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 服务提供者静态实例提供器,本类将ServiceProvider中的实例存储在静态实例以减少持续使用时的查找实例时间;
    /// </summary>
    public abstract class GenericServiceStaticInstances<TService> where TService : class {
        private static IEnumerable<TService> _currents;
        public static IEnumerable<TService> Currents => _currents ?? (_currents = ServiceProvider.GetAllInstances<TService>());
    }
}
