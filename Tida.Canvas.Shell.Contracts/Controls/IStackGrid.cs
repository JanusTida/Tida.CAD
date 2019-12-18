using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tida.Canvas.Shell.Contracts.Controls {
    /// <summary>
    /// UI堆叠栈,本契约封装对<see cref="Grid"/>的直接操作;
    /// </summary>
    /// <typeparam name="IStackItem">堆叠项类型</typeparam>
    public interface IStackGrid<TStackItem> : IUIObjectProvider where TStackItem:IUIObjectProvider{
        /// <summary>
        /// 所有子项;
        /// </summary>
        IEnumerable<TStackItem> Children { get; }

        /// <summary>
        /// 添加子项;
        /// </summary>
        /// <typeparam name="TDefinition"></typeparam>
        /// <param name="child"></param>
        /// <param name="definition">子项类型,须是ColumnDefinition/RowDefinition</param>
        /// <param name="index">插入位置</param>
        void AddChild(TStackItem child,GridChildLength gridChildLength, int index = -1);
        void Remove(TStackItem child);
        void Clear();
        /// <summary>
        /// 朝向;
        /// </summary>
		Orientation Orientation { get; set; }
        double SplitterLength { get; set; }
    }
	
    /// <summary>
    /// UI堆叠栈工厂;
    /// </summary>
	public interface IStackGridFactory {
        /// <summary>
        /// 创建一个IStackGrid;
        /// </summary>
        /// <param name="grid">从外界提供的Grid,若为空则将自行创建一个Grid</param>
        /// <returns></returns>
        IStackGrid<TStackItem> CreateNew<TStackItem>(Grid grid = null) where TStackItem:class,IUIObjectProvider;
    }

    public class StackGridFactory : GenericServiceStaticInstance<IStackGridFactory>{
        public static IStackGrid<TStackItem> CreateNew<TStackItem>(Grid grid = null) where TStackItem : class,IUIObjectProvider {
            return Current?.CreateNew<TStackItem>(grid);
        }
    }
    
}
