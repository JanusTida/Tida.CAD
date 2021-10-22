using System;
using System.Collections.Generic;

namespace Tida.CAD.Events
{
    /// <summary>
    /// 事务已经重做事件;
    /// </summary>
    public class EditTransactionRedoneEventArgs:EventArgs {
        public EditTransactionRedoneEventArgs(IEnumerable<IEditTransaction> editTransactions) {
            EditTransaction = editTransactions ?? throw new ArgumentNullException(nameof(editTransactions));
        }

        /// <summary>
        /// 事务集合;
        /// </summary>
        public IEnumerable<IEditTransaction> EditTransaction { get; }
    }
}
