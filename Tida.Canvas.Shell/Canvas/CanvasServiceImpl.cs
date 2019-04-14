using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Canvas.ViewModels;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using static Tida.Canvas.Shell.Canvas.Constants;

namespace Tida.Canvas.Shell.Canvas {
    [Export(typeof(ICanvasService))]
    class CanvasServiceImpl : ICanvasService {
        [ImportingConstructor]
        public CanvasServiceImpl(CanvasViewModel canvasViewModel,Views.Canvas canvas) {
            _canvasViewModel = canvasViewModel;
            _canvas = canvas;
            _canvasViewModel.EditTransactionCommited += _canvasViewModel_EditTransactionCommited;
        }

      
        private readonly CanvasViewModel _canvasViewModel;
        private readonly Views.Canvas _canvas;
        public ICanvasDataContext CanvasDataContext => _canvasViewModel;
        
        public void Initialize() {
            try {
                CommonEventHelper.Publish<CanvasDataContextInitializingEvent, ICanvasDataContext>(CanvasDataContext);
                CommonEventHelper.PublishEventToHandlers<ICanvasDataContextInitializingEventHandler, ICanvasDataContext>(CanvasDataContext);
            }
            catch(Exception ex) {
                LoggerService.WriteException(ex);
            }
        }
        
        private void _canvasViewModel_EditTransactionCommited(object sender, IEditTransaction e) {
            _canvas.CanvasControl.CommitTransaction(e);
        }



    }
}
