using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Antumbra.Glow.ExtensionFramework.ScreenProcessors
{
    public class AntumbraScreenProcessor : ScreenProcessor
    {
        public double saturationAdditive { get; set; } //TODO make settings for these
        public bool saturationEnabled { get; set; }
        public int useAllTol { get; set; }
        public int minMixPerc { get; set; }
        public AntumbraScreenProcessor(double satAdd, bool satEnabled, int useAllTol, int minMixPerc) {
            this.saturationAdditive = satAdd;//+(satAdd*100)% saturation (caps @ 100% (1.0))
            this.saturationEnabled = true;
            this.useAllTol = useAllTol;
            this.minMixPerc = minMixPerc;
        }
        public Color Process(Bitmap screen) {
            if (screen == null) {
                Console.WriteLine("null BitMap returned");
                return Color.Empty;
            }
            return SmartCalculateReprColor(screen, this.useAllTol, this.minMixPerc);
        }

        private Color SmartCalculateReprColor(Bitmap bm, int useAllTolerance, int mixPercThreshold)
        {
            int width = bm.Width;
            int height = bm.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            //int minDiversion = 15; // drop pixels that do not differ by at least minDiversion between color values (white, gray or black)
            //int dropped = 0; // keep track of dropped pixels
            long[] blues = new long[] { 0, 0, 0 };
            long[] greens = new long[] { 0, 0, 0 };
            long[] reds = new long[] { 0, 0, 0 };
            long[] all = new long[] { 0, 0, 0 }; 
            int bppModifier = bm.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4; // cutting corners, will fail on anything else but 32 and 24 bit images

            BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
            int stride = srcData.Stride;
            IntPtr Scan0 = srcData.Scan0;
            int bluesCount = 0, greensCount = 0, redsCount = 0;

            unsafe {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++) { //for each row
                    for (int x = 0; x < width; x++) { //for each col
                        int idx = (y * stride) + x * bppModifier;
                        red = p[idx + 2];
                        green = p[idx + 1];
                        blue = p[idx];
                        int max = Math.Max(blue, Math.Max(green, red));
                        if (blue == max) {//blue dominant
                            blues[2] += red;
                            blues[1] += green;
                            blues[0] += blue;
                            bluesCount += 1;
                        }
                        else if (green == max) {//green dominant
                            greens[2] += red;
                            greens[1] += green;
                            greens[0] += blue;
                            greensCount += 1;
                        }
                        else if (red == max) {//red dominant
                            reds[2] += red;
                            reds[1] += green;
                            reds[0] += blue;
                            redsCount += 1;
                        }
                        else {
                            Console.WriteLine("this should not happen! (in getReprColor)");
                        }
                        all[2] += red;
                        all[1] += green;
                        all[0] += blue;
                    }
                }
            }
            long[] totals = new long[] { 0, 0, 0 };
            int count = Math.Max(bluesCount, Math.Max(greensCount, redsCount));
            if (Math.Abs(bluesCount - greensCount) < useAllTolerance && Math.Abs(bluesCount - redsCount) < useAllTolerance && Math.Abs(greensCount - redsCount) < useAllTolerance)
                totals = all;
            else if (bluesCount >= greensCount && bluesCount >= redsCount) {
                totals = blues;
                double mixThreshold = bluesCount * (mixPercThreshold / 100.0);
                if (redsCount > mixThreshold) { //mix in red
                    totals[2] += reds[2];
                    totals[1] += reds[1];
                    totals[0] += reds[0];
                    count += redsCount;
                }
                if (greensCount > mixThreshold) { //mix in green
                    totals[2] += greens[2];
                    totals[1] += greens[1];
                    totals[0] += greens[0];
                    count += greensCount;
                }
            }
            else if (greensCount >= bluesCount && greensCount >= redsCount) {
                totals = greens;
                double mixThreshold = greensCount * (mixPercThreshold / 100.0);
                if (redsCount > mixThreshold) { //mix in red
                    totals[2] += reds[2];
                    totals[1] += reds[1];
                    totals[0] += reds[0];
                    count += redsCount;
                }
                if (bluesCount > mixThreshold) { //mix in blue
                    totals[2] += blues[2];
                    totals[1] += blues[1];
                    totals[0] += blues[0];
                    count += bluesCount;
                }
            }
            else if (redsCount >= bluesCount && redsCount >= greensCount) {
                totals = reds;
                double mixThreshold = redsCount * (mixPercThreshold / 100.0);
                if (bluesCount > mixThreshold) { //mix in blue
                    totals[2] += blues[2];
                    totals[1] += blues[1];
                    totals[0] += blues[0];
                    count += bluesCount;
                }
                if (greensCount > mixThreshold) { //mix in green
                    totals[2] += greens[2];
                    totals[1] += greens[1];
                    totals[0] += greens[0];
                    count += greensCount;
                }
            }
            else
                Console.WriteLine("this should not happen! (in getReprColor) #2");
            //int count = width * height; //total number of pixels in avgs
            int avgR = (int)(totals[2] / count);
            int avgG = (int)(totals[1] / count);
            int avgB = (int)(totals[0] / count);
            System.Drawing.Color newColor = System.Drawing.Color.FromArgb(avgR, avgG, avgB);
            if (this.saturationEnabled)
                return intensify(newColor);
            return newColor;
        }

        public System.Drawing.Color intensify(System.Drawing.Color boringColor)
        {
            HslColor boringHSL = new HslColor(boringColor);
            if (boringHSL.S <= .65)
                boringHSL.S += this.saturationAdditive; //saturate
            else
                boringHSL.S = 1.0;
            return boringHSL.ToRgbColor();
        }
    }
}
