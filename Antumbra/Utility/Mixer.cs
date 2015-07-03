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
    }
}
