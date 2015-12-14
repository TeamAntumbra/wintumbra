using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Reflection;

namespace AntumbraScreenshotProcessor {

    [Export(typeof(GlowExtension))]
    public class AntumbraScreenshotProcessor : GlowScreenProcessor, Loggable {

        #region Private Fields

        private readonly object sync = new object();
        private Dictionary<int, Rectangle> areas;
        private long index;
        private int min, x, y, width, height, deviceId;
        private Dictionary<int, Dictionary<int, Rectangle>> regions;
        private bool running;
        private List<Rectangle> screenBounds;

        #endregion Private Fields

        #region Public Constructors

        public AntumbraScreenshotProcessor() {
            regions = new Dictionary<int, Dictionary<int, Rectangle>>();
            screenBounds = new List<Rectangle>();
            areas = new Dictionary<int, Rectangle>();
            min = 0;
            foreach(var screen in System.Windows.Forms.Screen.AllScreens) {
                min = min < screen.Bounds.X ? min : screen.Bounds.X;
                screenBounds.Add(screen.Bounds);
            }

            min *= -1;

            if(min != 0) {
                for(var i = 0; i < screenBounds.Count; i += 1) {
                    var screen = screenBounds[i];
                    screen.X += min;
                }
            }

            screenBounds.Sort((x, y) => x.X.CompareTo(y.X));
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewColorAvail(Color16Bit color, int id, long index);

        public delegate void NewLogMsg(string title, string msg);

        #endregion Public Delegates

        #region Public Events

        public event NewColorAvail NewColorAvailEvent;

        public event NewLogMsg NewLogMsgEvent;

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

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgEvent += observer.NewLogMsgAvail;
        }

        public override void AttachObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        public override GlowScreenProcessor Create() {
            return new AntumbraScreenshotProcessor();
        }

        public override void Dispose() {
        }

        public override Dictionary<int, Rectangle> GetMappings() {
            return this.areas;
        }

        public override void NewScreenInfoAvail(List<int[, ,]> pixelArray, EventArgs args) {
            try {
                var pairs = Process(pixelArray, regions);
                foreach(KeyValuePair<int, Color16Bit> res in pairs) {
                    lock(sync) {
                        NewColorAvailEvent(res.Value, res.Key, index++);
                    }
                }
            } catch(Exception ex) {
                Console.WriteLine(ex.StackTrace);
                //TODO log ex
            }
        }

        public override void SetArea(int x, int y, int width, int height, int id) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            var captureRegion = new Rectangle(x, y, width, height);
            if(areas.ContainsKey(id)) {
                areas.Remove(id);
            }
            areas.Add(id, captureRegion);
            regions = SplitRegionByScreenBounds(areas);
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

        private Dictionary<int, Color16Bit> Process(List<int[, ,]> pixels, Dictionary<int, Dictionary<int, Rectangle>> regions) {
            int size = 0;
            int r = 0;
            int g = 0;
            int b = 0;
            Dictionary<int, Color16Bit> result = new Dictionary<int, Color16Bit>();

            foreach(KeyValuePair<int, Dictionary<int, Rectangle>> devSet in regions) {
                r = 0;
                g = 0;
                b = 0;
                foreach(int i in devSet.Value.Keys) {
                    Rectangle region = devSet.Value[i];
                    region.X = 0;

                    int xMax = region.Right / 25;
                    for(int x = region.Left / 25; x < xMax; x += 1) {
                        for(int y = region.Top / 25; y < region.Bottom / 25; y += 1) {
                            if(x == xMax) {
                                break;
                            }
                            try {
                                r += pixels[i][x, y, 0];
                                g += pixels[i][x, y, 1];
                                b += pixels[i][x, y, 2];
                                size += 1;
                            } catch(Exception e) {
                                if(NewLogMsgEvent != null)
                                    NewLogMsgEvent("AntumbraScreenshotProcessor", e.Message + '\n' + e.StackTrace);
                                break;
                            }
                        }
                    }
                }

                byte red, green, blue;
                red = (byte)((double)r / size);
                green = (byte)((double)g / size);
                blue = (byte)((double)b / size);
                result.Add(devSet.Key, Color16BitUtil.FunnelIntoColor(red << 8, green << 8, blue << 8));
            }
            return result;
        }

        private Dictionary<int, Dictionary<int, Rectangle>> SplitRegionByScreenBounds(Dictionary<int, Rectangle> regions) {
            Dictionary<int, Dictionary<int, Rectangle>> result = new Dictionary<int, Dictionary<int, Rectangle>>();
            Dictionary<int, Rectangle> inner = new Dictionary<int, Rectangle>();
            foreach(KeyValuePair<int, Rectangle> pair in regions) {
                var region = pair.Value;
                for(int i = 0; i < screenBounds.Count; i += 1) {
                    Rectangle screen = screenBounds[i];
                    if(!result.ContainsKey(pair.Key)) {
                        result[pair.Key] = new Dictionary<int, Rectangle>();
                    }
                    Rectangle final;
                    if(screen.Contains(region)) { //All on this screen
                        final = region;
                    } else {
                        final = Rectangle.Intersect(screen, region);
                    }
                    if(final.X < 0) {
                        final.Width = final.X * -1;
                        final.X = screen.Width - final.Width;
                    }
                    result[pair.Key].Add(i, final);
                }
            }
            return result;
        }

        #endregion Private Methods
    }
}
