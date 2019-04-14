//using Tida.Application.Contracts.App;
//using Tida.Application.Contracts.Common;
//using Tida.Application.Contracts.Docking;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using Telerik.Windows.Controls;
//using Telerik.Windows.Controls.Docking;
//using static Tida.Canvas.Shell.MainPage.Constants;

//namespace Tida.Canvas.Shell.MainPage {
//    class DockingPanesFactoryImpl : DockingPanesFactory {
//        static DockingPanesFactoryImpl() {
//            ThemeService.AddDictionary(_dockingThemesDict);
//        }

//        public DockingPanesFactoryImpl(IDockingService dockingService) {
//            this._dockingService = dockingService ?? throw new ArgumentNullException(nameof(dockingService));
//        }

//        private IDockingService _dockingService;
//        
//        
//        
//        

        

//        
//        
//        
//        
//        
//        
//        
//        
//        

//        //protected override void RemovePane(RadPane pane) {
//        //    pane.RemoveFromParent();
//        //    return;
//        //    var radDocking = pane.GetParentDocking();
//        //    if (!(pane.Tag is IDockingPane dockingPane)) {
//        //        base.RemovePane(pane);
//        //        return;
//        //    }
           
//        //    //寻找DockGroup是否已经存在;
//        //    var paneGroupControl =  radDocking.SplitItems.Select(p => p as RadPaneGroup).Where(p => p != null).
//        //        FirstOrDefault(g => g.Tag is IDockingPaneGroup group && group.GUID == dockingPane.InitPaneGroupGUID);
            
//        //    //若存在,直接加入新的Pane;
//        //    if (paneGroupControl != null) {
//        //        paneGroupControl.RemovePane(pane);
//        //        return;
//        //    }
//        //}

//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        
//        }
//    }
//}
