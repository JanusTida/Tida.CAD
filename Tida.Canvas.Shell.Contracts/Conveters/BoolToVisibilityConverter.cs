using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Tida.Canvas.Shell.Contracts.Conveters {
    public class BoolToVisibilityConverter : IValueConverter {
        public static readonly IValueConverter StaticInstance = new BoolToVisibilityConverter();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">调整输出的参数,是否反转，是否保留(hidden,collapsed)</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool val;
            VisibilityAttributes attributes = VisibilityAttributes.Normal;
            Boolean.TryParse(value.ToString(), out val);
            if (parameter != null) {
                Enum.TryParse<VisibilityAttributes>(parameter.ToString(), out attributes);
            }
            if ((attributes & VisibilityAttributes.Reverse) == VisibilityAttributes.Reverse) {
                return val ? (attributes & VisibilityAttributes.SaveSpace) == VisibilityAttributes.SaveSpace ?
                    Visibility.Hidden : Visibility.Collapsed :
                    Visibility.Visible;
            }
            else {
                return val ? Visibility.Visible :
                    (attributes & VisibilityAttributes.SaveSpace) == VisibilityAttributes.SaveSpace ?
                    Visibility.Hidden : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 转换可视的枚举;
    /// </summary>
    [Flags]
    public enum VisibilityAttributes {
        Normal = 0,
        SaveSpace = 1,//是否保留位置;
        Reverse = 2//是否反转;
    }
}
