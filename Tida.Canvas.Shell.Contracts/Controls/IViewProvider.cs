using System;

namespace Tida.Canvas.Shell.Contracts.Controls {
    /// <summary>
    /// 视图提供器契约,避免单元测试因无法满足的条件而不能正常进行;
    /// </summary>
    public interface IViewProvider {
        /// <summary>
        /// 获取视图;
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        object GetView(string viewName);

        /// <summary>
        /// 创建视图;
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        //object CreateView(string viewName, object dataContext);
    }

    public static class ViewProvider {
        public static IViewProvider Current {
            get {
                if (IsViewProviderProvided) {
                    return _viewProvider;
                }
                throw new InvalidOperationException("ViewProvidder has not been set!");
            }
        }

        public static bool IsViewProviderProvided => _viewProvider != null;

        private static IViewProvider _viewProvider;

        public static void SetViewProvider(IViewProvider serviceProvider) {
            _viewProvider = serviceProvider;
        }

        public static object GetView(string viewName) => Current?.GetView(viewName);

        //public static object CreateView(string viewName, object dataContext) => Current?.CreateView(viewName, dataContext);
    }
}
