using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Interop;


namespace Antumbra
{
    public class ScreenGrabber
    {
        //Being DLL declarations
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        //BitBlt - used to get screen info in an efficent manner

        public int width, height;//screen width and height
        //int widthDivs, heightDivs;//width and height divisions for screen polling
        int x, y;//x and y bounds of screen
        System.Drawing.Point[] points;//polling points
        System.Drawing.Size size;//polling size
        public Screen display { get; set; }//display for avging
        public double saturationAdditive { get; set; }
        public bool saturationEnabled { get; set; }

        public ScreenGrabber()
        {
            this.size = new System.Drawing.Size(50, 30);
            this.display = Screen.PrimaryScreen;
            this.saturationAdditive = .45;//+45% saturation (caps @ 100%)
            this.saturationEnabled = true;
            updateBounds();
            //this.widthDivs = 4;
            //this.heightDivs = 4;
        }

        public void updateBounds()
        {
            this.width = this.display.Bounds.Width;
            this.height = this.display.Bounds.Height;
            this.x = this.display.Bounds.X;
            this.y = this.display.Bounds.Y;
            //this.points = getPollingPoints(this.width, this.height, this.widthDivs, this.heightDivs);
            this.points = getPollingRectPoints(this.width, this.height);

        }

        public System.Drawing.Color getScreenAvgColor(int width, int height)
        {
            if (this.width == 0 || this.height == 0)
                return System.Drawing.Color.Black;
            Bitmap screen = getPixelBitBlt(width, height);
            if (screen == null)
                return System.Drawing.Color.Black;
            System.Drawing.Color result = SmartCalculateReprColor(screen, 10, 20);//use all tolerance, and percent needed to be mixed in
            screen.Dispose();
            return result;
        }

        private System.Drawing.Color SmartCalculateReprColor(Bitmap bm, int useAllTolerance, int mixPercThreshold)
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

        private Bitmap getPixelBitBlt(int width, int height)
        {
           Bitmap screenPixel = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics gdest = Graphics.FromImage(screenPixel)) {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, 0, 0, (int)0x00CC0020);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            return screenPixel;
        }

        private Bitmap getPixelBitBlt(System.Drawing.Point topLeft, System.Drawing.Point botRight)
        {
            int width = botRight.X - topLeft.X;
            int height = botRight.Y - topLeft.Y;
            Bitmap screenPixel = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            using (Graphics gdest = Graphics.FromImage(screenPixel)) {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, topLeft.X, topLeft.Y, width, height, hSrcDC, 0, 0, (int)0x00CC0020);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            return screenPixel;
        }

        private System.Drawing.Point[] getPollingRectPoints(float width, float height)
        {
            System.Drawing.Point topLeft = new System.Drawing.Point((int)(width * .125), (int)(height * .125));
            System.Drawing.Point topRight = new System.Drawing.Point((int)(width * .875), (int)(height * .125));
            System.Drawing.Point botLeft = new System.Drawing.Point((int)(width * .125), (int)(height * .875));
            System.Drawing.Point botRight = new System.Drawing.Point((int)(width * .875), (int)(height * .875));
            System.Drawing.Point[] result = { topLeft, topRight, botLeft, botRight };
            return result;
        }

        private System.Drawing.Point[] getPollingPoints(float width, float height, int widthDivs, int heightDivs)
        {           //TODO make this only called when changed in settings (if we even use this in the future)
            List<System.Drawing.Point> points = new List<System.Drawing.Point>();
            float hStep = height / heightDivs;
            float wStep = width / widthDivs;
            for (float y = hStep; y < height; y += hStep) {
                for (float x = wStep; x < width; x += wStep) {
                    //Console.WriteLine(x.ToString() + " " + y.ToString());//debug print
                    points.Add(new System.Drawing.Point((int)x, (int)y));
                }
            }
            return points.ToArray();
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
