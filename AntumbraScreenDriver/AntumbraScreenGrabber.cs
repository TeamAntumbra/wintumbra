using System.ComponentModel.Composition;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using System;
using Antumbra.Glow;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;

namespace AntumbraScreenDriver
{
    [Export(typeof(GlowExtension))]
    public class AntumbraScreenGrabber : GlowScreenGrabber
    {
        private int width, height;
        //private Bitmap screen;
        private Thread driver;
        public delegate void NewScreenAvail(object sender, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        private List<IObserver<Bitmap>> observers;

        //DLL declaration
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
        //BitBlt - used to get screen info

        public AntumbraScreenGrabber()
        {
            //will be set as an observed object by core
            //this.screen = null;
            this.observers = new List<IObserver<Bitmap>>();
            this.width = Screen.PrimaryScreen.Bounds.Width;
            this.height = Screen.PrimaryScreen.Bounds.Height;
        }
        public override String Name { get { return "Antumbra Screen Grabber (Default)"; } }
        public override String Description { get { return "Default means of grabbing the screen. "
                                                        + "Not DirectX compatible."; } }
        public override String Author { get { return "Team Antumbra"; } }
        public override String Version { get { return "V0.0.1"; } }

    /*    public override IDisposable Subscribe(IObserver<Bitmap> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);
            return new Unsubscriber(this.observers, observer);
        }*/

        public override bool ready()
        {
            this.driver = new Thread(new ThreadStart(captureTarget));
            return true;
        }

        public override bool start()
        {
            this.driver.Start();
            return true;
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Bitmap>> _observers;
            private IObserver<Bitmap> _observer;

            public Unsubscriber(List<IObserver<Bitmap>> observers, IObserver<Bitmap> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public override void AttachEvent(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += new NewScreenAvail(observer.NewBitmapAvail);
        }

        private void captureTarget()
        {
            while (true) {
                Bitmap screen = getPixelBitBlt(this.width, this.height);
                NewScreenAvailEvent(screen, EventArgs.Empty);
                screen.Dispose();//TODO fix out of memory errors that the bitmap probably causes
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