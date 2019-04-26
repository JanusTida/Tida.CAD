using Tida.Canvas.Shell.Dialogs.Models;
using Tida.Application.Contracts.App;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Tida.Canvas.Shell.Dialogs.ViewModels {
    class DrawObjectSelectWindowViewModel:BindableBase {
        /// <summary>
        /// 所有绘制对象模型;
        /// </summary>
        public ObservableCollection<DrawObjectModel> DrawObjectModels { get; } = new ObservableCollection<DrawObjectModel>();


        private DrawObjectModel _selectedDrawObjectModel;
        public DrawObjectModel SelectedDrawObjectModel {
            get { return _selectedDrawObjectModel; }
            set {
                if(_selectedDrawObjectModel != null) {
                    _selectedDrawObjectModel.DrawObject.IsSelected = false;
                }

                if (value != null) {
                    value.DrawObject.IsSelected = true;
                }

                SetProperty(ref _selectedDrawObjectModel, value);
                
            }
        }


        public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();

        /// <summary>
        /// 对话框结果;
        /// </summary>
        public bool DialogResult { get; private set; }

        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand {
            get {
                if(_confirmCommand == null) {
                    _confirmCommand = new DelegateCommand(
                        () => {
                            if(SelectedDrawObjectModel == null) {

                                MsgBoxService.Show(LanguageService.FindResourceString(Constants.MsgText_NoDrawObjectSelected));

                                return;
                            }

                            DialogResult = true;
                            CloseRequest?.Raise(new Notification());
                        },
                        () => SelectedDrawObjectModel != null
                    );
                    _confirmCommand.ObservesProperty(() => SelectedDrawObjectModel);
                }

                return _confirmCommand;
            }
        }


        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??
            (_cancelCommand = new DelegateCommand(
                () => {
                    SelectedDrawObjectModel = null;
                    CloseRequest.Raise(new Notification());
                }
            ));

#if DEBUG
        ~DrawObjectSelectWindowViewModel() {

        }
#endif
    }
}
