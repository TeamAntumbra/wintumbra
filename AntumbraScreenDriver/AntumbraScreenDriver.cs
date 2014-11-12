namespace AntumbraScreenDriver
{
    using System.ComponentModel.Composition;
    using System.Drawing;
    using Antumbra.Glow.ExtensionFramework;
    using System;
    using Antumbra.Glow;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Export(typeof(GlowScreenDriver))]
    public class AntumbraScreenDriver : Antumbra.Glow.ExtensionFramework.GlowScreenDriver //TODO make observable for screen processors (which will be observed by core)
    {
        private int width, height;
        private Bitmap screen;

        //DLL declaration
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        //BitBlt - used to get screen info

        public AntumbraScreenDriver()
        {
            //will be set as an observed object by core
            this.screen = null;
            this.width = Screen.PrimaryScreen.Bounds.Width;
            this.height = Screen.PrimaryScreen.Bounds.Height;
        }
        public override String Name { get { return "Antumbra Screen Driver (Default)"; } }
        public override String Description { get { return "Test"; } }
        public override String Author { get { return "Team Antumbra"; } }
        public override String Version { get { return "V0.0.1"; } }

        public override void captureTarget()
        {
            while (true) {
                this.screen = getPixelBitBlt(this.width, this.height);
                Console.WriteLine("screen grabbed!");
                //notify for update here
            }
        }

        private Bitmap getPixelBitBlt(int width, int height)
        {
            try {
                Bitmap screen = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                using (Graphics gdest = Graphics.FromImage(screen)) {
                    using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero)) {
                        IntPtr hSrcDC = gsrc.GetHdc();
                        IntPtr hDC = gdest.GetHdc();
                        int retval = BitBlt(hDC, 0, 0, width, height, hSrcDC, 0, 0, (int)0x00CC0020);
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();
                    }
                }
                return screen;
            }
            catch (System.ArgumentException) {
                return null;
            }
        }

    }

}