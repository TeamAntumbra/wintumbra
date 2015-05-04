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
            get { return "https://antumbra.io/docs/extensions/screenProcessors/fastScreenProcessor"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        private Color16Bit Process(FastBitmap bm)
        {
            long[] colors = new long[] { 0, 0, 0 };
            for (int x = 0; x < bm.Width; x += 1) {
                for (int y = 0; y < bm.Height; y += 1) {
                    Color pixel = bm.GetPixel(x, y);
                    colors[0] += pixel.R;
                    colors[1] += pixel.G;
                    colors[2] += pixel.B;
                }
            }
            byte red, green, blue;
            red = (byte)(colors[0] / colors.Length);
            green = (byte)(colors[1] / colors.Length);
            blue = (byte)(colors[2] / colors.Length);
            return new Color16Bit(red, green, blue);
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override void NewBitmapAvail(FastBitmap bm, EventArgs args)
        {
            try {
                NewColorAvailEvent(this.Process(bm));
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
