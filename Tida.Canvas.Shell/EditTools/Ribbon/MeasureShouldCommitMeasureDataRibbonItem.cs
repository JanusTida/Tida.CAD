using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Ribbon;
using System.Windows.Controls;
using static Tida.Canvas.Shell.Contracts.EditTools.Constants;
using Tida.Canvas.Shell.EditTools.Measure;

namespace Tida.Canvas.Shell.EditTools.Ribbon {
    /// <summary>
    /// Ribbon项,标注,测量完成后,是否保留测量数据;
    /// </summary>
    [ExportRibbonItem(GroupGUID = EditToolGroup_Measure, GUID = "da", Order = 0)]
    class MeasureShouldCommitMeasureDataRibbonItem : IRibbonObjectItem {
       
        private CheckBox _checkBox;
        public object UIObject {
            get {
                if(_checkBox == null) {
                    _checkBox = new CheckBox {
                        Content = LanguageService.FindResourceString(Constants.MenuItemName_MeasureShouldCommitMeasureData)
                    };
                    _checkBox.IsThreeState = false;
                    _checkBox.Unchecked += CheckBox_Unchecked;
                    _checkBox.Checked += CheckBox_Checked;
                }

                return _checkBox;
            }
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e) {
            MeasureSettings.ShouldCommitMeasureData = true;
        }

        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e) {
            MeasureSettings.ShouldCommitMeasureData = false;
        }
    }
}
