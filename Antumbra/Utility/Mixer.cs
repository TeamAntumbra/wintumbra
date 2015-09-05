using Antumbra.Glow.Observer.Colors;
using System;

namespace Antumbra.Glow.Utility {

    public static class Mixer {

        #region Public Methods

        public static Color16Bit MixColorPercIn(Color16Bit newColor, Color16Bit prevColor, double newWeight) {
            double prevWeight = 1.00 - newWeight;
            Color16Bit result;
            result.red = Convert.ToUInt16(prevColor.red * prevWeight + newColor.red * newWeight);
            result.green = Convert.ToUInt16(prevColor.green * prevWeight + newColor.green * newWeight);
            result.blue = Convert.ToUInt16(prevColor.blue * prevWeight + newColor.blue * newWeight);
            return result;
        }

        #endregion Public Methods
    }
}
