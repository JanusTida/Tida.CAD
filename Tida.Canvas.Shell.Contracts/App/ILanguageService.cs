using System.Collections.Generic;
using Tida.Canvas.Shell.Contracts.Common;

namespace Tida.Canvas.Shell.Contracts.App {
    /// <summary>
    /// 描述语言种类的单位;
    /// </summary>
    public class LanguageProvider {
        public LanguageProvider(string languageName, string languageType) {
            this.LanguageName = languageName;
            Type = languageType;
        }
        /// <summary>
        /// 语言名称(比如简体中文);
        /// </summary>
        public string LanguageName { get; }
        /// <summary>
        /// 类型;比如(zh_CN,en_US)
        /// </summary>
        public string Type { get; }
    }

    /// <summary>
    /// 语言服务契约;
    /// </summary>
    public interface ILanguageService {
        /// <summary>
        /// 找寻资源字符串;
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        string FindResourceString(string keyName);

        /// <summary>
        /// 当前语言类型;
        /// </summary>
        LanguageProvider CurrentProvider { get; set; }

        /// <summary>
        /// 所有语言;
        /// </summary>
        IEnumerable<LanguageProvider> AllProviders { get; }

        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize();

        /// <summary>
        /// 尝试根据指定格式的值与参数获取字符串内容,适用于句势具有动态性的语言查找场景;
        /// </summary>
        /// <param name="languageFormatKey">语言格式键值(比如"{0}是哲学家")</param>
        /// <param name="args"></param>
        /// <returns></returns>
        string TryGetStringWithFormat(string languageFormatKey, params object[] args);
    }

    /// <summary>
    /// 由于测试项目中无Application.Current对象,故单独抽象出一个接口提供被操作的语言资源字典;
    ///  被操作的语言相关资源字典对象;
    /// </summary>
    public interface ILanguageDictionary {
        string this[string keyName] { get; }
        /// <summary>
        /// 清除所有合并后字典;
        /// </summary>
        void ClearMergedDictionaries();
        /// <summary>
        /// 从指定的绝对路径读取资源字典,并合并;
        /// </summary>
        /// <param name="path"></param>
        void AddMergedDictionaryFromPath(string path);
    }

    /// <summary>
    /// 语言服务的简单封装;
    /// </summary>
    public class LanguageService : GenericServiceStaticInstance<ILanguageService> {
        public static string FindResourceString(string keyName) => Current?.FindResourceString(keyName);
        public static string TryGetStringWithFormat(string languageFormatKey, params object[] args) => Current?.TryGetStringWithFormat(languageFormatKey, args);
    }
}
