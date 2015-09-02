using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.Utility {
    public static class Mixer {
        public static Color16Bit MixColorPercIn(Color16Bit newColor, Color16Bit prevColor, double newWeight) {
            double prevWeight = 1.00 - newWeight;
            Color16Bit result;
            result.red = Convert.ToUInt16((int)(prevColor.red * prevWeight) + (int)(newColor.red * newWeight));
            result.green = Convert.ToUInt16((int)(prevColor.green * prevWeight) + (int)(newColor.green * newWeight));
            result.blue = Convert.ToUInt16((int)(prevColor.blue * prevWeight) + (int)(newColor.blue * newWeight));
            return result;
        }
    }
}
