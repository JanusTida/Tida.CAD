using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.Base.NativePresentation {
    /// <summary>
    /// 数字输入框契约;
    /// </summary>
    public interface INumberBox {
        /// <summary>
        /// 所呈现的数字;
        /// </summary>
        double? Number { get; set; }

        /// <summary>
        /// 保留小数点小数的位数;
        /// </summary>
        int SavedBits { get; set; }

        /// <summary>
        /// 是否可见;
        /// </summary>
        bool Visible { get; set; }
        
        /// <summary>
        /// 数字通过Tab经过了确认;
        /// </summary>
        event EventHandler TabConfirmed;
        /// <summary>
        /// 数字经过回车进行了确认;
        /// </summary>
        event EventHandler EnterConfirmed;
        
        ///// <summary>
        ///// 全选;
        ///// </summary>
        void SelectAll();

        /// <summary>
        /// 是否为只读(仅限用户输入);
        /// </summary>
        bool IsReadOnly { get; set; }
        
        /// <summary>
        /// 当前视图位置(相对左上角);
        /// </summary>
        Vector2D Position { get; set; }


    }
}
