using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tida.Canvas.Shell.Contracts.Controls {
    /// <summary>
    /// 用于封装<see cref="GridLength"/>
    /// </summary>
    public class GridChildLength {
        public GridLength GridLength { get; set; }
        public double MinLength { get; set; }
        public double MaxLength { get; set; }
        
        public GridChildLength(GridLength length, double min = 0.0, double max = double.PositiveInfinity) {
            GridLength = length;
            MinLength = min;
            MaxLength = max;
        }
        
        /// <summary>
        /// 网格长度随内部内容而变化的常量;
        /// </summary>
        public static readonly GridChildLength Auto = new GridChildLength(GridLength.Auto);
        /// <summary>
        /// 网格长度按余下长度伸缩的常量;
        /// </summary>
        public static readonly GridChildLength Star = new GridChildLength(new GridLength(1, GridUnitType.Star));
    }


}
