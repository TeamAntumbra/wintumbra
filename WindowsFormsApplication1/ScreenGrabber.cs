﻿using System;
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
    class ScreenGrabber
    {
        //Being DLL declarations
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        //BitBlt - used to get screen info in an efficent manner

        [DllImport("Gdi32.dll")]

        public static extern int GetPixel(
            System.IntPtr hdc,    // handle to DC
            int nXPos,  // x-coordinate of pixel
            int nYPos   // y-coordinate of pixel
        );

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr wnd);

        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hWnd, IntPtr hDc);
        //End DLL Declarations

        public int width, height;//screen width and height
        //int widthDivs, heightDivs;//width and height divisions for screen polling
        int x, y;//x and y bounds of screen
        System.Drawing.Point[] points;//polling points
        System.Drawing.Size size;//polling size
        //GPGPU gpu;

        public ScreenGrabber()
        {
            this.size = new System.Drawing.Size(50, 30);
            updateBounds();
            //this.widthDivs = 4;
            //this.heightDivs = 4;
            //setupCudafy();
        }

        public void updateBounds()
        {
            this.width = Screen.PrimaryScreen.Bounds.Width;
            this.height = Screen.PrimaryScreen.Bounds.Height;
            this.x = Screen.PrimaryScreen.Bounds.X;
            this.y = Screen.PrimaryScreen.Bounds.Y;
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
            System.Drawing.Color result = SmartCalculateReprColor(screen, 10, 25);//use all tolerance, and percent needed to be mixed in
            //return CalculateReprColor(screen, true);
            screen.Dispose();
            return result;
        }

        public System.Drawing.Color getCenterScreenAvgColor()
        {
            //Console.WriteLine(this.points[0].X + " " + this.points[0].Y + " " + this.points[3].X + " " + this.points[3].Y);
            Bitmap screen = getPixelBitBlt(this.points[0], this.points[3]);
            System.Drawing.Color result = CalculateReprColor(screen, true);
            screen.Dispose();
            return result;
        }

        public System.Drawing.Color getScreenDomColor()
        {
            Bitmap screen = getPixelBitBlt(this.points[0], this.points[3]);
            System.Drawing.Color result = CalculateDominantColor(screen);
            screen.Dispose();
            return result;
        }

        public System.Drawing.Color getCenterScreenDomColor()
        {
            if (this.width == 0 || this.height == 0)
                return System.Drawing.Color.Black;
            Bitmap screen = getPixelBitBlt(this.width, this.height);
            if (screen == null)
                return System.Drawing.Color.Black;
            System.Drawing.Color result = CalculateDominantColor(screen);
            screen.Dispose();
            return result;
        }

        public void bench()
        {
            Stopwatch s = new Stopwatch();
            /*s.Start();
            Bitmap screen = getScreenAsBitmap();
            s.Stop();
            Console.WriteLine("get screen as bitmap: " + s.ElapsedMilliseconds);
            s.Reset();
            s.Start();
            Color result = CalculateAverageColor(screen);
            s.Stop();
            Console.WriteLine(result.R + " " + result.G + " " + result.B);
            Console.WriteLine("calcAverageColor time: " + s.ElapsedMilliseconds);
            s.Reset();
            s.Start();
            result = getAvgFromPoints(screen);
            s.Stop();
            Console.WriteLine(result.R + " " + result.G + " " + result.B);
            Console.WriteLine("getAvgFromPoints time: " + s.ElapsedMilliseconds);
            s.Reset();*/
            s.Start();
            Bitmap screen = getPixelBitBlt(this.width, this.height);
            if (screen == null)
                return;
            System.Drawing.Color result = CalculateReprColor(screen, true);
            s.Stop();
            Console.WriteLine(result.R + " " + result.G + " " + result.B);
            Console.WriteLine("process time: " + s.ElapsedMilliseconds);
        }

        public System.Drawing.Color getAvgFromPoints(Bitmap screen) 
        {
            int r = 0, g = 0, b = 0;
            foreach (System.Drawing.Point pnt in this.points) {
                Bitmap section = getSectionOf(screen, pnt, this.size);
                System.Drawing.Color each = getAvgFromBitmap(section);
                r += each.R;
                g += each.G;
                b += each.B;
            }
            int div = this.points.Length;
            return System.Drawing.Color.FromArgb(r / div, g / div, b / div);
        }

        private Bitmap getScreenAsBitmap()
        {
            Bitmap result = new Bitmap(this.width, this.height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics gfxScreenshot = Graphics.FromImage(result))
                gfxScreenshot.CopyFromScreen(this.x, this.y, 0, 0, new System.Drawing.Size(this.width, this.height));//, CopyPixelOperation.SourceCopy);
            return result;
        }

        private System.Drawing.Color getPixel(int x, int y)//get just one pixel from system
        {
            IntPtr dc = GetDC(IntPtr.Zero);
            System.Drawing.Color result = ColorTranslator.FromWin32(GetPixel(dc, x, y));
            ReleaseDC(IntPtr.Zero, dc);
            return result;
        }

        private double[] hsbToRgb(double hue, double saturation, double brightness)
        {
            double red = 0, green = 0, blue = 0;//default black
            if (saturation == 0) {
                red = 0;
                green = 0;
                blue = 0;
                brightness = 0;
            }
            else {
                // the color wheel consists of 6 sectors. Figure out which sector you're in.
                double sectorPos = hue / 60.0;
                int sectorNumber = (int)(Math.Floor(sectorPos));
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the color. 
                double p = brightness * (1.0 - saturation);
                double q = brightness * (1.0 - (saturation * fractionalSector));
                double t = brightness * (1.0 - (saturation * (1 - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (sectorNumber) {
                    case 0:
                        red = brightness;
                        green = t;
                        blue = p;
                        break;
                    case 1:
                        red = q;
                        green = brightness;
                        blue = p;
                        break;
                    case 2:
                        red = p;
                        green = brightness;
                        blue = t;
                        break;
                    case 3:
                        red = p;
                        green = q;
                        blue = brightness;
                        break;
                    case 4:
                        red = t;
                        green = p;
                        blue = brightness;
                        break;
                    case 5:
                        red = brightness;
                        green = p;
                        blue = q;
                        break;
                }
            }
            double[] result = { red, green, blue };
            return result;
        }

        private double[] rgbToHsb(int red, int green, int blue)
        {
            double dRed = red / 255.0;
            double dGreen = green / 255.0;
            double dBlue = blue / 255.0;

            double max = Math.Max(dRed, Math.Max(dGreen, dBlue));
            double min = Math.Min(dRed, Math.Min(dGreen, dBlue));

            double h = 0;
            if (max == dRed && dGreen >= dBlue) {
                h = 60 * (dGreen - dBlue) / (max - min);
            }
            else if (max == dRed && dGreen < dBlue) {
                h = 60 * (dGreen - dBlue) / (max - min) + 360;
            }
            else if (max == dGreen) {
                h = 60 * (dBlue - dRed) / (max - min) + 120;
            }
            else if (max == dBlue) {
                h = 60 * (dRed - dGreen) / (max - min) + 240;
            }

            double s = (max == 0) ? 0.0 : (1.0 - (min / max));
            double[] result = { h, s, max };
            return result;
        }

        private System.Drawing.Color CalculateReprColor(Bitmap bm, bool saturate)
        {
            int width = bm.Width;
            int height = bm.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            long[] totals = new long[] { 0, 0, 0 };
            int bppModifier = bm.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4; // cutting corners, will fail on anything else but 32 and 24 bit images

            BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
            int stride = srcData.Stride;
            IntPtr Scan0 = srcData.Scan0;

            unsafe {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++) { //for each row
                    for (int x = 0; x < width; x++) { //for each col
                        int idx = (y * stride) + x * bppModifier;
                        red = p[idx + 2];
                        green = p[idx + 1];
                        blue = p[idx];
                        if (saturate) { //saturate values
                            double[] hsv = HSVRGGConverter.RGBToHSV(red, green, blue);
                            int[] rgb = HSVRGGConverter.HSVToRGB(hsv[0], 100, hsv[2]);
                            red = rgb[0];
                            green = rgb[1];
                            blue = rgb[2];
                        }
                        totals[2] += red;
                        totals[1] += green;
                        totals[0] += blue;
                    }
                }
            }
            int count = width * height;//total number of pixels in avgs
            int avgR = (int)(totals[2] / count);
            int avgG = (int)(totals[1] / count);
            int avgB = (int)(totals[0] / count);
            return System.Drawing.Color.FromArgb(avgR, avgG, avgB);
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
            return System.Drawing.Color.FromArgb(avgR, avgG, avgB);
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

        public double[] RGBtoHSB(int red, int green, int blue)
        {
            // normalize red, green and blue values
            double r = ((double)red / 255.0);
            double g = ((double)green / 255.0);
            double b = ((double)blue / 255.0);

            // conversion start
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            double h = 0.0;
            if (max == r && g >= b) {
                h = 60 * (g - b) / (max - min);
            }
            else if (max == r && g < b) {
                h = 60 * (g - b) / (max - min) + 360;
            }
            else if (max == g) {
                h = 60 * (b - r) / (max - min) + 120;
            }
            else if (max == b) {
                h = 60 * (r - g) / (max - min) + 240;
            }

            double s = (max == 0) ? 0.0 : (1.0 - (min / max));
            return new double[] {h, s, max};
        }

        public double[] HSBtoRGB(double h, double s, double br)
        {
            double r = 0;
            double g = 0;
            double b = 0;

            if (s == 0) {
                r = g = b = br;
            }
            else {
                // the color wheel consists of 6 sectors. Figure out which sector
                // you're in.
                double sectorPos = h / 60.0;
                int sectorNumber = (int)(Math.Floor(sectorPos));
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the color.
                double p = b * (1.0 - s);
                double q = b * (1.0 - (s * fractionalSector));
                double t = b * (1.0 - (s * (1 - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector
                // the angle is in.
                switch (sectorNumber) {
                    case 0:
                        r = b;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = b;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = b;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = br;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = br;
                        break;
                    case 5:
                        r = b;
                        g = p;
                        b = q;
                        break;
                }
            }

            return new double[] {
                Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", r * 255.0))),
                Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", g * 255.0))),
                Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", b * 255.0)))
            };
        }

        private Bitmap getPixelBitBlt(System.Drawing.Point topLeft, System.Drawing.Point botRight)
        {
            int width = botRight.X - topLeft.X;
            int height = botRight.Y - topLeft.Y;
            //Console.WriteLine(width + " " + height);
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

        private Bitmap getSectionOf(Bitmap screen, System.Drawing.Point topLeft, System.Drawing.Size size)
        {
            Rectangle wanted = new Rectangle(topLeft, size);
            return screen.Clone(wanted, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
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

        private System.Drawing.Point[] getPollingPoints(float width, float height, int widthDivs, int heightDivs)  //TODO make this only called when changed in settings
        {
            List<System.Drawing.Point> points = new List<System.Drawing.Point>();
            float hStep = height / heightDivs;
            float wStep = width / widthDivs;
            for (float y = hStep; y < height; y += hStep) {
                for (float x = wStep; x < width; x += wStep) {
                    //Console.WriteLine(x.ToString() + " " + y.ToString());
                    points.Add(new System.Drawing.Point((int)x, (int)y));
                }
            }
            return points.ToArray();
        }

        private Bitmap getScreenAvgAt(System.Drawing.Point point, System.Drawing.Size size)
        {
            Bitmap result;
            result = new Bitmap(size.Width, size.Height);
            Graphics gfxScreenshot = Graphics.FromImage(result);
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            gfxScreenshot.CopyFromScreen(point.X, point.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
            return result;
        }

        private System.Drawing.Color getAvgFromBitmap(Bitmap bm)
        {
            int red = 0, blue = 0, green = 0;
            int total = bm.Width * bm.Height;
            for (int r = 0; r < bm.Width; r++) {
                for (int c = 0; c < bm.Height; c++) {
                    System.Drawing.Color current = bm.GetPixel(r, c);
                    red += current.R;
                    blue += current.B;
                    green += current.G;
                }
            }
            return System.Drawing.Color.FromArgb(red / total, green / total, blue / total);
        }

        private BitmapSource GetBitmapSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap
            (
            bitmap.GetHbitmap(),
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions()
            );

            return bitmapSource;
        }

        public System.Drawing.Color CalculateDominantColor(Bitmap bitmap)
        {
            BitmapSource source = GetBitmapSource(bitmap);
            if (source.Format.BitsPerPixel != 32 || source.Format != PixelFormats.Bgra32)
                throw new ApplicationException("expected 32bit image");


            Dictionary<System.Drawing.Color, double> colorDist = new Dictionary<System.Drawing.Color, double>();

            System.Windows.Size sz = new System.Windows.Size(source.PixelWidth, source.PixelHeight);

            //read bitmap 
            int pixelsSz = (int)sz.Width * (int)sz.Height * (source.Format.BitsPerPixel / 8);
            int stride = ((int)sz.Width * source.Format.BitsPerPixel + 7) / 8;
            int pixelBytes = (source.Format.BitsPerPixel / 8);

            byte[] pixels = new byte[pixelsSz];
            source.CopyPixels(pixels, stride, 0);

            const int alphaThershold = 10;
            UInt64 pixelCount = 0;
            UInt64 avgAlpha = 0;

            for (int y = 0; y < sz.Height; y++) {
                for (int x = 0; x < sz.Width; x++) {
                    int index = (int)((y * sz.Width) + x) * (pixelBytes);
                    byte r1, g1, b1, a1; r1 = g1 = b1 = a1 = 0;
                    a1 = pixels[index + 3];
                    r1 = pixels[index + 2];
                    g1 = pixels[index + 1];
                    b1 = pixels[index];

                    if (a1 <= alphaThershold)
                        continue; //ignore

                    pixelCount++;
                    avgAlpha += (UInt64)a1;

                    System.Drawing.Color cl = System.Drawing.Color.FromArgb(0, r1, g1, b1);
                    double dist = 0;
                    if (!colorDist.ContainsKey(cl)) {
                        colorDist.Add(cl, 0);

                        for (int y2 = 0; y2 < sz.Height; y2++) {
                            for (int x2 = 0; x2 < sz.Width; x2++) {
                                int index2 = (int)(y2 * sz.Width) + x2;
                                byte r2, g2, b2, a2; r2 = g2 = b2 = a2 = 0;
                                a2 = pixels[index2 + 3];
                                r2 = pixels[index2 + 2];
                                g2 = pixels[index2 + 1];
                                b2 = pixels[index2];

                                if (a2 <= alphaThershold)
                                    continue; //ignore

                                dist += Math.Sqrt(Math.Pow(r2 - r1, 2) +
                                                  Math.Pow(g2 - g1, 2) +
                                                  Math.Pow(b2 - b1, 2));
                            }
                        }

                        colorDist[cl] = dist;
                    }
                }
            }

            //clamp alpha
            avgAlpha = avgAlpha / pixelCount;
            if (avgAlpha >= (255 - alphaThershold))
                avgAlpha = 255;

            //take weighted average of top 2% of colors         
            var clrs = (from entry in colorDist
                        orderby entry.Value ascending
                        select new { Color = entry.Key, Dist = 1.0 / Math.Max(1, entry.Value) }).ToList().Take(Math.Max(1, (int)(colorDist.Count * 0.02)));

            double sumDist = clrs.Sum(x => x.Dist);
            System.Drawing.Color result = System.Drawing.Color.FromArgb((byte)avgAlpha,
                                          (byte)(clrs.Sum(x => x.Color.R * x.Dist) / sumDist),
                                          (byte)(clrs.Sum(x => x.Color.G * x.Dist) / sumDist),
                                          (byte)(clrs.Sum(x => x.Color.B * x.Dist) / sumDist));

            return result;
        }
    }
}