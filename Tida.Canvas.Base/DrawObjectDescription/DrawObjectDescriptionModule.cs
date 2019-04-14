using Prism.Mef.Modularity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Tida.Canvas.Infrastructure.ComponentModel;
using Tida.Canvas.Shell.Contracts.DrawObjectDescription;

namespace Tida.Canvas.Base.DrawObjectDescription {
    [ModuleExport(typeof(DrawObjectDescriptionModule))]
    class DrawObjectDescriptionModule : IModule {
        [ImportingConstructor]
        public DrawObjectDescriptionModule([ImportMany]IEnumerable<Lazy<IDrawObjectDescriptor, IDrawObjectDescriptorMetaData>> mefDrawObjectDescriptors) {
            DrawObjectDescriptionUtil.DrawObjectDescriptors.AddRange(mefDrawObjectDescriptors.OrderBy(p => p.Metadata.Order).Select(p => p.Value));
        }

        public void Initialize() {

        }
    }
}
