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
    public class AntumbraScreenGrabber : GlowScreenGrabber //TODO make observable for screen processors (which will be observed by core)
    {
        private int width, height;
        //private Bitmap screen;
        private Thread driver;
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

        public override IDisposable Subscribe(IObserver<Bitmap> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);
            return new Unsubscriber(this.observers, observer);
        }

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

        private void captureTarget()
        {
            while (true) {
                using (Bitmap screen = getPixelBitBlt(this.width, this.height)) {
                    //notify for update here
                    foreach (var observer in this.observers) {
                        observer.OnNext(screen);
                    }
                }
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