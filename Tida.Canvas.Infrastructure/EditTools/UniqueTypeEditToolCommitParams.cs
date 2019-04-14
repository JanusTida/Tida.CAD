using Tida.Canvas.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Infrastructure.EditTools {
    /// <summary>
    /// 单种单种绘制对象编辑工具呈递信息;
    /// </summary>
    public class UniqueTypeEditToolCommitParams {
        public UniqueTypeEditToolCommitParams(ICollection<DrawObject> addedDrawObjects) {

            AddedDrawObjects = addedDrawObjects ?? throw new ArgumentNullException(nameof(addedDrawObjects));

        }
        /// <summary>
        /// 应用的图层;
        /// </summary>
        public CanvasLayer AppliedLayer { get; set; }

        /// <summary>
        /// 将要被添加的绘制对象集合;
        /// </summary>
        public ICollection<DrawObject> AddedDrawObjects { get; }

        /// <summary>
        /// 指示是否取消;
        /// </summary>
        public bool Cancel { get; set; }
    }
}
