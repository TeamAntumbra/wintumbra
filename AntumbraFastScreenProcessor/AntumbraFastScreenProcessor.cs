using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Bitmaps;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace AntumbraFastScreenProcessor
{
    [Export(typeof(GlowExtension))]
    public class AntumbraFastScreenProcessor : GlowScreenProcessor
    //No fancy algorithms here. Just pure speed through a straight average
    {
        public delegate void NewColorAvail(Color16Bit newColor);
        public event NewColorAvail NewColorAvailEvent;
        private bool running = false;
        public override bool IsDefault
        {
            get { return false; }
        }
        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override string Name
        {
            get { return "Antumbra Fast Screen Processor"; }
        }

        public override bool Settings()
        {
            return false;
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override Guid id
        {
            get { return Guid.Parse("07eda8bc-28e6-4d57-a085-7f204785630f"); }
        }

        public override string Description
        {
            get
            {
                return "A very straight forward screen processor. "
                     + "Simply averages the bitmap in the fastest way "
                     + "possible.";
            }
        }

        public override string Website
        {
            get { return "https://antumbra.io/docs/extensions/screenProcessors/fastScreenProcessor"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        private Color16Bit Process(FastBitmap bm)
        {
            //FastBitmap fastBm = new FastBitmap(bm);
            //bm.Lock();
            //FastBitmap newBm = 
            //bm = Antumbra.Glow.Utility.BandingRemover.RemoveBanding(bm);
            Bitmap newBitmap = Antumbra.Glow.Utility.BandingRemover.ReplaceBandingWithTransparent(bm.GetBitmap());
            //fastBm.Unlock();
            //bm.Unlock();
            Bitmap small = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(small)) {//resize to 1X1 Bitmap using GDI+ Graphics class
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bm.GetBitmap(), 0, 0, 1, 1);
                g.Save();
            }
            bm.Dispose();
            return new Color16Bit(small.GetPixel(0, 0));
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override void NewBitmapAvail(FastBitmap bm, EventArgs args)
        {
            try {
                lock (bm.sync) {
                    NewColorAvailEvent(this.Process(bm));
                }
            }
            catch (Exception) {
                this.Stop();
            }
            finally {
                bm.Dispose();
                bm = null;
            }
        }

        public override bool Start()
        {
            this.running = true;
            return true;
        }

        public override bool Stop()
        {
            this.running = false;
            return true;
        }
    }
}
