using Tida.CAD.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.CAD;

namespace Tida.Canvas.Events {
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
