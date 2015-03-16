using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Utility
{
    public class UniqueColorGenerator
    {
        private static UniqueColorGenerator instance;
        public static UniqueColorGenerator GetInstance()
        {
            if (instance == null)
                instance = new UniqueColorGenerator();
            return instance;
        }
        private List<HslColor> assigned;
        public UniqueColorGenerator()
        {
            this.assigned = new List<HslColor>();
        }

        public Color GetUniqueColor()
        {
            Color result = Color.Empty;
            List<double> hues = new List<double>();
            foreach (HslColor col in this.assigned)
                hues.Add(col.H);
            int count = hues.Count;
            if (count == 0) {
                result = new HslColor(0, 1.0, .5);
            }
            else {
                double prev = 0;
                double gapStart = 0;
                double largestGap = 180.0;
                for (int i = 1; i < count; i += 1) {
                    double current = hues[i];
                    double gap = Math.Abs(prev-current);
                    if (gap > largestGap) {
                        largestGap = gap;
                        gapStart = current;
                    }
                }
                result = new HslColor(gapStart + (largestGap / 2.0), 1.0, .5);
            }
            this.assigned.Add(result);
            return result;
        }
    }
}
