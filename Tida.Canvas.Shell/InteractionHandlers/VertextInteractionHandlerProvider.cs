using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Infrastructure.InteractionHandlers;
using Tida.Canvas.Shell.Contracts.InteractionHandlers;
using Tida.Canvas.Shell.Contracts.Setting;
using static Tida.Canvas.Shell.Contracts.Constants;

namespace Tida.Canvas.Shell.InteractionHandlers {
    /// <summary>
    /// 正交模式交互处理器提供者;
    /// </summary>
    [ExportCanvasInteractionHandlerProvider(Order = 1)]
    public class VertextInteractionHandlerProvider : ICanvasInteractionHandlerProvider {
        public VertextInteractionHandlerProvider() {
            //加载设定;
            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            var vertextModeEnabled = section.GetAttribute<bool>(SettingName_VertexMode);

            VertextInteractionHandler.IsEnabled = vertextModeEnabled;
            VertextInteractionHandler.IsEnabledChanged += VertextInteractionHandler_IsEnabledChanged;
        }

        private void VertextInteractionHandler_IsEnabledChanged(object sender, ValueChangedEventArgs<bool> e) {
            //更改设定;
            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);
            section.SetAttribute(SettingName_VertexMode, e.NewValue);
        }

        public CanvasInteractionHandler CreateHandler() => new VertextInteractionHandler();
    }
}
