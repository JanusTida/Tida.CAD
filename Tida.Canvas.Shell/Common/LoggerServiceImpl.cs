using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Tida.Canvas.Shell.Contracts.App;
using Tida.Canvas.Shell.Contracts.Common;
using static Tida.Canvas.Shell.Common.Constants;

namespace Tida.Canvas.Shell.Common {
    [Export(typeof(ILoggerService))]
    class LoggerServiceImpl : ILoggerService {
        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(true);
        

        public void WriteCallerLine(string msg, [CallerMemberName] string callerName = null) {
            var st = new StackFrame(1);
            var sm = st.GetMethod();
            WriteLine($"{sm?.ReflectedType?.FullName} -> {callerName} : {msg}");
        }

        public void WriteException(Exception ex, [CallerMemberName] string callerName = null) {
            WriteCallerLine($"{nameof(Exception)}:{ex.Message}",callerName);
            WriteCallerLine($"{nameof(ex.StackTrace)}:{ex.StackTrace}",callerName);
            var ie = ex.InnerException;
            while (ie != null) {
                WriteLine($"\t{nameof(Type)}:{ie.GetType()}\t{nameof(ie.Message)}:{ie.Message}");
                //if(ie is System.ComponentModel.Composition.CompositionException ce) {
                //    foreach (var err in ce.Errors) {
                //        Logger.WriteLine(err.Description);
                //    }
                //}

                ie = ie.InnerException;
            }
        }

        public void WriteLine(string msg) {
            _waitHandle.WaitOne();

            try {
                var appDataDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\{CompanyName}\\{ProductName}";
                if (!Directory.Exists(appDataDir)) {
                    Directory.CreateDirectory(appDataDir);
                }

                using (var sw = new StreamWriter($"{appDataDir}\\{LogFileName}", true)) {
                    var record = $"{DateTime.Now.ToLongTimeString()}-{DateTime.Now.ToLongDateString()}\t{msg}";
                    sw.WriteLine(record);
                }
            }
            catch(Exception ex) {

            }
            
            
            _waitHandle.Set();
        }

        public void WriteStack(string msg, [CallerMemberName] string callerName = null) {
            var st = new StackFrame(1);
            var sm = st.GetMethod();
            WriteLine($"{sm?.ReflectedType?.FullName} -> {callerName} : {msg}");
        }
    }
}
