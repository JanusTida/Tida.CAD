using Tida.Application.Contracts.Common;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Tida.Canvas.Shell.EditTools.Ribbon {
    [ExportRibbonItemsProvider(Order = 1)]
    class EditToolRibbonItemsProvider : IRibbonItemsProvider {
        [ImportingConstructor]
        public EditToolRibbonItemsProvider(
            [ImportMany]IEnumerable<Lazy<IEditToolProvider, IEditToolProviderMetaData>> mefEditToolProviders,
            [ImportMany]IEnumerable<IEditToolGroup> mefEditToolGroups
        ) {
            _mefEditToolProviders = mefEditToolProviders;
            _mefEditToolGroups = mefEditToolGroups;
        }

        private readonly IEnumerable<Lazy<IEditToolProvider, IEditToolProviderMetaData>> _mefEditToolProviders;
        private readonly IEnumerable<IEditToolGroup> _mefEditToolGroups;
        
        private List<CreatedRibbonItem> _items;
        public IEnumerable<CreatedRibbonItem> Items {
            get {
                if(_items == null) {
                    InitializeEditTools();
                }
                return _items;
            }
        }
        
        private void InitializeEditTools() {
            _items = new List<CreatedRibbonItem>();
            
            foreach (var editToolProvider in _mefEditToolProviders.OrderBy(p => p.Metadata.Order)) {
                if(editToolProvider.Metadata == null) {
                    continue;
                }

                var createCommand = new DelegateCommand(
                    () => CanvasService.CanvasDataContext.CurrentEditTool = editToolProvider.Value.CreateEditTool(),
                    () => editToolProvider.Value.CanCreate
                );

                editToolProvider.Value.CanCreateChanged += (sender, e) => {
                    createCommand.RaiseCanExecuteChanged();
                };
                
                var ribbonBtnItem = new RibbonButtonItem {
                    Command = createCommand,
                    Icon = editToolProvider.Metadata.IconResource,
                    GUID = editToolProvider.Metadata.GUID,
                    GroupGUID = editToolProvider.Metadata.GroupGUID,
                    HeaderLanguageKey = editToolProvider.Metadata.EditToolLanguageKey,
                    Order = editToolProvider.Metadata.Order
                };

                var createdRibbonItem = new CreatedRibbonItem(
                    ribbonBtnItem, 
                    new ExportRibbonItemAttribute {
                        GUID = editToolProvider.Metadata.GUID,
                        GroupGUID = editToolProvider.Metadata.GroupGUID
                    }
                );

                _items.Add(createdRibbonItem);
            }

        }

       
    }
}
