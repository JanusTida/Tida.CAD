using Tida.Canvas.Contracts;
using Tida.Canvas.Events;
using Tida.Canvas.Infrastructure.DynamicInput;
using Tida.Canvas.Infrastructure.InteractionHandlers;
using Tida.Canvas.Shell.Contracts.InteractionHandlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.Setting;
using static Tida.Canvas.Shell.Contracts.Constants;

namespace Tida.Canvas.Shell.InteractionHandlers {

    /// <summary>
    /// 动态输入交互处理器提供器;
    /// </summary>
    [ExportCanvasInteractionHandlerProvider]
    class DynamicInputInteractionHandlerProvider : ICanvasInteractionHandlerProvider {
        [ImportingConstructor]
        public DynamicInputInteractionHandlerProvider([ImportMany]IEnumerable<Lazy<ICanvasControlDynamicInputerProvider, ICanvasInteractionHandlerProviderMetaData>> mefEditToolDynamicInputerProviders) {

            DynamicInputInteractionHandler.CanvasControlDynamicInputerProviders.
                AddRange(mefEditToolDynamicInputerProviders.OrderBy(p => p.Metadata.Order).Select(p => p.Value));

            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);

            //默认设为可用;
            section?.SetAttribute(SettingName_DynamicInput, true);
            DynamicInputInteractionHandler.IsEnabled = true;
            DynamicInputInteractionHandler.IsEnabledChanged += DynamicInputInteractionHandler_IsEnabledChanged;
        }

        /// <summary>
        /// 同步设定;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DynamicInputInteractionHandler_IsEnabledChanged(object sender, ValueChangedEventArgs<bool> e) {

            var section = SettingsService.GetOrCreateSection(SettingSection_Canvas);

            //默认设为可用;
            section?.SetAttribute(SettingName_DynamicInput, e.NewValue);
        }

        public CanvasInteractionHandler CreateHandler() => new DynamicInputInteractionHandler();
    }
}
