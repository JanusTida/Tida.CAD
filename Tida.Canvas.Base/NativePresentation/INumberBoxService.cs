using Tida.Application.Contracts.Common;

namespace Tida.Canvas.Base.NativePresentation {
    /// <summary>
    /// 输入框服务;
    /// </summary>
    public interface INumberBoxService {
        INumberBoxContainer CreateContainer();

        INumberBox CreateNumberBox();
    }

    public class NumberBoxService:GenericServiceStaticInstance<INumberBoxService> {

        public static INumberBoxContainer CreateINumberBoxContainer() => Current?.CreateContainer();
        public static INumberBox CreateNumberBox() => Current?.CreateNumberBox();
    }
}
