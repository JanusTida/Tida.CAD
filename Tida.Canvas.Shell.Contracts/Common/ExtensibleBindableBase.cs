using Tida.Extending;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 可拓展绑定基类;
    /// </summary>
    public abstract class ExtensibleBindableBase: BindableBase,IExtensible {
        private ExtensibleObject _extensibleBase = new ExtensibleObject();

        public TInstance GetGeneralInstance<TInstance>(string extName) => _extensibleBase.GetGeneralInstance<TInstance>(extName);

        public TInstance GetInstance<TInstance>(string extName) => _extensibleBase.GetInstance<TInstance>(extName);

        public void SetInstance<TInstance>(TInstance instance, string extName) => _extensibleBase.SetInstance(instance, extName);

        public void RemoveInstance<TInstance>(string extName) => _extensibleBase.RemoveInstance<TInstance>(extName);
    }
}
