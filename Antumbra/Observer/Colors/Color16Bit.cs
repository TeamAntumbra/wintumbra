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

        public Color16Bit()
        {
            this.red = 0;
            this.green = 0;
            this.blue = 0;
        }

        public Color16Bit(Color color)
        {
            this.red = ByteToUInt16(color.R);
            this.green = ByteToUInt16(color.G);
            this.blue = ByteToUInt16(color.B);
        }

        public Color16Bit(UInt16 r, UInt16 g, UInt16 b)
        {
            this.red = r;
            this.green = g;
            this.blue = b;
        }

        private UInt16 ByteToUInt16(byte target)
        {
            return Convert.ToUInt16((target / 255.0) * UInt16.MaxValue);
        }

        public Color ToRGBColor()
        {
            byte r = Convert.ToByte(this.red/UInt16.MaxValue);
            byte g = Convert.ToByte(this.green/UInt16.MaxValue);
            byte b = Convert.ToByte(this.blue/UInt16.MaxValue);
            return Color.FromArgb(r, g, b);
        }
    }
}
