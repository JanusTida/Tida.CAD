using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Events {
    /// <summary>
    /// 事务已经撤销事件;
    /// </summary>
    public class EditTransactionUndoneEventArgs:EventArgs {
        public EditTransactionUndoneEventArgs(IEnumerable<IEditTransaction> editTransactions) {
            EditTransaction = editTransactions ?? throw new ArgumentNullException(nameof(editTransactions));
        }

        /// <summary>
        /// 事务集合;
        /// </summary>
        public IEnumerable<IEditTransaction> EditTransaction { get; }
    }
}
