using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.CAD {
    /// <summary>
    /// 编辑事务,用于记录编辑操作,便于进行撤销,重做等操作;
    /// </summary>
    public interface IEditTransaction {
        /// <summary>
        /// 撤销;
        /// </summary>
        void Undo();

        /// <summary>
        /// 重做;
        /// </summary>
        void Redo();

        /// <summary>
        /// 能否重做;
        /// </summary>
        bool CanRedo { get; }
        
    }

}
