using Tida.Canvas.Shell.Contracts.Common;
using Tida.Canvas.Shell.Contracts.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.Controls {
    [Export(typeof(IStackGridFactory))]
    class StackGridFactoryImpl : IStackGridFactory {
        public IStackGrid<TStackItem> CreateNew<TStackItem>(Grid grid = null) where TStackItem : class,IUIObjectProvider {
            return new StackGrid<TStackItem>(grid) {
                SplitterLength = Constants.SpliterLength_Default
            };
        }
    }
}
