using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Shell.App {
    public static partial class Constants {
        //语言资源字典名称;
        public const string LanguageDict = nameof(LanguageDict);

        //主题资源字典名称;
        public const string ThemeDict = nameof(ThemeDict);


        //语言配置文件名;
        public const string LanguageConfigName = "LanguageConfig.xml";

        //语言文件夹;
        public const string LanguageDirect = "Languages";

        //当前语言;
        public const string CurrentLanguage = "CurrentLanguage";

        //语言提供器元素;
        public const string LanguageProviders = nameof(LanguageProviders);
        //语言元素名;
        public const string Provider = nameof(Provider);
        //语言名称;
        public const string ProviderName = "Name";
        //语言类型;
        public const string ProviderType = "Type";

        

    }
    /// <summary>
    /// 语言部分;
    /// </summary>
    public static partial class Constants {
        public const string IsWorking = nameof(IsWorking);

        //正在取消工作;
        public const string WindowDescrip_CancelingProcess = nameof(WindowDescrip_CancelingProcess);

        public const string MsgText_ConfirmToCancel = nameof(MsgText_ConfirmToCancel);

        public const string WindowTitle_InputString = nameof(WindowTitle_InputString);
            


        public const string MsgText_NoOptionSelected = nameof(MsgText_NoOptionSelected);


        public const string WindowTitle_Tip = nameof(WindowTitle_Tip);
    }
}
