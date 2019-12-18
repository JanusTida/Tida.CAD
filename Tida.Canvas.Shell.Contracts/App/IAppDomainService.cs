using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Contracts.App {
    /// <summary>
    /// 应用程序域相关公共服务,本程序须在程序入口点进行初始化;
    /// </summary>
    public interface IAppDomainService:IDisposable {
        /// <summary>
        /// 环境路径,这指示了运行进程的主入口点所在的程序集的路径;
        /// </summary>
        string EnvironmentDirectory { get; }

        /// <summary>
        /// 获取正在运行的程序集的路径;
        /// </summary>
        /// <returns></returns>
        string GetExecutingAssemblyDirectory();

        /// <summary>
        /// 获取最近一次通过<see cref="GetExecutingAssemblyDirectory"/>查询的程序集所在的路径,以提高性能;
        /// </summary>
        string ExecutingAssemblyDirectory { get; }

        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize();

        /// <summary>
        /// 向应用程序实例中添加一个资源文件;
        /// </summary>
        /// <param name="resourcePath"></param>
        void AddResourceDictionary(Uri resourcePath);

    }

    public class AppDomainService:GenericServiceStaticInstance<IAppDomainService> {
        
        /// <summary>
        /// 环境路径,这指示了运行进程的主入口点所在的程序集的路径;
        /// </summary>
        public static string EnvironmentDirectory => Current.EnvironmentDirectory;

        /// <summary>
        /// 获取正在运行的程序集的路径;
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingAssemblyDirectory() => Current.GetExecutingAssemblyDirectory();

        /// <summary>
        /// 获取最近一次通过<see cref="GetExecutingAssemblyDirectory"/>查询的程序集所在的路径,以提高性能;
        /// 若尚未查询过,内部将进行查询一次,并返回查询的结果;
        /// </summary>
        public static string ExecutingAssemblyDirectory => Current.ExecutingAssemblyDirectory;

        public static void AddResourceDictionary(Uri resoucePath) => Current.AddResourceDictionary(resoucePath);
    }
}
