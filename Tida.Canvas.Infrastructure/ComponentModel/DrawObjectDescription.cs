using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.ComponentModel {
    /// <summary>
    /// 绘制对象描述信息;
    /// </summary>
    public class DrawObjectDescription {
        public DrawObjectDescription(string typeName) {

            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));

        }

        /// <summary>
        /// 类型名;
        /// </summary>
        public string TypeName { get; }
    }
}
