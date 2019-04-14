using Tida.Application.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.ComponentModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.PropertyGrid.ViewModels {
    [Export]
    class PropertyGridViewModel:BindableBase {
        [ImportingConstructor]
        public PropertyGridViewModel(MefPropertyGridManager mefPropertyManager,
            [ImportMany]IEnumerable<Lazy<IObjectTypeDescriptor, IObjectTypeDescriptorMetaData>> mefObjectTypeDescriptors
            ) {

            _mefPropertyManager = mefPropertyManager;
            _mefObjectTypeDescriptors = mefObjectTypeDescriptors.Select(p => new CreatedObjectTypeDescriptor(p.Value, p.Metadata)).ToArray();

            CommonEventHelper.GetEvent<CanvasDrawObjectIsSelectedChangedEvent>().Subscribe(DrawObject_IsSelectedChanged);
            CommonEventHelper.GetEvent<CanvasDrawObjectsAddedEvent>().Subscribe(DrawObjects_Added);
            CommonEventHelper.GetEvent<CanvasDrawObjectsRemovedEvent>().Subscribe(DrawObjects_Removed);

        }

        private readonly CreatedObjectTypeDescriptor[] _mefObjectTypeDescriptors;
        private void DrawObjects_Removed(CanvasDrawObjectsRemovedEventArgs obj) => RefreshPropertyItem();
        private void DrawObjects_Added(CanvasDrawObjectsAddedEventArgs obj) => RefreshPropertyItem();
        private void DrawObject_IsSelectedChanged(CanvasDrawObjectSelectedChangedEventArgs obj) => RefreshPropertyItem();

        /// <summary>
        /// 刷新属性项;
        /// </summary>
        private void RefreshPropertyItem() {
            var allSelectedDrawObjects = CanvasService.CanvasDataContext.GetAllDrawObjects().Where(p => p.IsSelected);
            var firstDrawObject = allSelectedDrawObjects.FirstOrDefault();
            var secondDrawOject = allSelectedDrawObjects.Skip(1).FirstOrDefault();

            Item = null;
            if(firstDrawObject == null || secondDrawOject != null) {
                return;
            }

            Item = _mefPropertyManager.MakeObject(firstDrawObject);
        }
        
        private object _item;
        public object Item {
            get => _item;
            set => SetProperty(ref _item, value);
        }
        
        private readonly MefPropertyGridManager _mefPropertyManager;
        
    }
}
