using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Utility
{
    public static class Mixer
    {
        public static Color16Bit MixColorPercIn(Color16Bit newColor, Color16Bit prevColor, double newWeight)
        {
            if (prevColor == null)
                return newColor;
            double prevWeight = 1.00 - newWeight;
            UInt16 newR = Convert.ToUInt16((int)(prevColor.red * prevWeight) + (int)(newColor.red * newWeight));
            UInt16 newG = Convert.ToUInt16((int)(prevColor.green * prevWeight) + (int)(newColor.green * newWeight));
            UInt16 newB = Convert.ToUInt16((int)(prevColor.blue * prevWeight) + (int)(newColor.blue * newWeight));
            return new Color16Bit(newR, newG, newB);
        }

        public static Color16Bit Interpolate(Color16Bit color1, Color16Bit color2, double fraction)
        {
            double r = Interpolate(color1.red, color2.red, fraction);
            double g = Interpolate(color1.green, color2.green, fraction);
            double b = Interpolate(color1.blue, color2.blue, fraction);
            UInt16 rI = Convert.ToUInt16((int)Math.Round(r) % 255);
            UInt16 gI = Convert.ToUInt16((int)Math.Round(g) % 255);
            UInt16 bI = Convert.ToUInt16((int)Math.Round(b) % 255);
            if (rI < 0)
                rI = 0;
            if (gI < 0)
                gI = 0;
            if (bI < 0)
                bI = 0;
            return new Color16Bit(rI, gI, bI);
        }

        private static double Interpolate(double d1, double d2, double fraction)
        {
            return d1 + (d1 - d2) * fraction;
        }
    }
}
