using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200000A RID: 10
    public static class DependencyObjectHelper {
        // Token: 0x0600002D RID: 45 RVA: 0x0000293C File Offset: 0x00000B3C
        public static IEnumerable<DependencyObject> GetSelfAndAncestors(this DependencyObject dependencyObject) {
            while (dependencyObject != null) {
                yield return dependencyObject;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            yield break;
        }
    }
}
