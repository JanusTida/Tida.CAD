using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Controls;
using Tida.Canvas.Shell.Contracts.StatusBar;
using Tida.Canvas.Shell.Contracts.StatusBar.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.StatusBar {
    [Export(typeof(IStatusBarService))]
    class StatusBarServiceImpl : IStatusBarService {
        [ImportingConstructor]
        public StatusBarServiceImpl(
            Views.StatusBarView statusBar,
            [ImportMany]IEnumerable<IStatusBarItem> statusBarItems
        ) {
            this._statusBarItems = statusBarItems;
            _stackGrid = StackGridFactory.CreateNew<IStatusBarItem>(statusBar.Grid);
            _stackGrid.Orientation = Orientation.Horizontal;
        }

        private readonly IEnumerable<IStatusBarItem> _statusBarItems;

        private readonly IStackGrid<IStatusBarItem> _stackGrid;

        public void Initialize() {
            

            _items.Clear();
            _stackGrid.Clear();

            foreach (var item in _statusBarItems) {
                AddStatusBarItem(item);
            }

            //添加默认状态栏项;
            //var defaultItem = CreateStatusBarTextItem(Constants.StatusBarItemDefault);
            //defaultItem.Margin = new Thickness(3, 0, 3, 0);
            //AddStatusBarItem(defaultItem, GridChildLength.Auto);


            //添加中间的空余项;
            //var indentItem = CreateStatusBarObjectItem(Constants.StatusBarItemIndent, null);
            //AddStatusBarItem(
            //    indentItem,
            //    new GridChildLength(new GridLength(1, GridUnitType.Star)),
            //    Contracts.StatusBar.Constants.StatusBarOrder_Indent
            //);

            CommonEventHelper.Publish<StatusBarInitializeEvent,IStatusBarService>(this);
            CommonEventHelper.PublishEventToHandlers<IStatusBarInitializeEventHandler,IStatusBarService>(this);

        }

        

        private readonly List<IStatusBarItem> _items = new List<IStatusBarItem>();
        public IEnumerable<IStatusBarItem> StatusBarItems => _items.Select(p => p);

        public void AddStatusBarItem(IStatusBarItem item) {
            if (item == null) {
                throw new ArgumentNullException(nameof(item));
            }

            if(_items.Any(p => p.GUID == item.GUID)) {
                return;
            }
            
            _items.Add(item);
            try {
                _stackGrid.AddChild(item, item.GridChildLength, item.Order);
            }
            catch (Exception ex) {
                LoggerService.WriteCallerLine(ex.Message);
            }
        }

        //public void Report(string text, string statusBarItemGUID = null) {
            
        //}

        public void RemoveStatusBarItem(IStatusBarItem item) {
            if (item == null) {
                throw new ArgumentNullException(nameof(item));
            }

            if (!StatusBarItems.Contains(item)) {
                LoggerService.WriteCallerLine($"{nameof(StatusBarItems)} doesn't contain the {nameof(item)}");
                return;
            }

            try {
                _items.Remove(item);
                _stackGrid.Remove(item);
            }
            catch (Exception ex) {
                LoggerService.WriteCallerLine(ex.Message);
            }
        }

        //public IStatusBarItem CreateStatusBarObjectItem(string guid,object content) => new StatusBarObjectItem(guid, content);

        //public IStatusBarTextItem CreateStatusBarTextItem(string guid) => new StatusBarTextItem(guid);

        
    }
}
