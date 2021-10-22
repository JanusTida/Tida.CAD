using System;

namespace Tida.CAD
{

    /// <summary>
    /// 标准编辑事务,本类在构造时通过外部传递的委托的方式,得到撤销/重做动作;
    /// </summary>
    public class StandardEditTransaction : IEditTransaction {
        /// <summary>
        /// 以委托的方式构建标准事务;
        /// </summary>
        /// <param name="undoAct">撤销动作</param>
        /// <param name="redoAct">重做动作</param>
        public StandardEditTransaction(Action undoAct, Action redoAct) {
            if (undoAct == null) {
                throw new ArgumentNullException(nameof(undoAct));
            }
            if (redoAct == null) {
                throw new ArgumentNullException(nameof(redoAct));
            }

            UnDoAct = undoAct;
            RedoAct = redoAct;
        }
        public Action UnDoAct { get; }
        public Action RedoAct { get; }

        public bool CanRedo => true;

        public void Undo() => UnDoAct();

        public void Redo() => RedoAct();
    }
}
