using Aspose.CAD.FileFormats.Cad;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tida.Canvas.Shell.Contracts.App;

using Tida.Canvas.Shell.Contracts.Menu;
using Tida.Canvas.Shell.Contracts.Canvas;
using static Tida.Canvas.Shell.Contracts.Ribbon.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.DWG.Ribbon {
    [ExportMenuItem(
        GUID = Constants.MenuItemGUID_OpenDWGDoc,
        HeaderLanguageKey = Constants.MenuItemName_OpenDWGDoc,
        Icon = Constants.MenuItemIcon_OpenDWGDoc,
        InputGestureText = "Ctrl + N",
        OwnerGUID = Menu_CanvasShellRibbon,
        Order = 4
    )]
    class OpenDWGMenuItem : IMenuItem {
        [ImportingConstructor]
        public OpenDWGMenuItem([ImportMany]IEnumerable<ICADBaseToDrawObjectConverter> cadBaseToDrawObjectConverters) {
            this._cadBaseToDrawObjectConverters = cadBaseToDrawObjectConverters.ToArray();
        }
        private readonly ICADBaseToDrawObjectConverter[] _cadBaseToDrawObjectConverters;

        public ICommand Command => _openDWGCommand ?? (_openDWGCommand = new DelegateCommand(OpenDWG));
        private DelegateCommand _openDWGCommand;
        private void OpenDWG() {
            var fileName = DialogService.Current.OpenFile();
            if (string.IsNullOrEmpty(fileName)) {
                return;
            }

            try {
                if (!(CadImage.Load(fileName) is CadImage cadImg)) {
                    return;
                }
                
                CanvasService.CanvasDataContext.Layers.Clear();
                
                var layers = cadImg.Layers.GetLayersNames().
                    Select(p => new CanvasLayerEx(Guid.NewGuid().ToString()) { LayerName = p }).ToArray();

                if(layers.Length == 0) {
                    MsgBoxService.ShowLanguageString(Constants.MsgText_LayersEmpty);
                    return;
                }

                foreach (var layer in layers) {
                    CanvasService.CanvasDataContext.Layers.Add(layer);
                }
                
                for (int i = 0; i < cadImg.Entities.Length; i++) {
                    var entity = cadImg.Entities[i];

                    var drawObject = _cadBaseToDrawObjectConverters.Select(p => p.Convert(entity)).
                        FirstOrDefault(p => p != null);

                    //Not supported.
                    if (drawObject == null) {
                        continue;
                    }

                    var layer = layers.FirstOrDefault(p => p.LayerName == entity.LayerName);
                    if(layer == null) {
                        LoggerService.WriteCallerLine($"{nameof(layer)} can not be null.");
                        continue;
                    }

                    layer.AddDrawObject(drawObject);
                }
            }
            catch(Exception ex) {
                LoggerService.WriteException(ex);
                MsgBoxService.Show(ex.Message);
            }
        }
    }
}
