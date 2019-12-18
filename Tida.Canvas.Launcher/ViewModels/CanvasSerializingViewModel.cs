
using Tida.Canvas.Shell.Contracts.App;

using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Shell;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tida.Canvas.Shell.Contracts.Serializing;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Launcher.ViewModels {
    /// <summary>
    /// 画布状态持久化部分的逻辑;
    /// </summary>
    [Export]
    class CanvasSerializingViewModel {
        [ImportingConstructor]
        public CanvasSerializingViewModel(
            [ImportMany]IEnumerable<IDrawObjectXmlSerializer> drawObjectXmlSerializers
        ) {
            //添加绘制对象序列化器集合;
            _serializers.AddRange(drawObjectXmlSerializers);
        }

        private ICanvasDataContext CanvasDataContext => CanvasService.CanvasDataContext;

        /// <summary>
        /// 所有绘制对象的序列化器;
        /// </summary>
        private readonly List<IDrawObjectXmlSerializer> _serializers = new List<IDrawObjectXmlSerializer>();


        /// <summary>
        /// 当前的存档位置;
        /// </summary>
        private string _currentDocFileName;

        /// <summary>
        /// 保存命令;
        /// </summary>
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand => _saveCommand ??
            (_saveCommand = new DelegateCommand(
                () => {
                    //检查当前是否加载了文件,如若否,则另存为;
                    if (string.IsNullOrEmpty(_currentDocFileName)) {
                        SaveAsCommand.Execute();
                        return;
                    }

                    SaveCurrentDoc(_currentDocFileName);
                }
            ));

        /// <summary>
        /// 另存为命令;
        /// </summary>
        private DelegateCommand _saveAsCommand;
        public DelegateCommand SaveAsCommand => _saveAsCommand ??
            (_saveAsCommand = new DelegateCommand(
                () => {
                    var fileName = DialogService.GetSaveFilePath();
                    if (string.IsNullOrEmpty(fileName)) {
                        return;
                    }

                    SaveCurrentDoc(fileName);
                    _currentDocFileName = fileName;
                    ShellService.Current.SetTitle(Path.GetFileName(fileName));
                }
            ));

        /// <summary>
        /// 打开文件命令;
        /// </summary>
        private DelegateCommand _openDocCommand;
        public DelegateCommand OpenDocCommand => _openDocCommand ??
            (_openDocCommand = new DelegateCommand(
                () => {

                    var fileName = DialogService.OpenFile();
                    if (string.IsNullOrEmpty(fileName)) {
                        return;
                    }

                    //保存已加载的文件;
                    if (_currentDocFileName != null) {
                        SaveCurrentDoc(_currentDocFileName);
                    }

                    OpenDoc(fileName);

                    _currentDocFileName = fileName;
                    ShellService.Current.SetTitle(_currentDocFileName);
                }
            ));

        /// <summary>
        /// 将当前文档保存到指定位置;
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveCurrentDoc(string fileName) {
            var xDoc = new XDocument();
            xDoc.Add(new XElement(Constants.XElemName_CanvasDataModel));

            //遍历存储图层集合;
            foreach (var layer in CanvasDataContext.Layers) {
                var layerElem = new XElement(Constants.XElemName_Layer);
                layerElem.SetAttributeValue(Constants.XPropName_LayerGUID, layer.GUID);
                layerElem.SetAttributeValue(Constants.XPropName_LayerName, layer.LayerName);

                foreach (var drawObject in layer.DrawObjects) {
                    foreach (var serializer in _serializers) {
                        var drawObjectElem = serializer.Serialize(drawObject);
                        if (drawObjectElem == null) {
                            continue;
                        }

                        //添加到图层元素中;
                        layerElem.Add(drawObjectElem);
                    }
                }

                xDoc.Root.Add(layerElem);
            }

            xDoc.Save(fileName);
        }


        private DelegateCommand _createNewCommand;
        public DelegateCommand CreateNewCommand => _createNewCommand ??
            (_createNewCommand = new DelegateCommand(
                () => {
                    if(_currentDocFileName != null) {
                        SaveCurrentDoc(_currentDocFileName);
                        _currentDocFileName = null;
                    }

                    var canvasDataContext = ServiceProvider.GetInstance<ICanvasService>()?.CanvasDataContext;
                    if (canvasDataContext == null) {
                        return;
                    }
                    canvasDataContext.ClearTransactions();
                    canvasDataContext.ResetLayers();
                    
                    ShellService.Current.SetTitle(string.Empty);
                }
            ));


        /// <summary>
        /// 指定位置的文档打开并加载;
        /// </summary>
        /// <param name="fileName"></param>
        private void OpenDoc(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException(nameof(fileName));
            }
            if (!File.Exists(fileName)) {
                throw new FileNotFoundException();
            }

            var canvasContext = CanvasService.CanvasDataContext;

            canvasContext.Layers.Clear();

            var layers = new List<CanvasLayerEx>();
            //打开文档,遍历其中的图层元素;
            var xDoc = XDocument.Load(fileName);
            var layerElems = xDoc.Root.Elements(Constants.XElemName_Layer);

            foreach (var layerElem in layerElems) {

                var layer = new CanvasLayerEx(layerElem.Attribute(Constants.XPropName_LayerGUID)?.Value) {
                    LayerName = layerElem.Attribute(Constants.XPropName_LayerName)?.Value
                };

                //遍历图层内的绘制对象元素,反序列化为绘制对象后添加到图层中;
                foreach (var dwElem in layerElem.Elements()) {
                    //遍历序列器;
                    foreach (var serializer in _serializers) {
                        try {
                            //反序列化,直到返回结果不为空为止;
                            var drawObject = serializer.Deserialize(dwElem);
                            if (drawObject == null) {
                                continue;
                            }
                            layer.AddDrawObject(drawObject);
                        }
                        catch (Exception ex) {
                            LoggerService.WriteException(ex);
                        }
                    }

                }


                layers.Add(layer);
            }


            CanvasDataContext.ClearTransactions();

            CanvasDataContext.Layers.AddRange(layers);
            if (layers.Count != 0) {
                CanvasDataContext.ActiveLayer = layers[0];
            }

        }
    }
}
