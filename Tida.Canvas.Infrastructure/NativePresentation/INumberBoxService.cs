namespace Tida.Canvas.Infrastructure.NativePresentation {
    /// <summary>
    /// 输入框服务;
    /// </summary>
    public interface INumberBoxService {
        /// <summary>
        /// 创建一个新的输入框容器;
        /// </summary>
        /// <returns></returns>
        INumberBoxContainer CreateContainer();

        /// <summary>
        /// 创建一个新的输入框;
        /// </summary>
        /// <returns></returns>
        INumberBox CreateNumberBox();
    }
}
