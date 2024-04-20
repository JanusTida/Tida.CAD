
using Tida.Canvas.Shell.Canvas.Models;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas.ViewModels {
    /// <summary>
    /// 图层选择的状态栏控制视图模型;
    /// </summary>
    [Export]
    class CanvasLayersStatusBarItemViewModel:BindableBase {
        public CanvasLayersStatusBarItemViewModel() {
            CommonEventHelper.GetEvent<CanvasActiveLayerChangedEvent>().Subscribe(CanvasViewModel_ActiveLayerChanged);
            CanvasService.CanvasDataContext.Layers.CollectionChanged += Layers_CollectionChanged;
            Initialize();
        }

        private void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            Initialize();
            RaisePropertyChanged(nameof(ActiveLayerModel));
        }

        private void Initialize() {
            foreach (var layerModel in LayerModels) {
                layerModel.Dispose();
            }

            LayerModels.Clear();
            LayerModels.AddRange(CanvasService.CanvasDataContext.Layers.Select(p => new CanvasLayerModel(p)));
        }
        private void CanvasViewModel_ActiveLayerChanged(CanvasActiveLayerChangedEventArgs args) {
            RaisePropertyChanged(nameof(ActiveLayerModel));
        }

        public ObservableCollection<CanvasLayerModel> LayerModels { get; } = new ObservableCollection<CanvasLayerModel>();

        public CanvasLayerModel ActiveLayerModel {
            get { return LayerModels.FirstOrDefault(p => p.CanvasLayer == CanvasService.CanvasDataContext.ActiveLayer); }
            set {
                if(value == null) {
                    return;
                }

                CanvasService.CanvasDataContext.ActiveLayer = value.CanvasLayer;
            }
        }
    }
}
