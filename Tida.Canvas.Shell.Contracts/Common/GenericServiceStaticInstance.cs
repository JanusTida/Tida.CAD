namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 服务提供者静态实例提供器,本类将ServiceProvider中的实例存储在静态实例以减少持续使用时的查找实例时间;
    /// </summary>
    public abstract class GenericServiceStaticInstance<TService> where TService : class {
        private static TService _current;
        /// <summary>
        /// 对应服务的静态实例;
        /// </summary>
        public static TService Current => _current ?? (_current = ServiceProvider.GetInstance<TService>());
    }
}
