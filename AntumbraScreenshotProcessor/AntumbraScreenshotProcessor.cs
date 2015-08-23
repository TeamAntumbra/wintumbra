﻿using System;
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

        private Color16Bit Process(int[,] pixels) {
            int r = 0;
            int g = 0;
            int b = 0;

            for(int row = captureRegion.Left; row < captureRegion.Right; row += 1) {
                for(int col = captureRegion.Top; col < captureRegion.Bottom; col += 1) {
                    Color pixel = Color.FromArgb(pixels[row, col]);
                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;
                }
            }

            int size = captureRegion.Width * captureRegion.Height;

            byte red, green, blue;
            red = (byte)((double)r / size);
            green = (byte)((double)g / size);
            blue = (byte)((double)b / size);
            Color color = Color.FromArgb(red, green, blue);
            return new Color16Bit(color);
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override void NewBitmapAvail(int[,] pixels, EventArgs args)
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
