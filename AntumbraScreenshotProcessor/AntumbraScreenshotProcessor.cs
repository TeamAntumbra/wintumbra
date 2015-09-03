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
        public delegate void NewColorAvail(Color16Bit color, int id, long index);
        public event NewColorAvail NewColorAvailEvent;

        private int deviceId, x, y, width, height;
        private long index;
        private bool running;
        private Rectangle captureRegion;

        public override bool IsDefault
        {
            get { return false; }
        }

        public override int devId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
            }
        }

        public override bool IsRunning
        {
            get { return running; }
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
            get { return "https://wintumbra.rtfd.org"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override GlowScreenProcessor Create()
        {
            return new AntumbraScreenshotProcessor();
        }

        private Color16Bit Process(int[,,] pixels) {
            int r = 0;
            int g = 0;
            int b = 0;
            int size = 0;

            try {
                for(int x = captureRegion.Left / 25; x < captureRegion.Right / 25; x += 1) {
                    for(int y = captureRegion.Top / 25; y < captureRegion.Bottom / 25; y += 1) {
                        r += pixels[x, y, 0];
                        g += pixels[x, y, 1];
                        b += pixels[x, y, 2];
                        size += 1;
                    }
                }
            } catch(IndexOutOfRangeException e) {
                Console.WriteLine(e.Message + " " + e.Source);
            }

            byte red, green, blue;
            red = (byte)((double)r / size);
            green = (byte)((double)g / size);
            blue = (byte)((double)b / size);
            return Color16BitUtil.FunnelIntoColor(red << 8, green << 8, blue << 8);
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override void NewBitmapAvail(int[,,] pixels, EventArgs args)
        {
            try {
                NewColorAvailEvent(Process(pixels), devId, index++);
            }
            catch (Exception ex) {
                //TODO log ex
                Stop();
            }
        }

        public override bool Start()
        {
            index = long.MinValue;
            running = true;
            return true;
        }

        public override bool Stop()
        {
            running = false;
            return true;
        }

        public override void Dispose()
        {
            
        }

        public override void SetArea(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            captureRegion = new Rectangle(x, y, width, height);
        }
    }
}
