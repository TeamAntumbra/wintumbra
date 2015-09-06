using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Reflection;

namespace AntumbraScreenshotProcessor {

    [Export(typeof(GlowExtension))]
    public class AntumbraScreenshotProcessor : GlowScreenProcessor {

        #region Private Fields

        private readonly object sync = new object();

        private int deviceId, x, y, width, height, min;

        private long index;

        private Dictionary<int, Rectangle> regions;

        private bool running;

        private List<Rectangle> screenBounds;

        #endregion Private Fields

        #region Public Constructors

        public AntumbraScreenshotProcessor() {
            regions = new Dictionary<int, Rectangle>();
            screenBounds = new List<Rectangle>();
            min = 0;
            foreach(var screen in System.Windows.Forms.Screen.AllScreens) {
                min = min < screen.Bounds.X ? min : screen.Bounds.X;
                screenBounds.Add(screen.Bounds);
            }

            min *= -1;
            /*
                        for(int i = 0; i < screenBounds.Count; i += 1) {
                            Rectangle rect = screenBounds[i];
                            rect.X += min;
                            screenBounds[i] = rect;
                        }*/

            screenBounds.Sort((x, y) => x.X.CompareTo(y.X));
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewColorAvail(Color16Bit color, int id, long index);

        #endregion Public Delegates

        #region Public Events

        public event NewColorAvail NewColorAvailEvent;

        #endregion Public Events

        #region Public Properties

        public override string Author {
            get { return "Team Antumbra"; }
        }

        public override string Description {
            get {
                return "An image processor for extracting color information from screenshots.";
            }
        }

        public override int devId {
            get {
                return deviceId;
            }
            set {
                deviceId = value;
            }
        }

        public override Guid id {
            get { return Guid.Parse("3eea8b48-82e3-4db4-a04a-2b9865929993"); }
        }

        public override bool IsDefault {
            get { return false; }
        }

        public override bool IsRunning {
            get { return running; }
        }

        public override string Name {
            get { return "Antumbra Screenshot Processor"; }
        }

        public override Version Version {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website {
            get { return "https://wintumbra.rtfd.org"; }
        }

        #endregion Public Properties

        #region Public Methods

        public override void AttachObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override GlowScreenProcessor Create() {
            return new AntumbraScreenshotProcessor();
        }

        public override void Dispose() {
        }

        public override void NewScreenInfoAvail(List<int[, ,]> pixelArray, EventArgs args) {
            try {
                lock(sync) {
                    NewColorAvailEvent(Process(pixelArray, regions), devId, index++);
                }
            } catch(Exception ex) {
                //TODO log ex
            }
        }

        public override void SetArea(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            var captureRegion = new Rectangle(x, y, width, height);
            regions = SplitRegionByScreenBounds(captureRegion);
        }

        public override bool Settings() {
            return false;
        }

        public override bool Start() {
            index = long.MinValue;
            running = true;
            return true;
        }

        public override bool Stop() {
            running = false;
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private Color16Bit Process(List<int[, ,]> pixels, Dictionary<int, Rectangle> regions) {
            int r = 0;
            int g = 0;
            int b = 0;
            int size = 0;

            foreach(KeyValuePair<int, Rectangle> screenMappedRegion in regions) {
                Rectangle region = screenMappedRegion.Value;
                if(min != 0) {
                    int posX = Math.Abs(region.X);
                    if(posX > min) {
                        region.X = posX % min;
                    }

                    if(region.X < 0) {
                        region.X += min;
                    }
                }

                for(int x = region.Left / 25; x < region.Right / 25; x += 1) {
                    for(int y = region.Top / 25; y < region.Bottom / 25; y += 1) {
                        try {
                            r += pixels[screenMappedRegion.Key][x, y, 0];
                            g += pixels[screenMappedRegion.Key][x, y, 1];
                            b += pixels[screenMappedRegion.Key][x, y, 2];
                            size += 1;
                        } catch(Exception e) {
                            break;
                        }
                    }
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
                if(screen.Contains(region)) { //All on this screen
                    result.Add(i, region);
                } else {
                    result.Add(i, Rectangle.Intersect(screen, region));
                }
            }
            return result;
        }

        #endregion Private Methods
    }
}
