using System;
using System.Collections.Generic;
using Tida.Canvas.Shell.Contracts.Common;
using IServiceProvider = Tida.Canvas.Shell.Contracts.Common.IServiceProvider;
using CommonServiceLocator;

namespace Tida.Canvas.Shell.Common {
    /// <summary>
    /// PracticeServiceProvider,本类别连接本地IServiceLocator与Microsoft.Practices.ServiceLocation.IServiceLocator
    /// </summary>
    public class ServiceProviderWrapper : IServiceProvider {
        public ServiceProviderWrapper(IServiceLocator serviceLocator) {
            if (serviceLocator == null) {
                LoggerService.WriteCallerLine("servicelocator can't be null!");
                throw new InvalidOperationException("servicelocator can't be null!");
            }
            
            _serviceLocator = serviceLocator;
        }

        private IServiceLocator _serviceLocator;

        public IEnumerable<object> GetAllInstances(Type serviceType) => _serviceLocator.GetAllInstances(serviceType);

        public IEnumerable<TService> GetAllInstances<TService>() => _serviceLocator.GetAllInstances<TService>();

        public object GetInstance(Type serviceType) => _serviceLocator.GetInstance(serviceType);

        public object GetInstance(Type serviceType, string key) => _serviceLocator.GetInstance(serviceType, key);

        public TService GetInstance<TService>() => _serviceLocator.GetInstance<TService>();

        public TService GetInstance<TService>(string key) => _serviceLocator.GetInstance<TService>(key);

      
    }
}
