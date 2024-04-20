using Tida.Canvas.Shell.Contracts.DrawObjectDescription;
using Prism.Mef.Modularity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Prism.Ioc;

namespace Tida.Canvas.Shell.DrawObjectDescription {
    [ModuleExport(typeof(DrawObjectDescriptionModule))]
    class DrawObjectDescriptionModule : IModule {
        [ImportingConstructor]
        public DrawObjectDescriptionModule([ImportMany]IEnumerable<Lazy<IDrawObjectDescriptor, IDrawObjectDescriptorMetaData>> mefDrawObjectDescriptors) {
            DrawObjectDescriptionUtil.DrawObjectDescriptors.AddRange(mefDrawObjectDescriptors.OrderBy(p => p.Metadata.Order).Select(p => p.Value));
        }

       
        public void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
