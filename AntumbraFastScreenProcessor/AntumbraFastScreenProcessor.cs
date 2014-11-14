using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;

namespace AntumbraFastScreenProcessor
{
    [Export(typeof(GlowExtension))]
    public class AntumbraFastScreenProcessor : GlowScreenProcessor, AntumbraBitmapObserver
        //No fancy algorithms here. Just pure speed through a straight average
    {
        public delegate void NewColorAvail(object sender, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        public AntumbraFastScreenProcessor()
        {

        }

        public override string Name
        {
            get { return "Antumbra Fast Screen Processor"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A very straight forward screen processor. "
                       + "Simply averages the bitmap in the fastest way "
                       + "possible."; }
        }

        public override string Version
        {
            get { return "V0.0.1"; }
        }

        public override bool ready()
        {
            return true;
        }

        private Color Process(Bitmap bm)
        {
            BitmapData srcData = bm.LockBits(
            new Rectangle(0, 0, bm.Width, bm.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            long[] totals = new long[] { 0, 0, 0 };

            int width = bm.Width;
            int height = bm.Height;

            unsafe {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        for (int color = 0; color < 3; color++) {
                            int idx = (y * stride) + x * 4 + color;

                            totals[color] += p[idx];
                        }
                    }
                }
            }
            int avgB = (int)(totals[0] / (width * height));
            int avgG = (int)(totals[1] / (width * height));
            int avgR = (int)(totals[2] / (width * height));
            return Color.FromArgb(avgR, avgG, avgB);
        }

        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        void AntumbraBitmapObserver.NewBitmapAvail(object sender, EventArgs args)
        {
            if (sender is Bitmap) {
                Bitmap bm = (Bitmap)sender;
                NewColorAvailEvent(Process((Bitmap)sender), EventArgs.Empty);
                bm.Dispose();
            }
        }
    }
}
