using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Docking;
using Tida.Canvas.Shell.Contracts.MainPage;
using Tida.Canvas.Shell.Contracts.Ribbon;
using Prism.Commands;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using static Tida.Canvas.Shell.Contracts.MainPage.Constants;

namespace Tida.Canvas.Shell.MainPage {
    
    //[Export(typeof(IDockingPane)), Export]
    class LeftDockingPane : DockingPaneBase,IDockingPaneMetaData {
        public LeftDockingPane() {

        }
        public string InitPaneGroupGUID => DockingPaneGroup_Left;


        public string GUID => string.Empty;

        public override string Header { get; set; } = LanguageService.FindResourceString("CaseData");

        public double InitialWidth { get; } = 210;
        

        private TextBlock _txb = new TextBlock { Text = "Fuckyou" };
        public override object UIObject => _txb;

        public double InitialHeight => double.NaN;

        public bool CanUserClose => false;

        public bool CanFloat => false;
    }


#if DEBUG
    //[ExportRibbonItem(GroupGUID = Tida.Canvas.Shell.Canvas.Constants.RibbonGroup_Edit, GUID = "da", Order = 231)]
#endif
    class TestRibbonItem : IRibbonButtonItem {
        [ImportingConstructor]
        public TestRibbonItem(LeftDockingPane leftDockingPane) {
            this._leftDockingPane = leftDockingPane;
        }

        private readonly LeftDockingPane _leftDockingPane;
        public string Icon => null;

        public ICommand Command => _testCommand ?? (_testCommand =
            new DelegateCommand(() => {
                //if (MainDockingService.Current.DockingPanes.Contains(_leftDockingPane)) {
                //    MainDockingService.Current.RemovePane(_leftDockingPane);
                //}
                //else {
                //    MainDockingService.Current.AddPane(_leftDockingPane);
                //}
            }
        ));

        private DelegateCommand _testCommand;
        public string HeaderLanguageKey => "测试";
    }
}
