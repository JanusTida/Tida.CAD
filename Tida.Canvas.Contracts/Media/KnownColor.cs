using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tida.Canvas.Media {
    internal enum KnownColor : uint {
        // Token: 0x04000FE0 RID: 4064
        AliceBlue = 4293982463u,
        // Token: 0x04000FE1 RID: 4065
        AntiqueWhite = 4294634455u,
        // Token: 0x04000FE2 RID: 4066
        Aqua = 4278255615u,
        // Token: 0x04000FE3 RID: 4067
        Aquamarine = 4286578644u,
        // Token: 0x04000FE4 RID: 4068
        Azure = 4293984255u,
        // Token: 0x04000FE5 RID: 4069
        Beige = 4294309340u,
        // Token: 0x04000FE6 RID: 4070
        Bisque = 4294960324u,
        // Token: 0x04000FE7 RID: 4071
        Black = 4278190080u,
        // Token: 0x04000FE8 RID: 4072
        BlanchedAlmond = 4294962125u,
        // Token: 0x04000FE9 RID: 4073
        Blue = 4278190335u,
        // Token: 0x04000FEA RID: 4074
        BlueViolet = 4287245282u,
        // Token: 0x04000FEB RID: 4075
        Brown = 4289014314u,
        // Token: 0x04000FEC RID: 4076
        BurlyWood = 4292786311u,
        // Token: 0x04000FED RID: 4077
        CadetBlue = 4284456608u,
        // Token: 0x04000FEE RID: 4078
        Chartreuse = 4286578432u,
        // Token: 0x04000FEF RID: 4079
        Chocolate = 4291979550u,
        // Token: 0x04000FF0 RID: 4080
        Coral = 4294934352u,
        // Token: 0x04000FF1 RID: 4081
        CornflowerBlue = 4284782061u,
        // Token: 0x04000FF2 RID: 4082
        Cornsilk = 4294965468u,
        // Token: 0x04000FF3 RID: 4083
        Crimson = 4292613180u,
        // Token: 0x04000FF4 RID: 4084
        Cyan = 4278255615u,
        // Token: 0x04000FF5 RID: 4085
        DarkBlue = 4278190219u,
        // Token: 0x04000FF6 RID: 4086
        DarkCyan = 4278225803u,
        // Token: 0x04000FF7 RID: 4087
        DarkGoldenrod = 4290283019u,
        // Token: 0x04000FF8 RID: 4088
        DarkGray = 4289309097u,
        // Token: 0x04000FF9 RID: 4089
        DarkGreen = 4278215680u,
        // Token: 0x04000FFA RID: 4090
        DarkKhaki = 4290623339u,
        // Token: 0x04000FFB RID: 4091
        DarkMagenta = 4287299723u,
        // Token: 0x04000FFC RID: 4092
        DarkOliveGreen = 4283788079u,
        // Token: 0x04000FFD RID: 4093
        DarkOrange = 4294937600u,
        // Token: 0x04000FFE RID: 4094
        DarkOrchid = 4288230092u,
        // Token: 0x04000FFF RID: 4095
        DarkRed = 4287299584u,
        // Token: 0x04001000 RID: 4096
        DarkSalmon = 4293498490u,
        // Token: 0x04001001 RID: 4097
        DarkSeaGreen = 4287609999u,
        // Token: 0x04001002 RID: 4098
        DarkSlateBlue = 4282924427u,
        // Token: 0x04001003 RID: 4099
        DarkSlateGray = 4281290575u,
        // Token: 0x04001004 RID: 4100
        DarkTurquoise = 4278243025u,
        // Token: 0x04001005 RID: 4101
        DarkViolet = 4287889619u,
        // Token: 0x04001006 RID: 4102
        DeepPink = 4294907027u,
        // Token: 0x04001007 RID: 4103
        DeepSkyBlue = 4278239231u,
        // Token: 0x04001008 RID: 4104
        DimGray = 4285098345u,
        // Token: 0x04001009 RID: 4105
        DodgerBlue = 4280193279u,
        // Token: 0x0400100A RID: 4106
        Firebrick = 4289864226u,
        // Token: 0x0400100B RID: 4107
        FloralWhite = 4294966000u,
        // Token: 0x0400100C RID: 4108
        ForestGreen = 4280453922u,
        // Token: 0x0400100D RID: 4109
        Fuchsia = 4294902015u,
        // Token: 0x0400100E RID: 4110
        Gainsboro = 4292664540u,
        // Token: 0x0400100F RID: 4111
        GhostWhite = 4294506751u,
        // Token: 0x04001010 RID: 4112
        Gold = 4294956800u,
        // Token: 0x04001011 RID: 4113
        Goldenrod = 4292519200u,
        // Token: 0x04001012 RID: 4114
        Gray = 4286611584u,
        // Token: 0x04001013 RID: 4115
        Green = 4278222848u,
        // Token: 0x04001014 RID: 4116
        GreenYellow = 4289593135u,
        // Token: 0x04001015 RID: 4117
        Honeydew = 4293984240u,
        // Token: 0x04001016 RID: 4118
        HotPink = 4294928820u,
        // Token: 0x04001017 RID: 4119
        IndianRed = 4291648604u,
        // Token: 0x04001018 RID: 4120
        Indigo = 4283105410u,
        // Token: 0x04001019 RID: 4121
        Ivory = 4294967280u,
        // Token: 0x0400101A RID: 4122
        Khaki = 4293977740u,
        // Token: 0x0400101B RID: 4123
        Lavender = 4293322490u,
        // Token: 0x0400101C RID: 4124
        LavenderBlush = 4294963445u,
        // Token: 0x0400101D RID: 4125
        LawnGreen = 4286381056u,
        // Token: 0x0400101E RID: 4126
        LemonChiffon = 4294965965u,
        // Token: 0x0400101F RID: 4127
        LightBlue = 4289583334u,
        // Token: 0x04001020 RID: 4128
        LightCoral = 4293951616u,
        // Token: 0x04001021 RID: 4129
        LightCyan = 4292935679u,
        // Token: 0x04001022 RID: 4130
        LightGoldenrodYellow = 4294638290u,
        // Token: 0x04001023 RID: 4131
        LightGreen = 4287688336u,
        // Token: 0x04001024 RID: 4132
        LightGray = 4292072403u,
        // Token: 0x04001025 RID: 4133
        LightPink = 4294948545u,
        // Token: 0x04001026 RID: 4134
        LightSalmon = 4294942842u,
        // Token: 0x04001027 RID: 4135
        LightSeaGreen = 4280332970u,
        // Token: 0x04001028 RID: 4136
        LightSkyBlue = 4287090426u,
        // Token: 0x04001029 RID: 4137
        LightSlateGray = 4286023833u,
        // Token: 0x0400102A RID: 4138
        LightSteelBlue = 4289774814u,
        // Token: 0x0400102B RID: 4139
        LightYellow = 4294967264u,
        // Token: 0x0400102C RID: 4140
        Lime = 4278255360u,
        // Token: 0x0400102D RID: 4141
        LimeGreen = 4281519410u,
        // Token: 0x0400102E RID: 4142
        Linen = 4294635750u,
        // Token: 0x0400102F RID: 4143
        Magenta = 4294902015u,
        // Token: 0x04001030 RID: 4144
        Maroon = 4286578688u,
        // Token: 0x04001031 RID: 4145
        MediumAquamarine = 4284927402u,
        // Token: 0x04001032 RID: 4146
        MediumBlue = 4278190285u,
        // Token: 0x04001033 RID: 4147
        MediumOrchid = 4290401747u,
        // Token: 0x04001034 RID: 4148
        MediumPurple = 4287852763u,
        // Token: 0x04001035 RID: 4149
        MediumSeaGreen = 4282168177u,
        // Token: 0x04001036 RID: 4150
        MediumSlateBlue = 4286277870u,
        // Token: 0x04001037 RID: 4151
        MediumSpringGreen = 4278254234u,
        // Token: 0x04001038 RID: 4152
        MediumTurquoise = 4282962380u,
        // Token: 0x04001039 RID: 4153
        MediumVioletRed = 4291237253u,
        // Token: 0x0400103A RID: 4154
        MidnightBlue = 4279834992u,
        // Token: 0x0400103B RID: 4155
        MintCream = 4294311930u,
        // Token: 0x0400103C RID: 4156
        MistyRose = 4294960353u,
        // Token: 0x0400103D RID: 4157
        Moccasin = 4294960309u,
        // Token: 0x0400103E RID: 4158
        NavajoWhite = 4294958765u,
        // Token: 0x0400103F RID: 4159
        Navy = 4278190208u,
        // Token: 0x04001040 RID: 4160
        OldLace = 4294833638u,
        // Token: 0x04001041 RID: 4161
        Olive = 4286611456u,
        // Token: 0x04001042 RID: 4162
        OliveDrab = 4285238819u,
        // Token: 0x04001043 RID: 4163
        Orange = 4294944000u,
        // Token: 0x04001044 RID: 4164
        OrangeRed = 4294919424u,
        // Token: 0x04001045 RID: 4165
        Orchid = 4292505814u,
        // Token: 0x04001046 RID: 4166
        PaleGoldenrod = 4293847210u,
        // Token: 0x04001047 RID: 4167
        PaleGreen = 4288215960u,
        // Token: 0x04001048 RID: 4168
        PaleTurquoise = 4289720046u,
        // Token: 0x04001049 RID: 4169
        PaleVioletRed = 4292571283u,
        // Token: 0x0400104A RID: 4170
        PapayaWhip = 4294963157u,
        // Token: 0x0400104B RID: 4171
        PeachPuff = 4294957753u,
        // Token: 0x0400104C RID: 4172
        Peru = 4291659071u,
        // Token: 0x0400104D RID: 4173
        Pink = 4294951115u,
        // Token: 0x0400104E RID: 4174
        Plum = 4292714717u,
        // Token: 0x0400104F RID: 4175
        PowderBlue = 4289781990u,
        // Token: 0x04001050 RID: 4176
        Purple = 4286578816u,
        // Token: 0x04001051 RID: 4177
        Red = 4294901760u,
        // Token: 0x04001052 RID: 4178
        RosyBrown = 4290547599u,
        // Token: 0x04001053 RID: 4179
        RoyalBlue = 4282477025u,
        // Token: 0x04001054 RID: 4180
        SaddleBrown = 4287317267u,
        // Token: 0x04001055 RID: 4181
        Salmon = 4294606962u,
        // Token: 0x04001056 RID: 4182
        SandyBrown = 4294222944u,
        // Token: 0x04001057 RID: 4183
        SeaGreen = 4281240407u,
        // Token: 0x04001058 RID: 4184
        SeaShell = 4294964718u,
        // Token: 0x04001059 RID: 4185
        Sienna = 4288696877u,
        // Token: 0x0400105A RID: 4186
        Silver = 4290822336u,
        // Token: 0x0400105B RID: 4187
        SkyBlue = 4287090411u,
        // Token: 0x0400105C RID: 4188
        SlateBlue = 4285160141u,
        // Token: 0x0400105D RID: 4189
        SlateGray = 4285563024u,
        // Token: 0x0400105E RID: 4190
        Snow = 4294966010u,
        // Token: 0x0400105F RID: 4191
        SpringGreen = 4278255487u,
        // Token: 0x04001060 RID: 4192
        SteelBlue = 4282811060u,
        // Token: 0x04001061 RID: 4193
        Tan = 4291998860u,
        // Token: 0x04001062 RID: 4194
        Teal = 4278222976u,
        // Token: 0x04001063 RID: 4195
        Thistle = 4292394968u,
        // Token: 0x04001064 RID: 4196
        Tomato = 4294927175u,
        // Token: 0x04001065 RID: 4197
        Transparent = 16777215u,
        // Token: 0x04001066 RID: 4198
        Turquoise = 4282441936u,
        // Token: 0x04001067 RID: 4199
        Violet = 4293821166u,
        // Token: 0x04001068 RID: 4200
        Wheat = 4294303411u,
        // Token: 0x04001069 RID: 4201
        White = 4294967295u,
        // Token: 0x0400106A RID: 4202
        WhiteSmoke = 4294309365u,
        // Token: 0x0400106B RID: 4203
        Yellow = 4294967040u,
        // Token: 0x0400106C RID: 4204
        YellowGreen = 4288335154u,
        // Token: 0x0400106D RID: 4205
        UnknownColor = 1u
    }
}
