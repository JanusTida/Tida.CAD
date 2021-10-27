using System;
using System.Windows;
using Tida.CAD.Events;

namespace Tida.CAD
{

    /// <summary>
    /// 画布控件协约;
    /// </summary>
    public interface ICADControl : 
        IHaveEditTool , 
        ICADContextEx
#if WPF
        ,IInputElement
#endif
    {
        /// <summary>
        /// 是否是只读的,当此值设置为真时,本控件及其内容将无法通过输入设备被操作;
        /// </summary>
        bool IsReadOnly { get; set; }
        
        /// <summary>
        /// 撤销;
        /// </summary>
        void Undo();

        /// <summary>
        /// 能否撤销;
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// 可撤销状态发生变化;
        /// </summary>
        event EventHandler<CanUndoChangedEventArgs> CanUndoChanged;

        /// <summary>
        /// 重做;
        /// </summary>
        void Redo();

        /// <summary>
        /// 能否重做;
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// 可重做状态发生变化;
        /// </summary>
        event EventHandler<CanRedoChangedEventArgs> CanRedoChanged;

        /// <summary>
        /// 清除栈内所有事务;
        /// </summary>
        void ClearTransactions();

        /// <summary>
        /// 添加事务操作;
        /// </summary>
        /// <param name="editTranaction"></param>
        void CommitTransaction(IEditTransaction editTransaction);

        /// <summary>
        /// 事务已经回撤;
        /// </summary>
        event EventHandler<EditTransactionUndoneEventArgs> EditTransactionUndone;

        /// <summary>
        /// 事务已经重做;
        /// </summary>
        event EventHandler<EditTransactionRedoneEventArgs> EditTransactionRedone;
        
        /// <summary>
        /// 聚焦;
        /// </summary>
        bool Focus();

        /// <summary>
        /// 是否被聚焦;          
        /// </summary>
        bool IsFocused { get; }
        
        /// <summary>
        /// 画布内绘制对象选定状态发生了变化事件;
        /// </summary>
        event EventHandler<DrawObjectSelectedChangedEventArgs> DrawObjectIsSelectedChanged;

        /// <summary>
        /// 绘制对象被移除事件;
        /// </summary>
        event EventHandler<DrawObjectsRemovedEventArgs> DrawObjectsRemoved;

        /// <summary>
        /// 绘制对象被添加事件;
        /// </summary>
        event EventHandler<DrawObjectsAddedEventArgs> DrawObjectsAdded;

        /// <summary>
        /// 绘制对象是否正在被编辑发生了变化事件;
        /// </summary>
        event EventHandler<DrawObjectIsEditingChangedEventArgs> DrawObjectIsEditingChanged;
    }

}
