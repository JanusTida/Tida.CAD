using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Input {
    /// <summary>
    /// 按键;
    /// </summary>
    public enum Key {
        //
        // 摘要:
        //     没有按任何键。
        None = 0,
        //
        // 摘要:
        //     Cancel 键。
        Cancel = 1,
        //
        // 摘要:
        //     Backspace 键。
        Back = 2,
        //
        // 摘要:
        //     Tab 键
        Tab = 3,
        //
        // 摘要:
        //     Linefeed 键。
        LineFeed = 4,
        //
        // 摘要:
        //     Clear 键。
        Clear = 5,
        //
        // 摘要:
        //     Return 键。
        Return = 6,
        //
        // 摘要:
        //     Enter 键。
        Enter = 6,
        //
        // 摘要:
        //     Pause 键。
        Pause = 7,
        //
        // 摘要:
        //     Caps Lock 键。
        Capital = 8,
        //
        // 摘要:
        //     Caps Lock 键。
        CapsLock = 8,
        //
        // 摘要:
        //     IME Kana 模式键。
        KanaMode = 9,
        //
        // 摘要:
        //     IME Hangul 模式键。
        HangulMode = 9,
        //
        // 摘要:
        //     IME Junja 模式键。
        JunjaMode = 10,
        //
        // 摘要:
        //     IME 最终模式键。
        FinalMode = 11,
        //
        // 摘要:
        //     IME Hanja 模式键。
        HanjaMode = 12,
        //
        // 摘要:
        //     IME Kanji 模式键。
        KanjiMode = 12,
        //
        // 摘要:
        //     Esc 键。
        Escape = 13,
        //
        // 摘要:
        //     IME 转换键。
        ImeConvert = 14,
        //
        // 摘要:
        //     IME 非转换键。
        ImeNonConvert = 15,
        //
        // 摘要:
        //     IME 接受键。
        ImeAccept = 16,
        //
        // 摘要:
        //     IME 模式更改请求。
        ImeModeChange = 17,
        //
        // 摘要:
        //     空格键。
        Space = 18,
        //
        // 摘要:
        //     Page Up 键。
        Prior = 19,
        //
        // 摘要:
        //     Page Up 键。
        PageUp = 19,
        //
        // 摘要:
        //     Page Down 键。
        Next = 20,
        //
        // 摘要:
        //     Page Down 键。
        PageDown = 20,
        //
        // 摘要:
        //     End 键。
        End = 21,
        //
        // 摘要:
        //     Home 键。
        Home = 22,
        //
        // 摘要:
        //     向左键。
        Left = 23,
        //
        // 摘要:
        //     向上键。
        Up = 24,
        //
        // 摘要:
        //     向右键。
        Right = 25,
        //
        // 摘要:
        //     向下键。
        Down = 26,
        //
        // 摘要:
        //     Select 键。
        Select = 27,
        //
        // 摘要:
        //     Print 键。
        Print = 28,
        //
        // 摘要:
        //     Execute 键。
        Execute = 29,
        //
        // 摘要:
        //     Print Screen 键。
        Snapshot = 30,
        //
        // 摘要:
        //     Print Screen 键。
        PrintScreen = 30,
        //
        // 摘要:
        //     Insert 键。
        Insert = 31,
        //
        // 摘要:
        //     Delete 键。
        Delete = 32,
        //
        // 摘要:
        //     Help 键。
        Help = 33,
        //
        // 摘要:
        //     0（零）键。
        D0 = 34,
        //
        // 摘要:
        //     1（一）键。
        D1 = 35,
        //
        // 摘要:
        //     2 键。
        D2 = 36,
        //
        // 摘要:
        //     3 键。
        D3 = 37,
        //
        // 摘要:
        //     4 键。
        D4 = 38,
        //
        // 摘要:
        //     5 键。
        D5 = 39,
        //
        // 摘要:
        //     6 键。
        D6 = 40,
        //
        // 摘要:
        //     7 键。
        D7 = 41,
        //
        // 摘要:
        //     8 键。
        D8 = 42,
        //
        // 摘要:
        //     9 键。
        D9 = 43,
        //
        // 摘要:
        //     A 键。
        A = 44,
        //
        // 摘要:
        //     B 键。
        B = 45,
        //
        // 摘要:
        //     C 键。
        C = 46,
        //
        // 摘要:
        //     D 键。
        D = 47,
        //
        // 摘要:
        //     E 键。
        E = 48,
        //
        // 摘要:
        //     F 键。
        F = 49,
        //
        // 摘要:
        //     G 键。
        G = 50,
        //
        // 摘要:
        //     H 键。
        H = 51,
        //
        // 摘要:
        //     I 键。
        I = 52,
        //
        // 摘要:
        //     J 键。
        J = 53,
        //
        // 摘要:
        //     K 键。
        K = 54,
        //
        // 摘要:
        //     L 键。
        L = 55,
        //
        // 摘要:
        //     M 键。
        M = 56,
        //
        // 摘要:
        //     N 键。
        N = 57,
        //
        // 摘要:
        //     O 键。
        O = 58,
        //
        // 摘要:
        //     P 键。
        P = 59,
        //
        // 摘要:
        //     Q 键。
        Q = 60,
        //
        // 摘要:
        //     R 键。
        R = 61,
        //
        // 摘要:
        //     S 键。
        S = 62,
        //
        // 摘要:
        //     T 键。
        T = 63,
        //
        // 摘要:
        //     U 键。
        U = 64,
        //
        // 摘要:
        //     V 键。
        V = 65,
        //
        // 摘要:
        //     W 键。
        W = 66,
        //
        // 摘要:
        //     X 键。
        X = 67,
        //
        // 摘要:
        //     Y 键。
        Y = 68,
        //
        // 摘要:
        //     Z 键。
        Z = 69,
        //
        // 摘要:
        //     左 Windows 徽标键（Microsoft Natural Keyboard，人体工程学键盘）。
        LWin = 70,
        //
        // 摘要:
        //     右 Windows 徽标键（Microsoft Natural Keyboard，人体工程学键盘）。
        RWin = 71,
        //
        // 摘要:
        //     应用程序键（Microsoft Natural Keyboard，人体工程学键盘）。
        Apps = 72,
        //
        // 摘要:
        //     计算机睡眠键。
        Sleep = 73,
        //
        // 摘要:
        //     数字键盘上的 0 键。
        NumPad0 = 74,
        //
        // 摘要:
        //     数字键盘上的 1 键。
        NumPad1 = 75,
        //
        // 摘要:
        //     数字键盘上的 2 键。
        NumPad2 = 76,
        //
        // 摘要:
        //     数字键盘上的 3 键。
        NumPad3 = 77,
        //
        // 摘要:
        //     数字键盘上的 4 键。
        NumPad4 = 78,
        //
        // 摘要:
        //     数字键盘上的 5 键。
        NumPad5 = 79,
        //
        // 摘要:
        //     数字键盘上的 6 键。
        NumPad6 = 80,
        //
        // 摘要:
        //     数字键盘上的 7 键。
        NumPad7 = 81,
        //
        // 摘要:
        //     数字键盘上的 8 键。
        NumPad8 = 82,
        //
        // 摘要:
        //     数字键盘上的 9 键。
        NumPad9 = 83,
        //
        // 摘要:
        //     乘号键。
        Multiply = 84,
        //
        // 摘要:
        //     加号键。
        Add = 85,
        //
        // 摘要:
        //     分隔符键。
        Separator = 86,
        //
        // 摘要:
        //     减号键。
        Subtract = 87,
        //
        // 摘要:
        //     句点键。
        Decimal = 88,
        //
        // 摘要:
        //     除号键。
        Divide = 89,
        //
        // 摘要:
        //     F1 键。
        F1 = 90,
        //
        // 摘要:
        //     F2 键。
        F2 = 91,
        //
        // 摘要:
        //     F3 键。
        F3 = 92,
        //
        // 摘要:
        //     F4 键。
        F4 = 93,
        //
        // 摘要:
        //     F5 键。
        F5 = 94,
        //
        // 摘要:
        //     F6 键。
        F6 = 95,
        //
        // 摘要:
        //     F7 键。
        F7 = 96,
        //
        // 摘要:
        //     F8 键。
        F8 = 97,
        //
        // 摘要:
        //     F9 键。
        F9 = 98,
        //
        // 摘要:
        //     F10 键。
        F10 = 99,
        //
        // 摘要:
        //     F11 键。
        F11 = 100,
        //
        // 摘要:
        //     F12 键。
        F12 = 101,
        //
        // 摘要:
        //     F13 键。
        F13 = 102,
        //
        // 摘要:
        //     F14 键。
        F14 = 103,
        //
        // 摘要:
        //     F15 键。
        F15 = 104,
        //
        // 摘要:
        //     F16 键。
        F16 = 105,
        //
        // 摘要:
        //     F17 键。
        F17 = 106,
        //
        // 摘要:
        //     F18 键。
        F18 = 107,
        //
        // 摘要:
        //     F19 键。
        F19 = 108,
        //
        // 摘要:
        //     F20 键。
        F20 = 109,
        //
        // 摘要:
        //     F21 键。
        F21 = 110,
        //
        // 摘要:
        //     F22 键。
        F22 = 111,
        //
        // 摘要:
        //     F23 键。
        F23 = 112,
        //
        // 摘要:
        //     F24 键。
        F24 = 113,
        //
        // 摘要:
        //     Num Lock 键。
        NumLock = 114,
        //
        // 摘要:
        //     Scroll Lock 键。
        Scroll = 115,
        //
        // 摘要:
        //     左 Shift 键。
        LeftShift = 116,
        //
        // 摘要:
        //     右 Shift 键。
        RightShift = 117,
        //
        // 摘要:
        //     左 Ctrl 键。
        LeftCtrl = 118,
        //
        // 摘要:
        //     右 Ctrl 键。
        RightCtrl = 119,
        //
        // 摘要:
        //     左 Alt 键。
        LeftAlt = 120,
        //
        // 摘要:
        //     右 Alt 键。
        RightAlt = 121,
        //
        // 摘要:
        //     浏览器后退键。
        BrowserBack = 122,
        //
        // 摘要:
        //     浏览器前进键。
        BrowserForward = 123,
        //
        // 摘要:
        //     浏览器刷新键。
        BrowserRefresh = 124,
        //
        // 摘要:
        //     浏览器停止键。
        BrowserStop = 125,
        //
        // 摘要:
        //     浏览器搜索键。
        BrowserSearch = 126,
        //
        // 摘要:
        //     浏览器搜藏夹键。
        BrowserFavorites = 127,
        //
        // 摘要:
        //     浏览器主页键。
        BrowserHome = 128,
        //
        // 摘要:
        //     静音键。
        VolumeMute = 129,
        //
        // 摘要:
        //     调低音量键。
        VolumeDown = 130,
        //
        // 摘要:
        //     调高音量键。
        VolumeUp = 131,
        //
        // 摘要:
        //     媒体下一曲目键。
        MediaNextTrack = 132,
        //
        // 摘要:
        //     媒体上一曲目键。
        MediaPreviousTrack = 133,
        //
        // 摘要:
        //     媒体停止键。
        MediaStop = 134,
        //
        // 摘要:
        //     媒体暂停播放键。
        MediaPlayPause = 135,
        //
        // 摘要:
        //     启动邮件键。
        LaunchMail = 136,
        //
        // 摘要:
        //     选择媒体键。
        SelectMedia = 137,
        //
        // 摘要:
        //     启动应用程序 1 键。
        LaunchApplication1 = 138,
        //
        // 摘要:
        //     启动应用程序 2 键。
        LaunchApplication2 = 139,
        //
        // 摘要:
        //     OEM 1 键。
        Oem1 = 140,
        //
        // 摘要:
        //     OEM 分号键。
        OemSemicolon = 140,
        //
        // 摘要:
        //     OEM 添加键。
        OemPlus = 141,
        //
        // 摘要:
        //     OEM 逗号键。
        OemComma = 142,
        //
        // 摘要:
        //     OEM 减号键。
        OemMinus = 143,
        //
        // 摘要:
        //     OEM 句点键。
        OemPeriod = 144,
        //
        // 摘要:
        //     OEM 2 键。
        Oem2 = 145,
        //
        // 摘要:
        //     OEM 问号键。
        OemQuestion = 145,
        //
        // 摘要:
        //     OEM 3 键。
        Oem3 = 146,
        //
        // 摘要:
        //     OEM 波形符键。
        OemTilde = 146,
        //
        // 摘要:
        //     ABNT_C1（巴西）键。
        AbntC1 = 147,
        //
        // 摘要:
        //     ABNT_C2（巴西）键。
        AbntC2 = 148,
        //
        // 摘要:
        //     OEM 4 键。
        Oem4 = 149,
        //
        // 摘要:
        //     OEM 左括号键。
        OemOpenBrackets = 149,
        //
        // 摘要:
        //     OEM 5 键。
        Oem5 = 150,
        //
        // 摘要:
        //     OEM 管道键。
        OemPipe = 150,
        //
        // 摘要:
        //     OEM 6 键。
        Oem6 = 151,
        //
        // 摘要:
        //     OEM 右括号键。
        OemCloseBrackets = 151,
        //
        // 摘要:
        //     OEM 7 键。
        Oem7 = 152,
        //
        // 摘要:
        //     OEM 引号键。
        OemQuotes = 152,
        //
        // 摘要:
        //     OEM 8 键。
        Oem8 = 153,
        //
        // 摘要:
        //     OEM 102 键。
        Oem102 = 154,
        //
        // 摘要:
        //     OEM 反斜杠键。
        OemBackslash = 154,
        //
        // 摘要:
        //     一个特殊键，用于屏蔽 IME 正在处理的真实键。
        ImeProcessed = 155,
        //
        // 摘要:
        //     一个特殊键，用于屏蔽正作为系统键处理的真实键。
        System = 156,
        //
        // 摘要:
        //     OEM ATTN 键。
        OemAttn = 157,
        //
        // 摘要:
        //     DBE_ALPHANUMERIC 键。
        DbeAlphanumeric = 157,
        //
        // 摘要:
        //     OEM 完成键。
        OemFinish = 158,
        //
        // 摘要:
        //     DBE_KATAKANA 键
        DbeKatakana = 158,
        //
        // 摘要:
        //     OEM 复制键。
        OemCopy = 159,
        //
        // 摘要:
        //     DBE_HIRAGANA 键。
        DbeHiragana = 159,
        //
        // 摘要:
        //     OEM 自动键。
        OemAuto = 160,
        //
        // 摘要:
        //     DBE_SBCSCHAR 键。
        DbeSbcsChar = 160,
        //
        // 摘要:
        //     OEM ENLW 键。
        OemEnlw = 161,
        //
        // 摘要:
        //     DBE_DBCSCHAR 键。
        DbeDbcsChar = 161,
        //
        // 摘要:
        //     OEM BACKTAB 键。
        OemBackTab = 162,
        //
        // 摘要:
        //     DBE_ROMAN 键。
        DbeRoman = 162,
        //
        // 摘要:
        //     ATTN 键。
        Attn = 163,
        //
        // 摘要:
        //     DBE_NOROMAN 键。
        DbeNoRoman = 163,
        //
        // 摘要:
        //     Crsel 键。
        CrSel = 164,
        //
        // 摘要:
        //     DBE_ENTERWORDREGISTERMODE 键。
        DbeEnterWordRegisterMode = 164,
        //
        // 摘要:
        //     Exsel 键。
        ExSel = 165,
        //
        // 摘要:
        //     The DBE_ENTERIMECONFIGMODE 键。
        DbeEnterImeConfigureMode = 165,
        //
        // 摘要:
        //     ERASE EOF 键。
        EraseEof = 166,
        //
        // 摘要:
        //     DBE_FLUSHSTRING 键。
        DbeFlushString = 166,
        //
        // 摘要:
        //     PLAY 键。
        Play = 167,
        //
        // 摘要:
        //     DBE_CODEINPUT 键。
        DbeCodeInput = 167,
        //
        // 摘要:
        //     ZOOM 键。
        Zoom = 168,
        //
        // 摘要:
        //     DBE_NOCODEINPUT 键。
        DbeNoCodeInput = 168,
        //
        // 摘要:
        //     保留以备将来使用的常数。
        NoName = 169,
        //
        // 摘要:
        //     DBE_DETERMINESTRING 键。
        DbeDetermineString = 169,
        //
        // 摘要:
        //     PA1 键。
        Pa1 = 170,
        //
        // 摘要:
        //     DBE_ENTERDLGCONVERSIONMODE 键。
        DbeEnterDialogConversionMode = 170,
        //
        // 摘要:
        //     OEM Clear 键。
        OemClear = 171,
        //
        // 摘要:
        //     此键与另一个键配合使用，创建了一个组合字符。
        DeadCharProcessed = 172
    }
}
