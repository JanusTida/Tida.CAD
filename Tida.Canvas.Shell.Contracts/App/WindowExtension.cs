using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Tida.Canvas.Shell.Contracts.App {
    /// <summary>
    /// 窗体拓展;
    /// </summary>
    public static class WindowExtension {

        /// <summary>
        /// 设置窗口的主窗体
        /// </summary>
        /// <param name="window"></param>
        /// <param name="owner"></param>
        public static void Show(this Window window, IntPtr owner) {
            var mainhelper = new WindowInteropHelper(window);
            mainhelper.Owner = owner;
            window.Show();
        }

        /// <summary>
        /// 设置窗口的主窗体
        /// </summary>
        /// <param name="window"></param>
        /// <param name="owner"></param>
        public static void Show(this Window window, IWin32Window owner) {
            var mainhelper = new WindowInteropHelper(window);
            mainhelper.Owner = owner.Handle;
            window.Show();
        }
        
        public static bool? ShowDialog(this Window window, IntPtr owner) {
            var mainhelper = new WindowInteropHelper(window);
            mainhelper.Owner = owner;
            return window.ShowDialog();
        }

        public static bool? ShowDialog(this Window window, Window owner) {
            window.Owner = owner;
            return window.ShowDialog();
        }

        public static bool? ShowDialog(this Window window,object owner) {
            if(owner is Window ownerWindow) {
                return window.ShowDialog(ownerWindow);
            }
            else if(owner is IntPtr ownerPtr && ownerPtr != IntPtr.Zero) {
                return window.ShowDialog(ownerPtr);
            }

            return window.ShowDialog();
        }
    }


   
}
