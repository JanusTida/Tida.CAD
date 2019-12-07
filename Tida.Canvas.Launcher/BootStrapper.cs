using Microsoft.Practices.ServiceLocation;
using Prism.Mef;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tida.Application.Common;
using Tida.Application.Contracts.App;
using Tida.Application.Contracts.App.Events;
using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Controls;
using Tida.Application.Contracts.Setting;
using Tida.Application.Controls;

namespace Tida.Canvas.Launcher
{
    class BootStrapper : MefBootstrapper
    {

        

        /// <summary>

        /// 模块正在初始化事件;

        /// </summary>

        public event EventHandler ModulesInitializing;



        /// <summary>

        /// 模块初始化完毕事件;

        /// </summary>

        public event EventHandler ModulesInitialized;



        private Assembly[] _assemblies;



        protected override void ConfigureAggregateCatalog()
        {

            base.ConfigureAggregateCatalog();

            //主框架模块;

            
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Tida.Application.Dummy).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Tida.Canvas.Shell.Dummy).Assembly));
            this.AggregateCatalog.Catalogs.Add(new DirectoryCatalog("Plugins"));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Dummy).Assembly));
            //附加模块;

            if (_assemblies != null)
            {

                foreach (var asm in _assemblies)
                {

                    this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(asm));

                }

            }

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {

                var viewSpace = viewType.Namespace;

                var viewAssemblyName = viewType.GetTypeInfo().Assembly;

                var viewName = viewType.Name;

                try
                {

                    var lowerSpace = viewSpace.Substring(0, viewSpace.LastIndexOf("Views"));

                    var viewModelName = $"{lowerSpace}ViewModels.{viewName}ViewModel,{viewAssemblyName}";

                    return Type.GetType(viewModelName);

                }

                catch
                {

                    return null;

                }

            });

        }



        protected override IModuleCatalog CreateModuleCatalog()
        {

            return new ConfigurationModuleCatalog();

        }



        protected override void InitializeModules()
        {

            ServiceProvider.SetServiceProvider(new ServiceProviderWrapper(ServiceLocator.Current));

            ViewProvider.SetViewProvider(new ViewProviderImpl(ServiceProvider.Current));



            //应用程序域服务初始化;

            AppDomainService.Current.Initialize();

            //因为各个模块都可能用到语言服务,必须先初始化语言服务;

            LanguageService.Current.Initialize();







            //初始化设定服务;

            SettingsService.Current.Initialize();



            base.InitializeModules();

            ModulesInitializing?.Invoke(this, EventArgs.Empty);



            CommonEventHelper.GetEvent<ApplicationStartUpEvent>().Publish();

            CommonEventHelper.PublishEventToHandlers<IApplicationStartUpEventHandler>();





        }

    }
}
