using System;
using System.Collections.Generic;
using System.Drawing;

namespace Antumbra.Glow.Utility {

    public class UniqueColorGenerator {

        #region Private Fields

        private static UniqueColorGenerator instance;
        private List<HslColor> assigned;

        #endregion Private Fields

        #region Private Constructors

        private UniqueColorGenerator() {
            this.assigned = new List<HslColor>();
        }

        #endregion Private Constructors

        #region Public Methods

        public static UniqueColorGenerator GetInstance() {
            if(instance == null)
                instance = new UniqueColorGenerator();
            return instance;
        }

        public Color GetUniqueColor() {
            Color result = Color.Empty;
            List<double> hues = new List<double>();
            foreach(HslColor col in this.assigned)
                hues.Add(col.H);
            int count = hues.Count;
            if(count == 0) {
                result = new HslColor(0, 1.0, .5);
            } else {
                double prev = hues[0];
                double gapStart = prev;
                double largestGap = 0.0;
                double current = 0, gap = 0;
                for(int i = 1; i < count; i += 1) {
                    current = hues[i];
                    gap = Math.Abs(prev - current);
                    if(gap > largestGap) {
                        largestGap = gap;
                        gapStart = prev;
                    }
                    prev = current;
                }
                current = 360.0;
                gap = Math.Abs(prev - current);
                if(gap > largestGap) {
                    largestGap = gap;
                    gapStart = prev;
                }
                result = new HslColor(gapStart + (largestGap / 2.0), 1.0, .5);
            }
            this.assigned.Add(result);
            this.assigned.Sort((x, y) => x.H.CompareTo(y.H));
            return result;
        }

        public void RetireUniqueColor(Color color) {
            this.assigned.Remove(color);
        }

        #endregion Public Methods
    }
}
