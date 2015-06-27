using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Bitmaps;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel.Composition;
using System.Drawing;

namespace AntumbraScreenshotProcessor
{
    [Export(typeof(GlowExtension))]
    public class AntumbraScreenshotProcessor : GlowScreenProcessor
    {
        public delegate void NewColorAvail(Color16Bit color);
        public event NewColorAvail NewColorAvailEvent;
        private bool running;
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
            get { return "Antumbra Screenshot Processor"; }
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
            get { return Guid.Parse("3eea8b48-82e3-4db4-a04a-2b9865929993"); }
        }

        public override string Description
        {
            get
            {
                return "An image processor for extracting color information from screenshots.";
            }
        }

        public override string Website
        {
            get { return "https://antumbra.io/docs/extensions/screenProcessors/GlowScreenProcessor"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override GlowExtension Create()
        {
            return new AntumbraScreenshotProcessor();
        }

        private Color16Bit Process(FastBitmap bm)
        {
            int total = 0;
            long[] colors = new long[] { 0, 0, 0 };
            int skip = bm.Width * bm.Height / 50000;
            for (int x = 0; x < bm.Width; x += skip) {
                for (int y = 0; y < bm.Height; y += skip) {
                    Color pixel = bm.GetPixel(x, y);
                    colors[0] += pixel.R;
                    colors[1] += pixel.G;
                    colors[2] += pixel.B;
                    total += 1;
                }
            }
            byte red, green, blue;
            red = (byte)((double)colors[0] / total);
            green = (byte)((double)colors[1] / total);
            blue = (byte)((double)colors[2] / total);
            Color col = Color.FromArgb(red, green, blue);
            return new Color16Bit(col);
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override void NewBitmapAvail(Bitmap bm, EventArgs args)
        {
            try {
                FastBitmap fb = new FastBitmap(bm);
                fb.Lock();
                NewColorAvailEvent(this.Process(fb));
                fb.Dispose();
            }
            catch (Exception) {
                this.Stop();
            }
            finally {
                bm.Dispose();
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
