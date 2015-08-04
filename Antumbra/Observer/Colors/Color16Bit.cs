using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Observer.Colors
{
    public class Color16Bit
    {
        public UInt16 red { get; private set; }
        public UInt16 green { get; private set; }
        public UInt16 blue { get; private set; }

        /// <summary>
        /// Constructor (black return value)
        /// </summary>
        public Color16Bit()
        {
            this.red = 0;
            this.green = 0;
            this.blue = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">System.Drawing.Color to use for construction</param>
        public Color16Bit(Color color)
        {
            this.red = Convert.ToUInt16(color.R << 8);
            this.green = Convert.ToUInt16(color.G << 8);
            this.blue = Convert.ToUInt16(color.B << 8);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="r">Red value</param>
        /// <param name="g">Green value</param>
        /// <param name="b">Blue value</param>
        public Color16Bit(UInt16 r, UInt16 g, UInt16 b)
        {
            this.red = r;
            this.green = g;
            this.blue = b;
        }

        /// <summary>
        /// Converts the color to a System.Drawing.Color object
        /// </summary>
        /// <returns>System.Drawing.Color color equivalent</returns>
        public Color ToRGBColor()
        {
            byte r = Convert.ToByte(this.red >> 8);
            byte g = Convert.ToByte(this.green >> 8);
            byte b = Convert.ToByte(this.blue >> 8);
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Get a string representation of the color
        /// </summary>
        /// <returns>String representing the color</returns>
        public override string ToString()
        {
            return "R: " + red + " G: " + green + " B: " + blue;
        }

        /// <summary>
        /// Get the average brightness of a color's attributes
        /// </summary>
        /// <returns>Average of color attributes</returns>
        public UInt16 GetAvgBrightness()
        {
            int avg = 0;
            avg += red + green + blue;
            return Convert.ToUInt16(avg / 3);
        }

        /// <summary>
        /// Scales the attributes of a color based on a scaleFactor
        /// </summary>
        /// <param name="scaleFactor">Factor to scale the color with.  Must be between 0 and 1.0</param>
        public void ScaleColor(double scaleFactor)
        {
            if (scaleFactor < 0 || scaleFactor > 1.0) {
                throw new ArgumentException("Scale Factor out of range! Passed value: " + scaleFactor);
            }
            red = Convert.ToUInt16(red * scaleFactor);
            green = Convert.ToUInt16(green * scaleFactor);
            blue = Convert.ToUInt16(blue * scaleFactor);
        }
    }
}
