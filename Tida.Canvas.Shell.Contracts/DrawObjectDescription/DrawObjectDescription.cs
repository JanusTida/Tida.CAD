using System;

namespace Tida.Canvas.Shell.Contracts.DrawObjectDescription {
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
