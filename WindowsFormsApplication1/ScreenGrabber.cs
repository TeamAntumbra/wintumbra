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
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
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

        int width, height;//screen width and height
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
            Bitmap screen = getPixelBitBlt(width, height);
            return CalculateReprColor(screen);
        }

        public System.Drawing.Color getCenterScreenAvgColor()
        {
            //Console.WriteLine(this.points[0].X + " " + this.points[0].Y + " " + this.points[3].X + " " + this.points[3].Y);
            Bitmap screen = getPixelBitBlt(this.points[0], this.points[3]);
            return CalculateReprColor(screen);
        }

        public System.Drawing.Color getScreenDomColor()
        {
            Bitmap screen = getPixelBitBlt(this.points[0], this.points[3]);
            return CalculateDominantColor(screen);
        }

        public System.Drawing.Color getCenterScreenDomColor()
        {
            Bitmap screen = getPixelBitBlt(this.width, this.height);
            return CalculateDominantColor(screen);
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
            System.Drawing.Color result = CalculateReprColor(screen);
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

        private System.Drawing.Color CalculateReprColor(Bitmap bm)
        {
            int width = bm.Width;
            int height = bm.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            //int minDiversion = 15; // drop pixels that do not differ by at least minDiversion between color values (white, gray or black)
            int dropped = 0; // keep track of dropped pixels
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
                        //saturate values
                        /*double[] hsb = rgbToHsb(red, green, blue);
                        //Console.WriteLine(hsb[0] + " " + hsb[1] + " " + hsb[2]);
                        hsb[1] = 1.0;//full saturation
                        double[] rgb = hsbToRgb(hsb[0], hsb[1], hsb[2]);
                        red = 255;// rgb[0];
                        green = 255;// rgb[1];
                        blue = 255;// rgb[2];
                        Console.WriteLine(rgb[0] + " " + rgb[1] + " " + rgb[2]);*/
                        /*int rgDiff = Math.Abs(red - green);
                        int rbDiff = Math.Abs(red - blue);
                        int gbDiff = Math.Abs(green - blue);
                        if (rgDiff > minDiversion || rbDiff > minDiversion || gbDiff > minDiversion) { //not a dull color
                            totals[2] += red;
                            totals[1] += green;
                            totals[0] += blue;
                        }
                        else {
                            dropped++;
                        }*/
                        totals[2] += red;
                        totals[1] += green;
                        totals[0] += blue;
                    }
                }
            }
            int count = width * height - dropped;//total number of pixels in avgs
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

        private Bitmap getPixelBitBlt(System.Drawing.Point topLeft, System.Drawing.Point botRight)
        {
            int width = botRight.X - topLeft.X;
            int height = botRight.Y - topLeft.Y;
            //Console.WriteLine(width + " " + height);
            Bitmap screenPixel = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
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
            return screen.Clone(wanted, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
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

       /* private void setupCudafy()
        {
            CudafyModule mod = CudafyTranslator.Cudafy();
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(mod);
            this.gpu = gpu;
        }

        private Bitmap getScreenCuda()
        {
            this.gpu.Launch().cudaStuff();
            return new Bitmap(1, 1);
        }

        [Cudafy]
        private void cudaStuff()
        {
            //cuda stuff
        }*/

       /* public Color avgWithGL(Bitmap bitmap)
        {
            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(ShaderType.FragmentShader, @"");
        }*/
    }
}
