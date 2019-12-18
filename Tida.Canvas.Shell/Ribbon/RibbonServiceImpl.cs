using Tida.Canvas.Shell.Contracts.Ribbon;
using Tida.Canvas.Shell.Contracts.Shell;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Tida.Canvas.Shell.Contracts.App;
using Telerik.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System;
using Telerik.Windows.Controls.RibbonView;
using System.Windows;
using System.Windows.Automation;

using Tida.Canvas.Shell.Contracts.Ribbon.Events;
using Tida.Canvas.Shell.Contracts.Menu;
using static Tida.Canvas.Shell.Contracts.Ribbon.Constants;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Ribbon {
    /// <summary>
    /// Ribbon服务实现;
    /// </summary>
    [Export(typeof(IRibbonService))]
    class RibbonServiceImpl : IRibbonService {
        [ImportingConstructor]
        public RibbonServiceImpl(
            Views.Ribbon ribbon,
            [ImportMany]IEnumerable<Lazy<IRibbonTab,IRibbonTabMetaData>> ribbonTabs,

            [ImportMany]IEnumerable<Lazy<IRibbonGroup,IRibbonGroupMetaData>> ribbonGroups,
            [ImportMany]IEnumerable<Lazy<IRibbonGroupsProvider, IRibbonGroupsProviderMetaData>> ribbonGroupsProviders,

            [ImportMany]IEnumerable<Lazy<IRibbonItem,IRibbonItemMetaData>> ribbonItems,
            [ImportMany]IEnumerable<Lazy<IRibbonItemsProvider,IRibbonItemsProviderMetaData>> ribbonItemsProvider,

            [ImportMany]IEnumerable<Lazy<IMenuItem,IMenuItemMetaData>> menuItems
        ) {

            _ribbon = ribbon;
            
            this._mefRibbonTabs = ribbonTabs.ToArray();

            this._mefRibbonGroups = ribbonGroups.ToArray();
            this._mefRibbonGroupsProviders = ribbonGroupsProviders.ToArray();

            this._mefRibbonItems = ribbonItems.ToArray();
            this._mefRibbonItemsProviders = ribbonItemsProvider.ToArray();

            this._mefMenuItems = menuItems.Where(p => p.Metadata.OwnerGUID == Menu_CanvasShellRibbon).ToArray();
        }

        
        private Views.Ribbon _ribbon;


        private readonly Lazy<IMenuItem, IMenuItemMetaData>[] _mefMenuItems;

        private readonly Lazy<IRibbonItem, IRibbonItemMetaData>[] _mefRibbonItems;

        private readonly Lazy<IRibbonItemsProvider, IRibbonItemsProviderMetaData>[] _mefRibbonItemsProviders;

        private readonly Lazy<IRibbonGroup, IRibbonGroupMetaData>[] _mefRibbonGroups;

        private readonly Lazy<IRibbonTab, IRibbonTabMetaData>[] _mefRibbonTabs;

        private readonly Lazy<IRibbonGroupsProvider, IRibbonGroupsProviderMetaData>[] _mefRibbonGroupsProviders;

        public void Initialize() {
            InitializeMenu();

            InitializeRibbonTabs();
            
        }

        /// <summary>
        /// 初始化菜单;
        /// </summary>
        private void InitializeMenu() {
            _ribbon.AppMenu.Items.Clear();
            
            //初始化顶级菜单;
            foreach (var menuItem in _mefMenuItems.OrderBy(p => p.Metadata.Order)) {
                var header = LanguageService.FindResourceString(menuItem.Metadata.HeaderLanguageKey);
                var radMenuItem = new RadMenuItem {
                    Command = menuItem.Value.Command,
                    Header = header
                };

                if(!string.IsNullOrEmpty(menuItem.Metadata.Icon)) {
                    radMenuItem.Icon = new Image {
                        Source = new BitmapImage(new Uri(menuItem.Metadata.Icon, UriKind.RelativeOrAbsolute)),
                        Width = Constants.MenuIconWidth,
                        Height = Constants.MenuIconHeight
                    };
                }

                _ribbon.AppMenu.Items.Add(radMenuItem);

                ///设定UI自动化测试相关属性;
                AutomationProperties.SetAutomationId(radMenuItem, menuItem.Metadata.GUID);
                AutomationProperties.SetName(radMenuItem, header);

                if (menuItem.Metadata.Key != Key.None) {
                    ShellService.Current.AddKeyBinding(menuItem.Value.Command, menuItem.Metadata.Key, menuItem.Metadata.ModifierKeys);
                }
            }
        }

        /// <summary>
        /// 初始化所有的RibbonTab;
        /// </summary>
        private void InitializeRibbonTabs() {
            _ribbon.RibbonMenu.Items.Clear();
            _ribbon.RibbonMenu.SelectionChanged += RibbonMenu_SelectionChanged;

            foreach (var ribbonTab in _mefRibbonTabs.OrderBy(p => p.Metadata.Order)) {
                var header = LanguageService.FindResourceString(ribbonTab.Metadata.TextLangaugeKey);
                var ribbonTabItem = new RadRibbonTab {
                    Header = header,
                    DataContext = new RibbonTabDataContext(ribbonTab.Value,ribbonTab.Metadata)
                };

                ///设定UI自动化测试相关属性;
                AutomationProperties.SetAutomationId(ribbonTabItem,ribbonTab.Metadata.GUID);
                AutomationProperties.SetName(ribbonTabItem, header);

                InitializeRibbonTab(ribbonTabItem);
                
                _ribbon.RibbonMenu.Items.Add(ribbonTabItem);
            }
        }

        /// <summary>
        /// Ribbon的Tab发生变化时激发事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RibbonMenu_SelectionChanged(object sender, RadSelectionChangedEventArgs e) {
            if(e.AddedItems.Count == 0) {
                return;
            }

            if(!((e.AddedItems[0] as RadRibbonTab)?.DataContext is RibbonTabDataContext ribbonTabDataContext)){
                return;
            }

            var args = new SelectedRibbonTabChangedEventArgs(ribbonTabDataContext.RibbonTabMetaData);
            CommonEventHelper.Publish<SelectedRibbonTabChangedEvent, SelectedRibbonTabChangedEventArgs>(args);
            CommonEventHelper.PublishEventToHandlers<ISelectedRibbonTabChangedEventHandler, SelectedRibbonTabChangedEventArgs>(args);
        }

        private void InitializeRibbonTab(RadRibbonTab ribbonTabControl) {
            var ribbonTab = ribbonTabControl.DataContext as RibbonTabDataContext;
            var ribbonGroupTuples = GetAllRibbonGroups();

            foreach (var createdRibbonGroup in ribbonGroupTuples.
                Where(p => p.RibbonGroupMetaData.ParentGUID == ribbonTab.RibbonTabMetaData.GUID)) {

                var header = LanguageService.FindResourceString(createdRibbonGroup.RibbonGroupMetaData.HeaderLanguageKey);
                var groupBox = new RadRibbonGroup {
                    Header = header,
                    DataContext = createdRibbonGroup
                };

                ///设定UI自动化测试相关属性;
                AutomationProperties.SetAutomationId(groupBox,createdRibbonGroup.RibbonGroupMetaData.GUID);
                AutomationProperties.SetName(groupBox, header);

                InitializeRibbonGroup(groupBox);

                ribbonTabControl.Items.Add(groupBox);
            }
        }
        
        /// <summary>
        /// 获取所有的Ribbon组;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CreatedRibbonGroup> GetAllRibbonGroups() {
            var ribbonGroupObjects = _mefRibbonGroupsProviders.Union<object>(_mefRibbonGroups).
                OrderBy(p => {
                    if(p is Lazy<IRibbonGroup,IRibbonGroupMetaData> ribbonGroupTuple) {
                        return ribbonGroupTuple.Metadata.Order;
                    }
                    else if(p is Lazy<IRibbonGroupsProvider,IRibbonGroupsProviderMetaData> ribbonGroupsProviderTuple) {
                        return ribbonGroupsProviderTuple.Metadata.Order;
                    }
                    else {
                        return int.MaxValue;
                    }
                });

            foreach (var ribbonGroupObject in ribbonGroupObjects) {
                if(ribbonGroupObject is Lazy<IRibbonGroupsProvider, IRibbonGroupsProviderMetaData> groupProviderTuple) {
                    foreach (var ribbonGroup in groupProviderTuple.Value.Groups.OrderBy(p => p.RibbonGroupMetaData.Order)) {
                        yield return new CreatedRibbonGroup(ribbonGroup.RibbonGroup,ribbonGroup.RibbonGroupMetaData);
                    }
                }
                else if(ribbonGroupObject is Lazy<IRibbonGroup, IRibbonGroupMetaData> groupTuple) {
                    yield return new CreatedRibbonGroup(groupTuple.Value, groupTuple.Metadata);
                }
            }
        }

        /// <summary>
        /// 获取所有的Ribbon项;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CreatedRibbonItem> GetAllRibbonItems() {
            var ribbonItemsObjects = _mefRibbonItems.Union<object>(_mefRibbonItemsProviders).OrderBy(p => {
                if(p is Lazy<IRibbonItemsProvider,IRibbonItemsProviderMetaData> ribbonItemsProviderTuple) {
                    return ribbonItemsProviderTuple.Metadata.Order;
                }
                else if(p is Lazy<IRibbonItem,IRibbonItemMetaData> ribbonItemTuple) {
                    return ribbonItemTuple.Metadata.Order;
                }
                else {
                    return int.MaxValue;
                }
            });

            foreach (var ribbonItemObject in ribbonItemsObjects) {
                if (ribbonItemObject is Lazy<IRibbonItemsProvider,IRibbonItemsProviderMetaData> itemsProviderTuple) {
                    foreach (var innerRibbonItem in itemsProviderTuple.Value.Items) {
                        yield return innerRibbonItem;
                    }
                    continue;
                }
                else if(ribbonItemObject is Lazy<IRibbonItem,IRibbonItemMetaData> itemTuple){
                    yield return new CreatedRibbonItem(itemTuple.Value,itemTuple.Metadata);
                }
            }
        }

        private void InitializeRibbonGroup(RadRibbonGroup ribbonGroupControl) {
            var ribbonGroup = ribbonGroupControl.DataContext as CreatedRibbonGroup;

            var allRibbonGroups = GetAllRibbonGroups();
            var allRibbonItems = GetAllRibbonItems();

            foreach (var innerRibbonGroup in allRibbonGroups.
                Where(p => p.RibbonGroupMetaData.ParentGUID == ribbonGroup.RibbonGroupMetaData.GUID)) {

                var icon = innerRibbonGroup.RibbonGroupMetaData.Icon;
                var text = LanguageService.FindResourceString(innerRibbonGroup.RibbonGroupMetaData.HeaderLanguageKey);

                var dropDownButton = new RadRibbonDropDownButton {
                    DataContext = innerRibbonGroup,
                    SmallImage = !string.IsNullOrEmpty(icon) ?  new BitmapImage(new Uri(icon)):null,
                    Size = ButtonSize.Large,
                    Text = text
                };

                ///设定UI自动化测试相关属性;
                AutomationProperties.SetAutomationId(dropDownButton, innerRibbonGroup.RibbonGroupMetaData.GUID);
                AutomationProperties.SetName(dropDownButton, text);

                InitializeDropDownButton(dropDownButton);
                ribbonGroupControl.Items.Add(dropDownButton);
            }

            foreach (var ribbonItem in allRibbonItems.
                Where(p => p.RibbonItemMetaData.GroupGUID == ribbonGroup.RibbonGroupMetaData.GUID)) {
                
                if (ribbonItem.RibbonItem is IRibbonButtonItem buttonItem) {
                    var bitmapImage = !string.IsNullOrEmpty(buttonItem.Icon) ? new BitmapImage(new Uri(buttonItem.Icon, UriKind.RelativeOrAbsolute)) : null;
                    var text = LanguageService.FindResourceString(buttonItem.HeaderLanguageKey);

                    var ribbonButton = new RadRibbonButton {
                        Command = buttonItem.Command,
                        Text = text,
                        LargeImage = bitmapImage,
                        SmallImage = bitmapImage,
                        Size = ButtonSize.Large
                    };


                    ///设定UI自动化测试相关属性;
                    AutomationProperties.SetAutomationId(ribbonButton, ribbonItem.RibbonItemMetaData.GUID??string.Empty);
                    AutomationProperties.SetName(ribbonButton, text??string.Empty);

                    ribbonGroupControl.Items.Add(ribbonButton);
                }
                else if (ribbonItem.RibbonItem is IRibbonObjectItem objectItem) {
                    ribbonGroupControl.Items.Add(objectItem.UIObject);
                }
            }
        }

        private void InitializeDropDownButton(RadRibbonDropDownButton dropDownButton) {
            var ribbonGroupContext = dropDownButton.DataContext as CreatedRibbonGroup;
            var ribbonItems = GetAllRibbonItems();

            var contextMenu = new RadContextMenu();

            foreach (var ribbonItem in ribbonItems.
                Where(p => p.RibbonItemMetaData.GroupGUID == ribbonGroupContext.RibbonGroupMetaData.GUID)) {

                if(ribbonItem.RibbonItem is IRibbonButtonItem buttonItem) {
                    var bitMapImage = !string.IsNullOrEmpty(buttonItem.Icon) ? new BitmapImage(new Uri(buttonItem.Icon, UriKind.RelativeOrAbsolute)) : null;
                    var image = bitMapImage != null ? new Image { Source = bitMapImage }:null;
                    var header = LanguageService.FindResourceString(buttonItem.HeaderLanguageKey);
                    var menuItem = new RadMenuItem {
                        Header = header,
                        Icon = image,
                        Command = buttonItem.Command
                    };

                    ///设定UI自动化测试相关属性;
                    AutomationProperties.SetAutomationId(menuItem, ribbonItem.RibbonItemMetaData.GUID);
                    AutomationProperties.SetName(menuItem, header);

                    contextMenu.Items.Add(menuItem);
                }
            }
            
            dropDownButton.DropDownContent = contextMenu;
        }
        
    }

    class RibbonTabDataContext {
        public IRibbonTab RibbonTab { get; }

        public IRibbonTabMetaData RibbonTabMetaData { get; }

        public RibbonTabDataContext(IRibbonTab ribbonTab, IRibbonTabMetaData ribbonTabMetaData) {

            RibbonTab = ribbonTab ?? throw new ArgumentNullException(nameof(ribbonTab));

            RibbonTabMetaData = ribbonTabMetaData ?? throw new ArgumentNullException(nameof(ribbonTabMetaData));

        }
    }
}
