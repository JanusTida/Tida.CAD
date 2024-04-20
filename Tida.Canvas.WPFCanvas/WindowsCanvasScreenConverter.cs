using Tida.Canvas.Contracts;
using Tida.Geometry.Primitives;
using System;

namespace Tida.Canvas.WPFCanvas {
    /// <summary>
    /// <see cref="ICanvasScreenConvertable"/>的一个实现;
    /// </summary>
    public class WindowsCanvasScreenConverter : ICanvasScreenConvertable {
        /// <summary>
        /// Zoom为1.0时,工程数学单位和视图单位的比率;
        /// </summary>
        public const double ScreenResolution = 96;
        /// <summary>
        /// 默认缩放比例;
        /// </summary>
        public const double DefaultZoom = 1;

        /// <summary>
        /// 当前缩放比例;
        /// </summary>
        private double _zoom = DefaultZoom;
        public double Zoom {
            get => _zoom;
            set {
                if(value <= 0) {
                    throw new ArgumentException($"{nameof(Zoom)} should be larger than zero.");
                }

                _zoom = value;
            }
        } 

        /// <summary>
        /// 原点所在视图位置;
        /// </summary>
        public Vector2D PanScreenPosition { get; set; }

        /// <summary>
        /// 实际视图宽度;
        /// </summary>
        public  double ActualWidth { get; set; }

        /// <summary>
        /// 实际视图高度;
        /// </summary>
        public double ActualHeight { get; set; }


        public double ToScreen(double unitValue) {
            return unitValue * Zoom * ScreenResolution;
        }

        public void ToScreen(Vector2D unitpoint,Vector2D screenPoint) {

            if (unitpoint == null) {
                throw new ArgumentNullException(nameof(unitpoint));
            }
            
            if (screenPoint == null) {
                throw new ArgumentNullException(nameof(screenPoint));
            }


            var screenX = ToScreen(unitpoint.X);
            var screenY = ToScreen(unitpoint.Y);

            screenPoint.X = screenX + PanScreenPosition?.X ?? 0D;
            screenPoint.Y = -screenY + PanScreenPosition?.Y ?? 0D;

        }

        public void ToUnit(Vector2D screenpoint,Vector2D unitPoint) {

            if (screenpoint == null) {
                throw new ArgumentNullException(nameof(screenpoint));
            }


            if (unitPoint == null) {
                throw new ArgumentNullException(nameof(unitPoint));
            }


            var unitX = ToUnit(screenpoint.X   -  PanScreenPosition?.X ?? 0D);
            var unitY = ToUnit(-screenpoint.Y +  PanScreenPosition?.Y ?? 0D);

            unitPoint.X = unitX;
            unitPoint.Y = unitY;
        }

        public double ToUnit(double screenvalue) {
            return screenvalue / (ScreenResolution * Zoom);
        }


        public Size GetCharScreenSize(char ch) {
            var glyphTypeFace = WindowsCanvas.GlyphTypeFace;
            if(glyphTypeFace == null) {
                return null;
            }

            if (glyphTypeFace.CharacterToGlyphMap.ContainsKey(ch)) {
                var glyphIndex = glyphTypeFace.CharacterToGlyphMap[ch];
                return  Size.Create( glyphTypeFace.AdvanceWidths[glyphIndex], glyphTypeFace.AdvanceHeights[glyphIndex]);
            }
            //若不包含该字符,返回首个字符的大小;
            else if(glyphTypeFace.CharacterToGlyphMap.Count > 0) {
                return Size.Create(glyphTypeFace.AdvanceWidths[0], glyphTypeFace.AdvanceHeights[0]);
            }

            return null;
        }
    }
}
