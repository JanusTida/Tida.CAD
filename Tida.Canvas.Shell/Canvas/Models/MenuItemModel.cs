﻿using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Menu;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Tida.Canvas.Shell.Canvas.Models {
    /// <summary>
    /// 菜单项模型;
    /// </summary>
    public class MenuItemModel : BindableBase {
        public MenuItemModel(CreatedMenuItem createMenuItem) {

            _createMenuItem = createMenuItem ?? throw new ArgumentNullException(nameof(createMenuItem));

        }

        private readonly CreatedMenuItem _createMenuItem;

        private string _name;
        public string Name => _name ?? (_name = LanguageService.FindResourceString(_createMenuItem.MenuItemMetaData.HeaderLanguageKey));

        public ICommand Command => _createMenuItem.MenuItem.Command;

        public ObservableCollection<MenuItemModel> Children { get; } = new ObservableCollection<MenuItemModel>();
    }
}
