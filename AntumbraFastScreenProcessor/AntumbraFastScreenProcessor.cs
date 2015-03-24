using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;
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

        private Color16Bit Process(Bitmap bm)
        {
            Bitmap small = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(small))//resize to 1X1 Bitmap using GDI+ Graphics class
                g.DrawImage(bm, 0, 0, 1, 1);
            return new Color16Bit(small.GetPixel(0, 0));
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override void NewBitmapAvail(Bitmap bm, EventArgs args)
        {
            try {
                NewColorAvailEvent(Process(bm));
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
