using CDO.Common.Application.Contracts.Controls;
using CDO.Common.Canvas.Shell.Contracts.DynamicInput;
using CDO.Common.Canvas.Shell.DynamicInput.Models;
using CDO.Common.Canvas.Shell.DynamicInput.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDO.Common.Canvas.Shell.DynamicInput {
    class NumberBoxContainer : INumberBoxContainer {
        public NumberBoxContainer() {
            _view = new Views.NumberBoxContainer();
            _viewModel = new NumberBoxContainerViewModel();
            _view.DataContext = _viewModel;
        }

        public IReadOnlyList<INumberBox> NumberBoxes => _viewModel.NumberBoxes;

        private readonly Views.NumberBoxContainer _view;
        private readonly NumberBoxContainerViewModel _viewModel;

        public object UIObject => _view;

        public void AddNumberBox(INumberBox numberBox) {
            if(numberBox is NumberBoxModel model) {
                _viewModel.NumberBoxes.Add(model);
            }
        }

        public void RemoveNumberBox(INumberBox numberBox) {
            if (numberBox is NumberBoxModel model) {
                _viewModel.NumberBoxes.Remove(model);

            }
        }

#if DEBUG
        ~NumberBoxContainer() {

        }
#endif
    }
}
