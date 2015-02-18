using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Antumbra.Glow.Utility
{
    public static class HSVRGGConverter
    {
        public static double[] RGBToHSV(int r, int g, int b)
        {
            int max = Math.Max(r, Math.Max(g, b));
            int min = Math.Min(r, Math.Min(g, b));

            double[] result = new double[3];
            result[0] = Color.FromArgb(r, g, b).GetHue();
            result[1] = (max == 0) ? 0 : 1d - (1d * min / max);
            result[2] = max / 255d;
            return result;
        }
        
        public static int[] HSVToRGB(double h, double S, double V)//from here to... \/ \/ \/
        {
            int[] result = new int[3];
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0) { R = G = B = 0; }
            else if (S <= 0) {
                R = G = B = V;
            }
            else {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i) {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            result[0] = Clamp((int)(R * 255.0));
            result[1] = Clamp((int)(G * 255.0));
            result[2] = Clamp((int)(B * 255.0));
            return result;
        }

        private static   int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }//here is taken from StackOverflow @ https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
    }
}
