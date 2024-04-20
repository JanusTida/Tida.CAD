
using Tida.Canvas.Contracts;
using Tida.Canvas.Shell.Canvas.ViewModels;
using Tida.Canvas.Shell.Contracts.Canvas;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Canvas {
    [Export(typeof(ICanvasService))]
    class CanvasServiceImpl : ICanvasService {
        [ImportingConstructor]
        public CanvasServiceImpl(CanvasPresenterViewModel canvasViewModel,Views.CanvasPresenter canvas) {
            _canvasViewModel = canvasViewModel;
            _canvas = canvas;
            _canvasViewModel.EditTransactionCommited += _canvasViewModel_EditTransactionCommited;
        }

      
        private readonly CanvasPresenterViewModel _canvasViewModel;
        private readonly Views.CanvasPresenter _canvas;
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
