using Tida.Application.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.Canvas {
    /// <summary>
    /// 画布编辑器服务;
    /// </summary>
    public interface ICanvasService {
        /// <summary>
        /// 画布当前的上下文数据;
        /// </summary>
        ICanvasDataContext CanvasDataContext { get; }
        
        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize();

        /// <summary>
        /// 呈递事务;
        /// </summary>
        /// <param name="transaction"></param>
        //void CommitTransaction(IEditTransaction transaction);

        /// <summary>
        /// 移除指定的绘制对象集合;
        /// </summary>
        /// <param name="drawObjects"></param>
        /// <remarks>这将会呈递一个事务</remarks>
        //void RemoveDrawObjects(ICollection<DrawObject> drawObjects);

        /// <summary>
        /// 添加指定的绘制对象集合;
        /// </summary>
        /// <param name="drawObjects"></param>
        //void AddDrawObjects(ICollection<DrawObject> drawObjects);
    }

    public sealed class CanvasService : GenericServiceStaticInstance<ICanvasService> {
        
        /// <summary>
        /// 画布当前的上下文数据;
        /// </summary>
        public static ICanvasDataContext CanvasDataContext => Current?.CanvasDataContext;

        /// <summary>
        /// 呈递事务;
        /// </summary>
        /// <param name="transaction"></param>
        //public static void CommitTransaction(IEditTransaction transaction) => Current?.CommitTransaction(transaction);
    }
}
