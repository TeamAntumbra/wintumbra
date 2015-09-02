using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Observer.Colors {
    public struct Color16Bit {
        public UInt16 red;
        public UInt16 green;
        public UInt16 blue;
    }

    public class Color16BitUtil {
        /// <summary>
        /// Converts the color to a System.Drawing.Color object
        /// </summary>
        /// <returns>System.Drawing.Color color equivalent</returns>
        public static Color ToRGBColor(Color16Bit color) {
            byte r = Convert.ToByte(color.red >> 8);
            byte g = Convert.ToByte(color.green >> 8);
            byte b = Convert.ToByte(color.blue >> 8);
            return Color.FromArgb(r, g, b);
        }

        public static Color16Bit FromRGBColor(Color color) {
            Color16Bit result;
            result.red = Convert.ToUInt16(color.R << 8);
            result.green = Convert.ToUInt16(color.G << 8);
            result.blue = Convert.ToUInt16(color.B << 8);
            return result;
        }

        /// <summary>
        /// Get the average brightness of a color's attributes
        /// </summary>
        /// <returns>Average of color attributes</returns>
        public static UInt16 GetAvgBrightness(Color16Bit color) {
            int avg = 0;
            avg += color.red + color.green + color.blue;
            return Convert.ToUInt16(avg / 3);
        }

        /// <summary>
        /// Scales the attributes of a color based on a scaleFactor
        /// </summary>
        /// <param name="scaleFactor">Factor to scale the color with.  Must be between 0 and 1.0 inclusive</param>
        public static void ScaleColor(Color16Bit color, double scaleFactor) {
            if(scaleFactor < 0 || scaleFactor > 1.0) {
                throw new ArgumentException("Scale Factor out of range! Passed value: " + scaleFactor);
            }
            color.red = Convert.ToUInt16(color.red * scaleFactor);
            color.green = Convert.ToUInt16(color.green * scaleFactor);
            color.blue = Convert.ToUInt16(color.blue * scaleFactor);
        }

        /// <summary>
        /// Funnel the values into UInt16 bounds and create a Color16Bit object
        /// </summary>
        /// <param name="r">Red attribute</param>
        /// <param name="g">Green attribute</param>
        /// <param name="b">Blue attribute</param>
        /// <returns>Color16Bit made from the funneled values</returns>
        public static Color16Bit FunnelIntoColor(int r, int g, int b) {
            Color16Bit result;
            result.red = Funnel(r);
            result.green = Funnel(g);
            result.blue = Funnel(b);
            return result;
        }

        /// <summary>
        /// Funnel value into UInt16 bounds
        /// </summary>
        /// <param name="value">The value to funnel</param>
        /// <returns>0 for negative values, 65535 for values larger than that, else the input value as a UInt16</returns>
        private static UInt16 Funnel(int value) {
            if(value < 0) {
                return 0;
            }
            if(value > UInt16.MaxValue) {
                return UInt16.MaxValue;
            }
            return Convert.ToUInt16(value);
        }
    }
}
