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
        int widthDivs, heightDivs;//width and height divisions for screen polling
        int x, y;//x and y bounds of screen
        Point[] points;//polling points
        Size size;//polling size
        //GPGPU gpu;

        public ScreenGrabber()
        {
            this.size = new Size(50, 30);
            updateBounds();
            this.widthDivs = 4;
            this.heightDivs = 4;
            this.points = getPollingPoints(this.width, this.height, this.widthDivs, this.heightDivs);
            //setupCudafy();
        }

        public void updateBounds()
        {
            this.width = Screen.PrimaryScreen.Bounds.Width;
            this.height = Screen.PrimaryScreen.Bounds.Height;
            this.x = Screen.PrimaryScreen.Bounds.X;
            this.y = Screen.PrimaryScreen.Bounds.Y;
            this.points = getPollingPoints(this.width, this.height, this.widthDivs, this.heightDivs);
        }

        public Color getScreenAvgColor()
        {
            Bitmap screen = getPixelBitBlt(this.width, this.height);
            return CalculateAverageColor(screen);
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
            Color result = CalculateAverageColor(screen);
            s.Stop();
            Console.WriteLine(result.R + " " + result.G + " " + result.B);
            Console.WriteLine("process time: " + s.ElapsedMilliseconds);
        }

        public Color getAvgFromPoints(Bitmap screen) 
        {
            int r = 0, g = 0, b = 0;
            foreach (Point pnt in this.points) {
                Bitmap section = getSectionOf(screen, pnt, this.size);
                Color each = getAvgFromBitmap(section);
                r += each.R;
                g += each.G;
                b += each.B;
            }
            int div = this.points.Length;
            return Color.FromArgb(r / div, g / div, b / div);
        }

        private Bitmap getScreenAsBitmap()
        {
            Bitmap result = new Bitmap(this.width, this.height, PixelFormat.Format32bppRgb);
            using (Graphics gfxScreenshot = Graphics.FromImage(result))
                gfxScreenshot.CopyFromScreen(this.x, this.y, 0, 0, new Size(this.width, this.height));//, CopyPixelOperation.SourceCopy);
            return result;
        }

        private Color getPixel(int x, int y)//get just one pixel from system
        {
            IntPtr dc = GetDC(IntPtr.Zero);
            Color result = ColorTranslator.FromWin32(GetPixel(dc, x, y));
            ReleaseDC(IntPtr.Zero, dc);
            return result;
        }

        private Color CalculateAverageColor(Bitmap bm)
        {
            int width = bm.Width;
            int height = bm.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            int minDiversion = 15; // drop pixels that do not differ by at least minDiversion between color values (white, gray or black)
            int dropped = 0; // keep track of dropped pixels
            long[] totals = new long[] { 0, 0, 0 };
            int bppModifier = bm.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4; // cutting corners, will fail on anything else but 32 and 24 bit images

            BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
            int stride = srcData.Stride;
            IntPtr Scan0 = srcData.Scan0;

            unsafe {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        int idx = (y * stride) + x * bppModifier;
                        red = p[idx + 2];
                        green = p[idx + 1];
                        blue = p[idx];
                        if (Math.Abs(red - green) > minDiversion || Math.Abs(red - blue) > minDiversion || Math.Abs(green - blue) > minDiversion) {
                            totals[2] += red;
                            totals[1] += green;
                            totals[0] += blue;
                        }
                        else {
                            dropped++;
                        }
                    }
                }
            }

            int count = width * height - dropped;
            int avgR = (int)(totals[2] / count);
            int avgG = (int)(totals[1] / count);
            int avgB = (int)(totals[0] / count);

            return System.Drawing.Color.FromArgb(avgR, avgG, avgB);
        }

        private Bitmap getPixelBitBlt(int width, int height)
        {
            Bitmap screenPixel = new Bitmap(width, height, PixelFormat.Format32bppRgb);
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

        private Bitmap getSectionOf(Bitmap screen, Point topLeft, Size size)
        {
            Rectangle wanted = new Rectangle(topLeft, size);
            return screen.Clone(wanted, PixelFormat.Format32bppRgb);
        }

        private Point[] getPollingRectPoints(float width, float height)
        {
            Point topLeft = new Point((int)(width / 8), (int)(height / 8));
            Point topRight = new Point((int)(width * (7 / 8)), (int)(height / 8));
            Point botLeft = new Point((int)(width / 8), (int)(height * (7 / 8)));
            Point botRight = new Point((int)(width * (7/8)), (int)(height * (7/8)));
            Point[] result = { topLeft, topRight, botLeft, botRight };
            return result;
        }

        private Point[] getPollingPoints(float width, float height, int widthDivs, int heightDivs)  //TODO make this only called when changed in settings
        {
            List<Point> points = new List<Point>();
            float hStep = height / heightDivs;
            float wStep = width / widthDivs;
            for (float y = hStep; y < height; y += hStep) {
                for (float x = wStep; x < width; x += wStep) {
                    //Console.WriteLine(x.ToString() + " " + y.ToString());
                    points.Add(new Point((int)x, (int)y));
                }
            }
            return points.ToArray();
        }

        private Bitmap getScreenAvgAt(Point point, Size size)
        {
            Bitmap result;
            result = new Bitmap(size.Width, size.Height);
            Graphics gfxScreenshot = Graphics.FromImage(result);
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            gfxScreenshot.CopyFromScreen(point.X, point.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
            return result;
        }

        private Color getAvgFromBitmap(Bitmap bm)
        {
            int red = 0, blue = 0, green = 0;
            int total = bm.Width * bm.Height;
            for (int r = 0; r < bm.Width; r++) {
                for (int c = 0; c < bm.Height; c++) {
                    Color current = bm.GetPixel(r, c);
                    red += current.R;
                    blue += current.B;
                    green += current.G;
                }
            }
            return Color.FromArgb(red / total, green / total, blue / total);
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
    }
}
