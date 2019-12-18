using Prism.Regions;
using System;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 根据<see cref="IRegionManager"/>所封装的区域导航相关方法;
    /// </summary>
    public static class RegionHelper {
        /// <summary>
        /// 当前的导航器实例;
        /// </summary>
        private static IRegionManager _regionManager;
        public static IRegionManager RegionManager => _regionManager ?? (_regionManager = ServiceProvider.Current.GetInstance<IRegionManager>());

        /// <summary>
        /// 请求导航;
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="source"></param>
        public static void RequestNavigate(string regionName, Uri source) => RegionManager?.RequestNavigate(regionName, source);

        /// <summary>
        /// 请求导航;
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="source"></param>
        public static void RequestNavigate(string regionName, string source) => RegionManager?.RequestNavigate(regionName, source);

        /// <summary>
        /// 注册区域与视图;将某个区域关联到某个视图;
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public static IRegionManager RegisterViewWithRegion(string regionName, Type viewType) => RegionManager?.RegisterViewWithRegion(regionName, viewType);

        public static IRegionManager RegisterViewWithRegion(string regionName, Func<object> getContentDelegate) => RegionManager?.RegisterViewWithRegion(regionName, getContentDelegate);


    }
}
