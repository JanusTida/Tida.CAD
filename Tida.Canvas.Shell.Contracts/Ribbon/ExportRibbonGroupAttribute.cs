﻿using System;
using System.ComponentModel.Composition;

namespace Tida.Canvas.Shell.Contracts.Ribbon {
    /// <summary>
    /// 导出Ribbon组;
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportRibbonGroupAttribute:ExportAttribute, IRibbonGroupMetaData {
        public ExportRibbonGroupAttribute():base(typeof(IRibbonGroup)) {

        }

        public string ParentGUID { get; set; }

        public string GUID { get; set; }

        public string HeaderLanguageKey { get; set; }

        public int Order { get; set; }

        public string Icon { get; set; }
    }
}
