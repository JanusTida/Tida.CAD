using Tida.Canvas.Shell.Contracts.Common;
using System;
using System.Runtime.CompilerServices;

namespace Tida.Canvas.Shell.Contracts.Common {
    /// <summary>
    /// 日志服务契约;
    /// </summary>
    public interface ILoggerService {
        void WriteLine(string msg);
        void WriteCallerLine(string msg, [CallerMemberName] string callerName = null);
        void WriteException(Exception ex, [CallerMemberName] string callerName = null);
        void WriteStack(string msg, [CallerMemberName] string callerName = null);
    }

    /// <summary>
    /// 日志服务的简单封装(并未实现);
    /// </summary>
    public class LoggerService : GenericServiceStaticInstance<ILoggerService> {
        public static void WriteLine(string msg) {
            Current?.WriteLine(msg);
        }

        public static void WriteCallerLine(string msg, [CallerMemberName] string callerName = null) {
            Current?.WriteCallerLine(msg, callerName);
        }

        public static void WriteException(Exception ex, [CallerMemberName] string callerName = null) {
            Current?.WriteException(ex, callerName);
        }

        public static void WriteStack(string msg, [CallerMemberName] string callerName = null) {
            Current?.WriteStack(msg, callerName);
        }
    }
}
