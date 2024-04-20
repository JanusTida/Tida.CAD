using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// UI元素提供者;
    /// </summary>
    public interface IUIObjectProvider {
        /// <summary>
        /// UI元素;
        /// </summary>
        object UIObject { get; }
    }

    /// <summary>
    /// UI元素提供者工厂;
    /// </summary>
    public interface IUIObjectProviderFactory {
        /// <summary>
        /// 根据一个UI元素创建一个UI元素提供者;
        /// </summary>
        /// <param name="uiObject"></param>
        /// <returns></returns>
        IUIObjectProvider CreateNew(object uiObject);
    }

    public class UIObjectProviderFactory : GenericServiceStaticInstance<IUIObjectProviderFactory> {
        public static IUIObjectProvider CreateNew(object uiObject) => Current.CreateNew(uiObject);
    }
}
