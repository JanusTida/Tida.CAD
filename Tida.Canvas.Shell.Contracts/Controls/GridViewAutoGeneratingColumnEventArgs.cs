using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Tida.Canvas.Shell.Contracts.Controls {
    public class GridViewAutoGeneratingColumnEventArgs {
        public GridViewAutoGeneratingColumnEventArgs(ItemPropertyInfo propertyInfo) {
            if(propertyInfo == null) {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            this.ItemPropertyInfo = propertyInfo;
        }

        /// <summary>
        /// 绑定的数据属性信息;
        /// </summary>
        public ItemPropertyInfo ItemPropertyInfo { get; }

        /// <summary>
        /// 数据模板;
        /// </summary>
        public DataTemplate CellTemplate { get; set; }
        
        /// <summary>
        /// 转换器;
        /// </summary>
        public IValueConverter Converter {get;set;}

        public bool Cancel { get; set; }

        public bool ShowDistinctFilters { get; set; }
    }
}
