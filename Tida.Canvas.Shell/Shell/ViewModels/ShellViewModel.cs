using Prism.Mvvm;
using System.ComponentModel.Composition;
using Tida.Application.Contracts.App;
using Prism.Commands;

namespace Tida.Canvas.Shell.Shell.ViewModels {
    /// <summary>
    /// 主视图模型;
    /// </summary>
    [Export]
    public partial class ShellViewModel : BindableBase {
        [ImportingConstructor]
        public ShellViewModel() {
            SetTitle(null);
        }
        
        /// <summary>
        /// 标题名后缀;
        /// </summary>
        private string _brandName;
        private string BrandName => _brandName??(_brandName = LanguageService.FindResourceString(Constants.ShellTitle));

        /// <summary>
        /// 窗体标题;
        /// </summary>
        private string _title;
        public string Title {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        
        public void SetTitle(string word,bool saveBrandName = true) {
            if (saveBrandName && !string.IsNullOrEmpty(word)) {
                Title = $"{word} - {BrandName}";
            }
            else if (word == null) {
                Title = BrandName;
            }
            else {
                Title = word;
            }
        }


        private DelegateCommand _suckCommand;
        public DelegateCommand SuckCommand => _suckCommand ??
            (_suckCommand = new DelegateCommand(
                () => {

                }
            ));

    }
    
}
