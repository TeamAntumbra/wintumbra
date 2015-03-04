using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Utility
{
    public static class Mixer
    {
        public static Color MixColorPercIn(Color newColor, Color prevColor, double newWeight)
        {
            if (prevColor == null)
                prevColor = Color.Black;
            if (prevColor.Equals(Color.Empty))
                return newColor;
            double prevWeight = 1.00 - newWeight;
            int newR = (int)(prevColor.R * prevWeight) + (int)(newColor.R * newWeight);
            int newG = (int)(prevColor.G * prevWeight) + (int)(newColor.G * newWeight);
            int newB = (int)(prevColor.B * prevWeight) + (int)(newColor.B * newWeight);
            if (newR > 255)
                newR = 255;
            if (newG > 255)
                newG = 255;
            if (newB > 255)
                newB = 255;
            return Color.FromArgb(newR, newG, newB);
        }

        public static Color Interpolate(Color color1, Color color2, double fraction)
        {
            double r = Interpolate(color1.R, color2.R, fraction);
            double g = Interpolate(color1.G, color2.G, fraction);
            double b = Interpolate(color1.B, color2.B, fraction);
            int rI = (int)Math.Round(r) % 255;
            int gI = (int)Math.Round(g) % 255;
            int bI = (int)Math.Round(b) % 255;
            if (rI < 0)
                rI = 0;
            if (gI < 0)
                gI = 0;
            if (bI < 0)
                bI = 0;
            return Color.FromArgb(rI, gI, bI);
        }

        private static double Interpolate(double d1, double d2, double fraction)
        {
            return d1 + (d1 - d2) * fraction;
        }
    }
}
