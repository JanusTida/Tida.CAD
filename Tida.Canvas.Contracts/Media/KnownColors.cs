using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    internal static class KnownColors {
        // Token: 0x060026AD RID: 9901 RVA: 0x0009D184 File Offset: 0x0009C584
        static KnownColors() {
            foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor))) {
                string key = string.Format("#{0,8:X8}", (uint)knownColor);
                s_knownArgbColors[key] = knownColor;
            }
        }

        // Token: 0x060026AE RID: 9902 RVA: 0x0009D21C File Offset: 0x0009C61C
        //public static SolidColorBrush ColorStringToKnownBrush(string s) {
        //    if (s != null) {
        //        KnownColor knownColor = KnownColors.ColorStringToKnownColor(s);
        //        if (knownColor != KnownColor.UnknownColor) {
        //            return KnownColors.SolidColorBrushFromUint((uint)knownColor);
        //        }
        //    }
        //    return null;
        //}

        // Token: 0x060026AF RID: 9903 RVA: 0x0009D240 File Offset: 0x0009C640
        public static bool IsKnownSolidColorBrush(SolidColorBrush scp) {
            Dictionary<uint, SolidColorBrush> obj = s_solidColorBrushCache;
            bool result;
            lock (obj) {
                result = s_solidColorBrushCache.ContainsValue(scp);
            }
            return result;
        }

        // Token: 0x060026B0 RID: 9904 RVA: 0x0009D294 File Offset: 0x0009C694
        public static SolidColorBrush SolidColorBrushFromUint(uint argb) {
            SolidColorBrush solidColorBrush = null;
            Dictionary<uint, SolidColorBrush> obj = s_solidColorBrushCache;
            lock (obj) {
                if (!s_solidColorBrushCache.TryGetValue(argb, out solidColorBrush)) {
                    solidColorBrush = new SolidColorBrush(Color.FromUInt32(argb));
                    solidColorBrush.Freeze();
                    s_solidColorBrushCache[argb] = solidColorBrush;
                }
            }
            return solidColorBrush;
        }

        // Token: 0x060026B1 RID: 9905 RVA: 0x0009D30C File Offset: 0x0009C70C
        internal static string MatchColor(string colorString, out bool isKnownColor, out bool isNumericColor, out bool isContextColor, out bool isScRgbColor) {
            string text = colorString.Trim();
            if ((text.Length == 4 || text.Length == 5 || text.Length == 7 || text.Length == 9) && text[0] == '#') {
                isNumericColor = true;
                isScRgbColor = false;
                isKnownColor = false;
                isContextColor = false;
                return text;
            }
            isNumericColor = false;
            if (text.StartsWith("sc#", StringComparison.Ordinal)) {
                isNumericColor = false;
                isScRgbColor = true;
                isKnownColor = false;
                isContextColor = false;
            }
            else {
                isScRgbColor = false;
            }
            if (text.StartsWith("ContextColor ", StringComparison.OrdinalIgnoreCase)) {
                isContextColor = true;
                isScRgbColor = false;
                isKnownColor = false;
                return text;
            }
            isContextColor = false;
            isKnownColor = true;
            return text;
        }

        //// Token: 0x060026B2 RID: 9906 RVA: 0x0009D3A4 File Offset: 0x0009C7A4
        //internal static KnownColor ColorStringToKnownColor(string colorString) {
        //    if (colorString != null) {
        //        string text = colorString.ToUpper(CultureInfo.InvariantCulture);
        //        switch (text.Length) {
        //            case 3:
        //                if (text.Equals("RED")) {
        //                    return (KnownColor)4294901760u;
        //                }
        //                if (text.Equals("TAN")) {
        //                    return (KnownColor)4291998860u;
        //                }
        //                break;
        //            case 4: {
        //                    char c = text[0];
        //                    switch (c) {
        //                        case 'A':
        //                            if (text.Equals("AQUA")) {
        //                                return (KnownColor)4278255615u;
        //                            }
        //                            break;
        //                        case 'B':
        //                            if (text.Equals("BLUE")) {
        //                                return (KnownColor)4278190335u;
        //                            }
        //                            break;
        //                        case 'C':
        //                            if (text.Equals("CYAN")) {
        //                                return (KnownColor)4278255615u;
        //                            }
        //                            break;
        //                        case 'D':
        //                        case 'E':
        //                        case 'F':
        //                            break;
        //                        case 'G':
        //                            if (text.Equals("GOLD")) {
        //                                return (KnownColor)4294956800u;
        //                            }
        //                            if (text.Equals("GRAY")) {
        //                                return (KnownColor)4286611584u;
        //                            }
        //                            break;
        //                        default:
        //                            switch (c) {
        //                                case 'L':
        //                                    if (text.Equals("LIME")) {
        //                                        return (KnownColor)4278255360u;
        //                                    }
        //                                    break;
        //                                case 'N':
        //                                    if (text.Equals("NAVY")) {
        //                                        return (KnownColor)4278190208u;
        //                                    }
        //                                    break;
        //                                case 'P':
        //                                    if (text.Equals("PERU")) {
        //                                        return (KnownColor)4291659071u;
        //                                    }
        //                                    if (text.Equals("PINK")) {
        //                                        return (KnownColor)4294951115u;
        //                                    }
        //                                    if (text.Equals("PLUM")) {
        //                                        return (KnownColor)4292714717u;
        //                                    }
        //                                    break;
        //                                case 'S':
        //                                    if (text.Equals("SNOW")) {
        //                                        return (KnownColor)4294966010u;
        //                                    }
        //                                    break;
        //                                case 'T':
        //                                    if (text.Equals("TEAL")) {
        //                                        return (KnownColor)4278222976u;
        //                                    }
        //                                    break;
        //                            }
        //                            break;
        //                    }
        //                    break;
        //                }
        //            case 5: {
        //                    char c = text[0];
        //                    switch (c) {
        //                        case 'A':
        //                            if (text.Equals("AZURE")) {
        //                                return (KnownColor)4293984255u;
        //                            }
        //                            break;
        //                        case 'B':
        //                            if (text.Equals("BEIGE")) {
        //                                return (KnownColor)4294309340u;
        //                            }
        //                            if (text.Equals("BLACK")) {
        //                                return (KnownColor)4278190080u;
        //                            }
        //                            if (text.Equals("BROWN")) {
        //                                return (KnownColor)4289014314u;
        //                            }
        //                            break;
        //                        case 'C':
        //                            if (text.Equals("CORAL")) {
        //                                return (KnownColor)4294934352u;
        //                            }
        //                            break;
        //                        case 'D':
        //                        case 'E':
        //                        case 'F':
        //                        case 'H':
        //                        case 'J':
        //                        case 'M':
        //                        case 'N':
        //                            break;
        //                        case 'G':
        //                            if (text.Equals("GREEN")) {
        //                                return (KnownColor)4278222848u;
        //                            }
        //                            break;
        //                        case 'I':
        //                            if (text.Equals("IVORY")) {
        //                                return (KnownColor)4294967280u;
        //                            }
        //                            break;
        //                        case 'K':
        //                            if (text.Equals("KHAKI")) {
        //                                return (KnownColor)4293977740u;
        //                            }
        //                            break;
        //                        case 'L':
        //                            if (text.Equals("LINEN")) {
        //                                return (KnownColor)4294635750u;
        //                            }
        //                            break;
        //                        case 'O':
        //                            if (text.Equals("OLIVE")) {
        //                                return (KnownColor)4286611456u;
        //                            }
        //                            break;
        //                        default:
        //                            if (c == 'W') {
        //                                if (text.Equals("WHEAT")) {
        //                                    return (KnownColor)4294303411u;
        //                                }
        //                                if (text.Equals("WHITE")) {
        //                                    return (KnownColor)4294967295u;
        //                                }
        //                            }
        //                            break;
        //                    }
        //                    break;
        //                }
        //            case 6: {
        //                    char c = text[0];
        //                    if (c != 'B') {
        //                        if (c != 'I') {
        //                            switch (c) {
        //                                case 'M':
        //                                    if (text.Equals("MAROON")) {
        //                                        return (KnownColor)4286578688u;
        //                                    }
        //                                    break;
        //                                case 'O':
        //                                    if (text.Equals("ORANGE")) {
        //                                        return (KnownColor)4294944000u;
        //                                    }
        //                                    if (text.Equals("ORCHID")) {
        //                                        return (KnownColor)4292505814u;
        //                                    }
        //                                    break;
        //                                case 'P':
        //                                    if (text.Equals("PURPLE")) {
        //                                        return (KnownColor)4286578816u;
        //                                    }
        //                                    break;
        //                                case 'S':
        //                                    if (text.Equals("SALMON")) {
        //                                        return (KnownColor)4294606962u;
        //                                    }
        //                                    if (text.Equals("SIENNA")) {
        //                                        return (KnownColor)4288696877u;
        //                                    }
        //                                    if (text.Equals("SILVER")) {
        //                                        return (KnownColor)4290822336u;
        //                                    }
        //                                    break;
        //                                case 'T':
        //                                    if (text.Equals("TOMATO")) {
        //                                        return (KnownColor)4294927175u;
        //                                    }
        //                                    break;
        //                                case 'V':
        //                                    if (text.Equals("VIOLET")) {
        //                                        return (KnownColor)4293821166u;
        //                                    }
        //                                    break;
        //                                case 'Y':
        //                                    if (text.Equals("YELLOW")) {
        //                                        return (KnownColor)4294967040u;
        //                                    }
        //                                    break;
        //                            }
        //                        }
        //                        else if (text.Equals("INDIGO")) {
        //                            return (KnownColor)4283105410u;
        //                        }
        //                    }
        //                    else if (text.Equals("BISQUE")) {
        //                        return (KnownColor)4294960324u;
        //                    }
        //                    break;
        //                }
        //            case 7: {
        //                    char c = text[0];
        //                    if (c <= 'M') {
        //                        switch (c) {
        //                            case 'C':
        //                                if (text.Equals("CRIMSON")) {
        //                                    return (KnownColor)4292613180u;
        //                                }
        //                                break;
        //                            case 'D':
        //                                if (text.Equals("DARKRED")) {
        //                                    return (KnownColor)4287299584u;
        //                                }
        //                                if (text.Equals("DIMGRAY")) {
        //                                    return (KnownColor)4285098345u;
        //                                }
        //                                break;
        //                            case 'E':
        //                            case 'G':
        //                                break;
        //                            case 'F':
        //                                if (text.Equals("FUCHSIA")) {
        //                                    return (KnownColor)4294902015u;
        //                                }
        //                                break;
        //                            case 'H':
        //                                if (text.Equals("HOTPINK")) {
        //                                    return (KnownColor)4294928820u;
        //                                }
        //                                break;
        //                            default:
        //                                if (c == 'M') {
        //                                    if (text.Equals("MAGENTA")) {
        //                                        return (KnownColor)4294902015u;
        //                                    }
        //                                }
        //                                break;
        //                        }
        //                    }
        //                    else if (c != 'O') {
        //                        if (c != 'S') {
        //                            if (c == 'T') {
        //                                if (text.Equals("THISTLE")) {
        //                                    return (KnownColor)4292394968u;
        //                                }
        //                            }
        //                        }
        //                        else if (text.Equals("SKYBLUE")) {
        //                            return (KnownColor)4287090411u;
        //                        }
        //                    }
        //                    else if (text.Equals("OLDLACE")) {
        //                        return (KnownColor)4294833638u;
        //                    }
        //                    break;
        //                }
        //            case 8: {
        //                    char c = text[0];
        //                    if (c <= 'H') {
        //                        if (c != 'C') {
        //                            if (c != 'D') {
        //                                if (c == 'H') {
        //                                    if (text.Equals("HONEYDEW")) {
        //                                        return (KnownColor)4293984240u;
        //                                    }
        //                                }
        //                            }
        //                            else {
        //                                if (text.Equals("DARKBLUE")) {
        //                                    return (KnownColor)4278190219u;
        //                                }
        //                                if (text.Equals("DARKCYAN")) {
        //                                    return (KnownColor)4278225803u;
        //                                }
        //                                if (text.Equals("DARKGRAY")) {
        //                                    return (KnownColor)4289309097u;
        //                                }
        //                                if (text.Equals("DEEPPINK")) {
        //                                    return (KnownColor)4294907027u;
        //                                }
        //                            }
        //                        }
        //                        else if (text.Equals("CORNSILK")) {
        //                            return (KnownColor)4294965468u;
        //                        }
        //                    }
        //                    else if (c != 'L') {
        //                        if (c != 'M') {
        //                            if (c == 'S') {
        //                                if (text.Equals("SEAGREEN")) {
        //                                    return (KnownColor)4281240407u;
        //                                }
        //                                if (text.Equals("SEASHELL")) {
        //                                    return (KnownColor)4294964718u;
        //                                }
        //                            }
        //                        }
        //                        else if (text.Equals("MOCCASIN")) {
        //                            return (KnownColor)4294960309u;
        //                        }
        //                    }
        //                    else if (text.Equals("LAVENDER")) {
        //                        return (KnownColor)4293322490u;
        //                    }
        //                    break;
        //                }
        //            case 9:
        //                switch (text[0]) {
        //                    case 'A':
        //                        if (text.Equals("ALICEBLUE")) {
        //                            return (KnownColor)4293982463u;
        //                        }
        //                        break;
        //                    case 'B':
        //                        if (text.Equals("BURLYWOOD")) {
        //                            return (KnownColor)4292786311u;
        //                        }
        //                        break;
        //                    case 'C':
        //                        if (text.Equals("CADETBLUE")) {
        //                            return (KnownColor)4284456608u;
        //                        }
        //                        if (text.Equals("CHOCOLATE")) {
        //                            return (KnownColor)4291979550u;
        //                        }
        //                        break;
        //                    case 'D':
        //                        if (text.Equals("DARKGREEN")) {
        //                            return (KnownColor)4278215680u;
        //                        }
        //                        if (text.Equals("DARKKHAKI")) {
        //                            return (KnownColor)4290623339u;
        //                        }
        //                        break;
        //                    case 'F':
        //                        if (text.Equals("FIREBRICK")) {
        //                            return (KnownColor)4289864226u;
        //                        }
        //                        break;
        //                    case 'G':
        //                        if (text.Equals("GAINSBORO")) {
        //                            return (KnownColor)4292664540u;
        //                        }
        //                        if (text.Equals("GOLDENROD")) {
        //                            return (KnownColor)4292519200u;
        //                        }
        //                        break;
        //                    case 'I':
        //                        if (text.Equals("INDIANRED")) {
        //                            return (KnownColor)4291648604u;
        //                        }
        //                        break;
        //                    case 'L':
        //                        if (text.Equals("LAWNGREEN")) {
        //                            return (KnownColor)4286381056u;
        //                        }
        //                        if (text.Equals("LIGHTBLUE")) {
        //                            return (KnownColor)4289583334u;
        //                        }
        //                        if (text.Equals("LIGHTCYAN")) {
        //                            return (KnownColor)4292935679u;
        //                        }
        //                        if (text.Equals("LIGHTGRAY")) {
        //                            return (KnownColor)4292072403u;
        //                        }
        //                        if (text.Equals("LIGHTPINK")) {
        //                            return (KnownColor)4294948545u;
        //                        }
        //                        if (text.Equals("LIMEGREEN")) {
        //                            return (KnownColor)4281519410u;
        //                        }
        //                        break;
        //                    case 'M':
        //                        if (text.Equals("MINTCREAM")) {
        //                            return (KnownColor)4294311930u;
        //                        }
        //                        if (text.Equals("MISTYROSE")) {
        //                            return (KnownColor)4294960353u;
        //                        }
        //                        break;
        //                    case 'O':
        //                        if (text.Equals("OLIVEDRAB")) {
        //                            return (KnownColor)4285238819u;
        //                        }
        //                        if (text.Equals("ORANGERED")) {
        //                            return (KnownColor)4294919424u;
        //                        }
        //                        break;
        //                    case 'P':
        //                        if (text.Equals("PALEGREEN")) {
        //                            return (KnownColor)4288215960u;
        //                        }
        //                        if (text.Equals("PEACHPUFF")) {
        //                            return (KnownColor)4294957753u;
        //                        }
        //                        break;
        //                    case 'R':
        //                        if (text.Equals("ROSYBROWN")) {
        //                            return (KnownColor)4290547599u;
        //                        }
        //                        if (text.Equals("ROYALBLUE")) {
        //                            return (KnownColor)4282477025u;
        //                        }
        //                        break;
        //                    case 'S':
        //                        if (text.Equals("SLATEBLUE")) {
        //                            return (KnownColor)4285160141u;
        //                        }
        //                        if (text.Equals("SLATEGRAY")) {
        //                            return (KnownColor)4285563024u;
        //                        }
        //                        if (text.Equals("STEELBLUE")) {
        //                            return (KnownColor)4282811060u;
        //                        }
        //                        break;
        //                    case 'T':
        //                        if (text.Equals("TURQUOISE")) {
        //                            return (KnownColor)4282441936u;
        //                        }
        //                        break;
        //                }
        //                break;
        //            case 10: {
        //                    char c = text[0];
        //                    if (c <= 'P') {
        //                        switch (c) {
        //                            case 'A':
        //                                if (text.Equals("AQUAMARINE")) {
        //                                    return (KnownColor)4286578644u;
        //                                }
        //                                break;
        //                            case 'B':
        //                                if (text.Equals("BLUEVIOLET")) {
        //                                    return (KnownColor)4287245282u;
        //                                }
        //                                break;
        //                            case 'C':
        //                                if (text.Equals("CHARTREUSE")) {
        //                                    return (KnownColor)4286578432u;
        //                                }
        //                                break;
        //                            case 'D':
        //                                if (text.Equals("DARKORANGE")) {
        //                                    return (KnownColor)4294937600u;
        //                                }
        //                                if (text.Equals("DARKORCHID")) {
        //                                    return (KnownColor)4288230092u;
        //                                }
        //                                if (text.Equals("DARKSALMON")) {
        //                                    return (KnownColor)4293498490u;
        //                                }
        //                                if (text.Equals("DARKVIOLET")) {
        //                                    return (KnownColor)4287889619u;
        //                                }
        //                                if (text.Equals("DODGERBLUE")) {
        //                                    return (KnownColor)4280193279u;
        //                                }
        //                                break;
        //                            case 'E':
        //                            case 'F':
        //                            case 'H':
        //                            case 'I':
        //                            case 'J':
        //                            case 'K':
        //                                break;
        //                            case 'G':
        //                                if (text.Equals("GHOSTWHITE")) {
        //                                    return (KnownColor)4294506751u;
        //                                }
        //                                break;
        //                            case 'L':
        //                                if (text.Equals("LIGHTCORAL")) {
        //                                    return (KnownColor)4293951616u;
        //                                }
        //                                if (text.Equals("LIGHTGREEN")) {
        //                                    return (KnownColor)4287688336u;
        //                                }
        //                                break;
        //                            case 'M':
        //                                if (text.Equals("MEDIUMBLUE")) {
        //                                    return (KnownColor)4278190285u;
        //                                }
        //                                break;
        //                            default:
        //                                if (c == 'P') {
        //                                    if (text.Equals("PAPAYAWHIP")) {
        //                                        return (KnownColor)4294963157u;
        //                                    }
        //                                    if (text.Equals("POWDERBLUE")) {
        //                                        return (KnownColor)4289781990u;
        //                                    }
        //                                }
        //                                break;
        //                        }
        //                    }
        //                    else if (c != 'S') {
        //                        if (c == 'W') {
        //                            if (text.Equals("WHITESMOKE")) {
        //                                return (KnownColor)4294309365u;
        //                            }
        //                        }
        //                    }
        //                    else if (text.Equals("SANDYBROWN")) {
        //                        return (KnownColor)4294222944u;
        //                    }
        //                    break;
        //                }
        //            case 11: {
        //                    char c = text[0];
        //                    if (c <= 'N') {
        //                        switch (c) {
        //                            case 'D':
        //                                if (text.Equals("DARKMAGENTA")) {
        //                                    return (KnownColor)4287299723u;
        //                                }
        //                                if (text.Equals("DEEPSKYBLUE")) {
        //                                    return (KnownColor)4278239231u;
        //                                }
        //                                break;
        //                            case 'E':
        //                                break;
        //                            case 'F':
        //                                if (text.Equals("FLORALWHITE")) {
        //                                    return (KnownColor)4294966000u;
        //                                }
        //                                if (text.Equals("FORESTGREEN")) {
        //                                    return (KnownColor)4280453922u;
        //                                }
        //                                break;
        //                            case 'G':
        //                                if (text.Equals("GREENYELLOW")) {
        //                                    return (KnownColor)4289593135u;
        //                                }
        //                                break;
        //                            default:
        //                                if (c != 'L') {
        //                                    if (c == 'N') {
        //                                        if (text.Equals("NAVAJOWHITE")) {
        //                                            return (KnownColor)4294958765u;
        //                                        }
        //                                    }
        //                                }
        //                                else {
        //                                    if (text.Equals("LIGHTSALMON")) {
        //                                        return (KnownColor)4294942842u;
        //                                    }
        //                                    if (text.Equals("LIGHTYELLOW")) {
        //                                        return (KnownColor)4294967264u;
        //                                    }
        //                                }
        //                                break;
        //                        }
        //                    }
        //                    else if (c != 'S') {
        //                        if (c != 'T') {
        //                            if (c == 'Y') {
        //                                if (text.Equals("YELLOWGREEN")) {
        //                                    return (KnownColor)4288335154u;
        //                                }
        //                            }
        //                        }
        //                        else if (text.Equals("TRANSPARENT")) {
        //                            return KnownColor.Transparent;
        //                        }
        //                    }
        //                    else {
        //                        if (text.Equals("SADDLEBROWN")) {
        //                            return (KnownColor)4287317267u;
        //                        }
        //                        if (text.Equals("SPRINGGREEN")) {
        //                            return (KnownColor)4278255487u;
        //                        }
        //                    }
        //                    break;
        //                }
        //            case 12: {
        //                    char c = text[0];
        //                    if (c <= 'D') {
        //                        if (c != 'A') {
        //                            if (c == 'D') {
        //                                if (text.Equals("DARKSEAGREEN")) {
        //                                    return (KnownColor)4287609999u;
        //                                }
        //                            }
        //                        }
        //                        else if (text.Equals("ANTIQUEWHITE")) {
        //                            return (KnownColor)4294634455u;
        //                        }
        //                    }
        //                    else if (c != 'L') {
        //                        if (c == 'M') {
        //                            if (text.Equals("MEDIUMORCHID")) {
        //                                return (KnownColor)4290401747u;
        //                            }
        //                            if (text.Equals("MEDIUMPURPLE")) {
        //                                return (KnownColor)4287852763u;
        //                            }
        //                            if (text.Equals("MIDNIGHTBLUE")) {
        //                                return (KnownColor)4279834992u;
        //                            }
        //                        }
        //                    }
        //                    else {
        //                        if (text.Equals("LIGHTSKYBLUE")) {
        //                            return (KnownColor)4287090426u;
        //                        }
        //                        if (text.Equals("LEMONCHIFFON")) {
        //                            return (KnownColor)4294965965u;
        //                        }
        //                    }
        //                    break;
        //                }
        //            case 13: {
        //                    char c = text[0];
        //                    if (c != 'D') {
        //                        if (c != 'L') {
        //                            if (c == 'P') {
        //                                if (text.Equals("PALEGOLDENROD")) {
        //                                    return (KnownColor)4293847210u;
        //                                }
        //                                if (text.Equals("PALETURQUOISE")) {
        //                                    return (KnownColor)4289720046u;
        //                                }
        //                                if (text.Equals("PALEVIOLETRED")) {
        //                                    return (KnownColor)4292571283u;
        //                                }
        //                            }
        //                        }
        //                        else {
        //                            if (text.Equals("LIGHTSEAGREEN")) {
        //                                return (KnownColor)4280332970u;
        //                            }
        //                            if (text.Equals("LAVENDERBLUSH")) {
        //                                return (KnownColor)4294963445u;
        //                            }
        //                        }
        //                    }
        //                    else {
        //                        if (text.Equals("DARKSLATEBLUE")) {
        //                            return (KnownColor)4282924427u;
        //                        }
        //                        if (text.Equals("DARKSLATEGRAY")) {
        //                            return (KnownColor)4281290575u;
        //                        }
        //                        if (text.Equals("DARKGOLDENROD")) {
        //                            return (KnownColor)4290283019u;
        //                        }
        //                        if (text.Equals("DARKTURQUOISE")) {
        //                            return (KnownColor)4278243025u;
        //                        }
        //                    }
        //                    break;
        //                }
        //            case 14: {
        //                    char c = text[0];
        //                    switch (c) {
        //                        case 'B':
        //                            if (text.Equals("BLANCHEDALMOND")) {
        //                                return (KnownColor)4294962125u;
        //                            }
        //                            break;
        //                        case 'C':
        //                            if (text.Equals("CORNFLOWERBLUE")) {
        //                                return (KnownColor)4284782061u;
        //                            }
        //                            break;
        //                        case 'D':
        //                            if (text.Equals("DARKOLIVEGREEN")) {
        //                                return (KnownColor)4283788079u;
        //                            }
        //                            break;
        //                        default:
        //                            if (c != 'L') {
        //                                if (c == 'M') {
        //                                    if (text.Equals("MEDIUMSEAGREEN")) {
        //                                        return (KnownColor)4282168177u;
        //                                    }
        //                                }
        //                            }
        //                            else {
        //                                if (text.Equals("LIGHTSLATEGRAY")) {
        //                                    return (KnownColor)4286023833u;
        //                                }
        //                                if (text.Equals("LIGHTSTEELBLUE")) {
        //                                    return (KnownColor)4289774814u;
        //                                }
        //                            }
        //                            break;
        //                    }
        //                    break;
        //                }
        //            case 15:
        //                if (text.Equals("MEDIUMSLATEBLUE")) {
        //                    return (KnownColor)4286277870u;
        //                }
        //                if (text.Equals("MEDIUMTURQUOISE")) {
        //                    return (KnownColor)4282962380u;
        //                }
        //                if (text.Equals("MEDIUMVIOLETRED")) {
        //                    return (KnownColor)4291237253u;
        //                }
        //                break;
        //            case 16:
        //                if (text.Equals("MEDIUMAQUAMARINE")) {
        //                    return (KnownColor)4284927402u;
        //                }
        //                break;
        //            case 17:
        //                if (text.Equals("MEDIUMSPRINGGREEN")) {
        //                    return (KnownColor)4278254234u;
        //                }
        //                break;
        //            case 20:
        //                if (text.Equals("LIGHTGOLDENRODYELLOW")) {
        //                    return (KnownColor)4294638290u;
        //                }
        //                break;
        //        }
        //    }
        //    return KnownColor.UnknownColor;
        //}

        //// Token: 0x060026B3 RID: 9907 RVA: 0x0009E2D8 File Offset: 0x0009D6D8
        //internal static KnownColor ArgbStringToKnownColor(string argbString) {
        //    string key = argbString.Trim().ToUpper(CultureInfo.InvariantCulture);
        //    KnownColor result;
        //    if (KnownColors.s_knownArgbColors.TryGetValue(key, out result)) {
        //        return result;
        //    }
        //    return KnownColor.UnknownColor;
        //}

        // Token: 0x0400106E RID: 4206
        private static Dictionary<uint, SolidColorBrush> s_solidColorBrushCache = new Dictionary<uint, SolidColorBrush>();

        // Token: 0x0400106F RID: 4207
        private static Dictionary<string, KnownColor> s_knownArgbColors = new Dictionary<string, KnownColor>();
    }
}
