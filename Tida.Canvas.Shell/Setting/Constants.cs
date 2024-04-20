using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.Setting {
    /// <summary>
    /// 设定相关常量;
    /// </summary>
    static class Constants {

        /// <summary>
        /// 设定存档名称;
        /// </summary>
        public const string SettingProfileName = "Settings.xml";
        

        /*设定存档内各元素名称;*/

        public const string XElemName_Setting = "Setting";
        
        public const string XElemName_Sections = "Sections";
        
        public const string XElemName_Section = "Section";

        public const string XElemName_Section_GUID = "GUID";
        
        public const string XElemName_Section_Value = "Value";

    }
}
