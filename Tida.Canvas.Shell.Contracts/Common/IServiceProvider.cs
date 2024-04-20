using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 服务提供者契约;
    /// </summary>
    public interface IServiceProvider {
        //
        // Summary:
        //     Get all instances of the given serviceType currently registered in the container.
        //
        // Parameters:
        //   serviceType:
        //     Type of object requested.
        //
        // Returns:
        //     A sequence of instances of the requested serviceType.
        //
        // Exceptions:
        //   T:Microsoft.Practices.ServiceLocation.ActivationException:
        //     if there is are errors resolving the service instance.
        IEnumerable<object> GetAllInstances(Type serviceType);
        //
        // Summary:
        //     Get all instances of the given TService currently registered in the container.
        //
        // Type parameters:
        //   TService:
        //     Type of object requested.
        //
        // Returns:
        //     A sequence of instances of the requested TService.
        //
        // Exceptions:
        //   T:Microsoft.Practices.ServiceLocation.ActivationException:
        //     if there is are errors resolving the service instance.
        IEnumerable<TService> GetAllInstances<TService>();
        //
        // Summary:
        //     Get an instance of the given serviceType.
        //
        // Parameters:
        //   serviceType:
        //     Type of object requested.
        //
        // Returns:
        //     The requested service instance.
        //
        // Exceptions:
        //   T:Microsoft.Practices.ServiceLocation.ActivationException:
        //     if there is an error resolving the service instance.
        object GetInstance(Type serviceType);
        //
        // Summary:
        //     Get an instance of the given named serviceType.
        //
        // Parameters:
        //   serviceType:
        //     Type of object requested.
        //
        //   key:
        //     Name the object was registered with.
        //
        // Returns:
        //     The requested service instance.
        //
        // Exceptions:
        //   T:Microsoft.Practices.ServiceLocation.ActivationException:
        //     if there is an error resolving the service instance.
        object GetInstance(Type serviceType, string key);

        //
        // Summary:
        //     Get an instance of the given TService.
        //
        // Type parameters:
        //   TService:
        //     Type of object requested.
        //
        // Returns:
        //     The requested service instance.
        //
        // Exceptions:
        //   T:Microsoft.Practices.ServiceLocation.ActivationException:
        //     if there is are errors resolving the service instance.
        TService GetInstance<TService>();

        //
        // Summary:
        //     Get an instance of the given named TService.
        //
        // Parameters:
        //   key:
        //     Name the object was registered with.
        //
        // Type parameters:
        //   TService:
        //     Type of object requested.
        //
        // Returns:
        //     The requested service instance.
        //
        // Exceptions:
        //   T:Microsoft.Practices.ServiceLocation.ActivationException:
        //     if there is are errors resolving the service instance.
        TService GetInstance<TService>(string key);

        ///// <summary>
        ///// 添加实例;
        ///// </summary>
        ///// <typeparam name="TService"></typeparam>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //TService AddInstance<TService>(string key);
    }

    /// <summary>
    /// 服务提供者实例的存储包装,,整个应用程序的服务实例提供将由此单位直接或间接提供;
    /// </summary>
    public static class ServiceProvider {
        public static IServiceProvider Current { get; private set; }

        /// <summary>
        /// 设定当前的服务实例提供者实现;
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetServiceProvider(IServiceProvider serviceProvider) {
            Current = serviceProvider;
        }

        public static TService GetInstance<TService>() where TService : class {
            if (Current == null) {
                
#if DEBUG
                return null;
#endif
                var st = new StackFrame(6);
                var sm = st.GetMethod();
                
                throw new InvalidOperationException($"ServiceProvider has not been set2!{typeof(TService)}{sm.Name}");
            }

            return Current.GetInstance<TService>();
        }

        public static IEnumerable<TService> GetAllInstances<TService>() where TService : class {
            if (Current == null) {
                throw new InvalidOperationException("ServiceProvidder has not been set!");
            }

            return Current.GetAllInstances<TService>();
        }

        public static TService GetInstance<TService>(string key) where TService : class {
            if (Current == null) {
                throw new InvalidOperationException("ServiceProvidder has not been set!");
            }

            return Current.GetInstance<TService>(key);
        }
    }
}
