﻿using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;
using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Shell.Contracts.Canvas.Events;
using Tida.Canvas.Shell.Contracts.EditTools;
using Tida.Canvas.Shell.Contracts.StatusBar;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Tida.Canvas.Shell.Canvas.StatusBar {
    /// <summary>
    /// 状态栏项——当前编辑工具项;
    /// </summary>
    [Export(typeof(IStatusBarItem))]
    class CurrentEditToolStatusBarItem : StatusBarTextItem {
        [ImportingConstructor]
        public CurrentEditToolStatusBarItem(
            [ImportMany]IEnumerable<Lazy<IEditToolProvider, IEditToolProviderMetaData>> mefEditToolProviders
        ) : base(Constants.StatusBarItem_CurrentEditTool) {

            this.Order = Constants.StatusBarOrder_CurrentEditTool;
            _mefEditToolProviders = mefEditToolProviders;

            CommonEventHelper.GetEvent<CanvasEditToolChangedEvent>().Subscribe(EditTool_Loaded); 
            
            Text = _statusBarText_CurrentEditTool;
        }
        
        private readonly string _statusBarText_CurrentEditTool =
            LanguageService.FindResourceString(Constants.StatusBarItem_CurrentEditTool);

        private void EditTool_Loaded(CanvasEditToolChangedEventArgs args) {
            var metaData = _mefEditToolProviders.FirstOrDefault(p => p.Value.ValidateFromThis(args.EventArgs.NewValue))?.Metadata;
            if(metaData != null) {
                Text = _statusBarText_CurrentEditTool + LanguageService.FindResourceString(metaData.EditToolLanguageKey);
            }
            else {
                Text = _statusBarText_CurrentEditTool;
            }
        }

        private readonly IEnumerable<Lazy<IEditToolProvider, IEditToolProviderMetaData>> _mefEditToolProviders;
    }
}
