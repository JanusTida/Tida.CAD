﻿using Tida.Canvas.Shell.Contracts.ComponentModel.Views;
using Prism.Mef.Modularity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using Prism.Ioc;

namespace Tida.Canvas.Shell.ComponentModel {
    [ModuleExport(typeof(ComponentModelModule))]
    public class ComponentModelModule : IModule {
       
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //设定SelectorFactory为Telerik相关的;
            StandardValuesEditorManager.SelectorFactory = () => new RadComboBox(); 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
