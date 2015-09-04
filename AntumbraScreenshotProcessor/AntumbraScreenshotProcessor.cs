using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.ScreenInfo;
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

        private int deviceId, x, y, width, height, min;
        private long index;
        private bool running;
        private Rectangle captureRegion;
        private List<Rectangle> screenBounds;
        private readonly object sync = new object();

        public AntumbraScreenshotProcessor() {
            screenBounds = new List<Rectangle>();
            min = 0;
            foreach(var screen in System.Windows.Forms.Screen.AllScreens) {
                min = min < screen.Bounds.X ? min : screen.Bounds.X;
                screenBounds.Add(screen.Bounds);
            }

            min *= -1;

           /* for(int i = 0; i < screenBounds.Count; i += 1) {
                Rectangle rect = screenBounds[i];
                rect.X -= min;
                screenBounds[i] = rect;
            } Note: related to the TODO below regarding cross display capture zones*/

            //screenBounds.Sort((x,y) => x.X.CompareTo(y.X));
        }

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

        private Color16Bit Process(List<int[,,]> pixels) {
            int r = 0;
            int g = 0;
            int b = 0;
            int size = 0;

            foreach(KeyValuePair<int, Rectangle> screenMappedRegion in SplitRegionByScreenBounds(captureRegion)) {
                Rectangle region = screenMappedRegion.Value;
                region.X += min;
                int xPos = 0, yPos = 0;
                for(int x = region.Left; x < region.Right; x += 25) {
                    for(int y = region.Top; y < region.Bottom; y += 25) {
                        try {
                            r += pixels[screenMappedRegion.Key][xPos, yPos, 0];
                            g += pixels[screenMappedRegion.Key][xPos, yPos, 1];
                            b += pixels[screenMappedRegion.Key][xPos, yPos, 2];
                            yPos += 1;
                            size += 1;
                        } catch(IndexOutOfRangeException e) {
                            break;
                        }
                    }
                    xPos += 1;
                    yPos = 0;
                }
            }

            byte red, green, blue;
            red = (byte)((double)r / size);
            green = (byte)((double)g / size);
            blue = (byte)((double)b / size);
            return Color16BitUtil.FunnelIntoColor(red << 8, green << 8, blue << 8);
        }

        private Dictionary<int, Rectangle> SplitRegionByScreenBounds(Rectangle region) {
            Dictionary<int, Rectangle> result = new Dictionary<int, Rectangle>();
            for(int i = 0; i < screenBounds.Count; i += 1) {
                Rectangle screen = screenBounds[i];
                if(screen.Contains(region.Location)) {
                    //Point farX = new Point(region.Location.X + region.Width, 0);
                    /*if(screen.Contains(farX)) {
                        result.Add(i, region);
                    } else {//partially on next screen TODO: finish this to allow capture zone to cross between two displays
                        try {
                            if(screenBounds[i + 1].Contains(farX)) {
                                Rectangle back = new Rectangle(new Point(0, region.Location.Y),
                                                               new Size(region.Location.X + region.Width - screenBounds[i].Width, region.Height));
                                result.Add(i + 1, back);
                            }
                        } catch(IndexOutOfRangeException) {
                            // No next screen, window is partially off screen
                        } finally {*/
                    int newWidth = screenBounds[i].Width - region.X;
                    if(newWidth < region.Width / 2) {
                        region.X = screenBounds[i].Width;
                        Rectangle back = new Rectangle(region.Location, new Size(region.Width, region.Height));
                        result.Add(i + 1, back);
                    } else {
                        Rectangle front = new Rectangle(region.Location, new Size(newWidth, region.Height));
                        result.Add(i, front);
                    }
                     //   }
                    //}
                }
            }
            return result;
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override void NewScreenInfoAvail(List<int[,,]> pixelArray, EventArgs args)
        {
            try {
                lock(sync) {
                    NewColorAvailEvent(Process(pixelArray), devId, index++);
                }
            }
            catch (Exception ex) {
                //TODO log ex
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
