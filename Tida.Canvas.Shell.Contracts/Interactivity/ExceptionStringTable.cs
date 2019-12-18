using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Tida.Canvas.Shell.Contracts.Interactivity {
    // Token: 0x0200001F RID: 31
    internal class ExceptionStringTable {
        // Token: 0x060000D8 RID: 216 RVA: 0x000042F5 File Offset: 0x000024F5
        internal ExceptionStringTable() {
        }

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x060000D9 RID: 217 RVA: 0x00004300 File Offset: 0x00002500
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(ExceptionStringTable.resourceMan, null)) {
                    ResourceManager resourceManager = new ResourceManager("ExceptionStringTable", typeof(ExceptionStringTable).Assembly);
                    ExceptionStringTable.resourceMan = resourceManager;
                }
                return ExceptionStringTable.resourceMan;
            }
        }

        // Token: 0x1700003D RID: 61
        // (get) Token: 0x060000DA RID: 218 RVA: 0x0000433F File Offset: 0x0000253F
        // (set) Token: 0x060000DB RID: 219 RVA: 0x00004346 File Offset: 0x00002546
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture {
            get {
                return ExceptionStringTable.resourceCulture;
            }
            set {
                ExceptionStringTable.resourceCulture = value;
            }
        }

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x060000DC RID: 220 RVA: 0x0000434E File Offset: 0x0000254E
        internal static string CannotHostBehaviorCollectionMultipleTimesExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("CannotHostBehaviorCollectionMultipleTimesExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x060000DD RID: 221 RVA: 0x00004364 File Offset: 0x00002564
        internal static string CannotHostBehaviorMultipleTimesExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("CannotHostBehaviorMultipleTimesExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x060000DE RID: 222 RVA: 0x0000437A File Offset: 0x0000257A
        internal static string CannotHostTriggerActionMultipleTimesExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("CannotHostTriggerActionMultipleTimesExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x060000DF RID: 223 RVA: 0x00004390 File Offset: 0x00002590
        internal static string CannotHostTriggerCollectionMultipleTimesExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("CannotHostTriggerCollectionMultipleTimesExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x060000E0 RID: 224 RVA: 0x000043A6 File Offset: 0x000025A6
        internal static string CannotHostTriggerMultipleTimesExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("CannotHostTriggerMultipleTimesExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x060000E1 RID: 225 RVA: 0x000043BC File Offset: 0x000025BC
        internal static string CommandDoesNotExistOnBehaviorWarningMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("CommandDoesNotExistOnBehaviorWarningMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x060000E2 RID: 226 RVA: 0x000043D2 File Offset: 0x000025D2
        internal static string DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x060000E3 RID: 227 RVA: 0x000043E8 File Offset: 0x000025E8
        internal static string DuplicateItemInCollectionExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("DuplicateItemInCollectionExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x060000E4 RID: 228 RVA: 0x000043FE File Offset: 0x000025FE
        internal static string EventTriggerBaseInvalidEventExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("EventTriggerBaseInvalidEventExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000047 RID: 71
        // (get) Token: 0x060000E5 RID: 229 RVA: 0x00004414 File Offset: 0x00002614
        internal static string EventTriggerCannotFindEventNameExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("EventTriggerCannotFindEventNameExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x060000E6 RID: 230 RVA: 0x0000442A File Offset: 0x0000262A
        internal static string RetargetedTypeConstraintViolatedExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("RetargetedTypeConstraintViolatedExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x060000E7 RID: 231 RVA: 0x00004440 File Offset: 0x00002640
        internal static string TypeConstraintViolatedExceptionMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("TypeConstraintViolatedExceptionMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x060000E8 RID: 232 RVA: 0x00004456 File Offset: 0x00002656
        internal static string UnableToResolveTargetNameWarningMessage {
            get {
                return ExceptionStringTable.ResourceManager.GetString("UnableToResolveTargetNameWarningMessage", ExceptionStringTable.resourceCulture);
            }
        }

        // Token: 0x04000040 RID: 64
        private static ResourceManager resourceMan;

        // Token: 0x04000041 RID: 65
        private static CultureInfo resourceCulture;
    }
}

