using System;
using System.ComponentModel.Composition;
using Tida.Canvas.Infrastructure.InteractionHandlers;

namespace Tida.Canvas.Shell.Contracts.InteractionHandlers {
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportCanvasInteractionHandlerProviderAttribute : ExportAttribute, ICanvasInteractionHandlerProviderMetaData {
        public ExportCanvasInteractionHandlerProviderAttribute() : base(typeof(ICanvasInteractionHandlerProvider)) {

        }

        public int Order { get; set; }
    }
}
