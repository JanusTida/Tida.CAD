using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.App.Input;
using Tida.Canvas.Shell.Contracts.Common;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.App.ViewModels {
    /// <summary>
    /// 输入值对话框窗体模型;
    /// </summary>
    class InputValueWindowViewModel:BindableBase {
        public InputValueWindowViewModel(IInputChecker inputChecker = null) {
            this._inputChecker = inputChecker;
        }
        private readonly IInputChecker _inputChecker;
        private string _title;
        /// <summary>
        /// 对话框标题;
        /// </summary>
        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }



        private string _val;
        public string Val {
            get { return _val; }
            set { SetProperty(ref _val, value); }
        }


        private string _desc;
        public string Desc {
            get { return _desc; }
            set { SetProperty(ref _desc, value); }
        }


        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand => _confirmCommand ??
            (_confirmCommand = new DelegateCommand(
                () => {
                    if(_inputChecker != null) {
                        try {
                            var res = _inputChecker.Check(Val);
                            if(res == null) {
                                throw new InvalidOperationException($"{nameof(res)} can not be null.");
                            }

                            if (!res.IsValid) {
                                MsgBoxService.Show(res.ErrorMessage);
                                return;
                            }

                            
                        }
                        catch(Exception ex) {
                            LoggerService.WriteException(ex);
                            MsgBoxService.ShowError(ex.Message);

                            return;
                        }
                    }

                    Confirmed = true;
                    CloseRequest.Raise(new Notification());
                }
            ));


        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand => _cancelCommand ??
            (_cancelCommand = new DelegateCommand(
                () => {
                    Confirmed = false;
                    CloseRequest.Raise(new Notification());
                }
            ));


        /// <summary>
        /// 是否确认了;
        /// </summary>
        public bool Confirmed { get; private set; }

        public InteractionRequest<INotification> CloseRequest { get; } = new InteractionRequest<INotification>();
    }
}
