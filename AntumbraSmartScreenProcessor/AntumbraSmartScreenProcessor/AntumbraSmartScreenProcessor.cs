using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Utility;
using System.Windows.Forms;

namespace AntumbraScreenProcessor
{
    [Export(typeof(GlowExtension))]
    public class AntumbraSmartScreenProcessor : GlowScreenProcessor
    {
        public delegate void NewColorAvail(Color newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private bool running = false;
        private SmartProcSettingsWindow settings;
        private Dictionary<string, int> instanceSettings;
        public override int id { get; set; }
        public override bool IsDefault
        {
            get { return true; }
        }

        public override String Name
        {
            get { return "Antumbra Screen Processor (Default)"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get
            {
                return "The Antumbra Screen Processor. Has various modes for getting "
                    + "the dominant color of a screen capture bitmap.";
            }
        }

        public override string Website
        {
            get { return "https://antumbra.io/TBD"; }
        }

        public override Version Version
        {
            get { return new Version("0.0.1"); }
        }

        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool Setup()
        {
            this.instanceSettings = new Dictionary<string,int>();
            this.instanceSettings["useAllTol"] = 20;
            this.instanceSettings["minMixPerc"] = 15;
            this.instanceSettings["minBright"] = 10;
            return true;
        }

        public override bool Start()
        {
            if (this.instanceSettings == null)//not ready
                if (!this.Setup())
                    return false;//failed to setup
            this.running = true;
            return true;
        }

        public override bool Settings()
        {
            this.settings = new SmartProcSettingsWindow(this);
            this.settings.Show();
            if (this.instanceSettings == null)//not ready
                if (!this.Setup())
                    return false;//failed to setup
            this.settings.useAllTxt.Text = this.instanceSettings["useAllTol"].ToString();
            this.settings.minMixTxt.Text = this.instanceSettings["minMixPerc"].ToString();
            this.settings.minBrightTxt.Text = this.instanceSettings["minBright"].ToString();
            this.settings.useAllTxt.TextChanged += new EventHandler(useAllChanged);
            this.settings.minBrightTxt.TextChanged += new EventHandler(minMixChanged);
            this.settings.minMixTxt.TextChanged += new EventHandler(minBrightChanged);
            return true;
        }

        private void useAllChanged(object sender, EventArgs args)
        {
            int i;
            TextBox box = (TextBox)sender;
            if (int.TryParse(box.Text, out i))
                this.instanceSettings["useAllTol"] = i;
        }

        private void minMixChanged(object sender, EventArgs args)
        {
            int i;
            TextBox box = (TextBox)sender;
            if (int.TryParse(box.Text, out i))
                this.instanceSettings["minMixPerc"] = i;
        }

        private void minBrightChanged(object sender, EventArgs args)
        {
            int i;
            TextBox box = (TextBox)sender;
            if (int.TryParse(box.Text, out i))
                this.instanceSettings["minBright"] = i;
        }

        public override bool Stop()
        {
            if (this.settings != null)
                this.settings.Dispose();
            this.NewColorAvailEvent = null;
            this.running = false;
            return true;
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override void NewBitmapAvail(Bitmap bm, EventArgs args)
        {
            NewColorAvailEvent(Process(bm), EventArgs.Empty);
            bm.Dispose();
        }

        public Color Process(Bitmap screen)
        {
            if (screen == null) {
                //Console.WriteLine("null BitMap returned");
                return Color.Empty;
            }
            return SmartCalculateReprColor(screen, this.instanceSettings["useAllTol"], (int)this.instanceSettings["minMixPerc"],
                this.instanceSettings["minBright"]);
        }

        private Color SmartCalculateReprColor(Bitmap bm, int useAllTolerance, int mixPercThreshold, int minBrightness)
        {
            int width = bm.Width;
            int height = bm.Height;
            int red = 0;
            int green = 0;
            int blue = 0;
            long[] blues = new long[] { 0, 0, 0 };
            long[] greens = new long[] { 0, 0, 0 };
            long[] reds = new long[] { 0, 0, 0 };
            long[] all = new long[] { 0, 0, 0 };
            int bppModifier = bm.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? 3 : 4; // cutting corners, will fail on anything else but 32 and 24 bit images

            BitmapData srcData = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, bm.PixelFormat);
            int stride = srcData.Stride;
            IntPtr Scan0 = srcData.Scan0;
            int bluesCount = 0, greensCount = 0, redsCount = 0;

            unsafe {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++) { //for each row
                    for (int x = 0; x < width; x++) { //for each col
                        int idx = (y * stride) + x * bppModifier;
                        red = p[idx + 2];
                        green = p[idx + 1];
                        blue = p[idx];
                        if (red < minBrightness && green < minBrightness && blue < minBrightness)//skip pixel, too dark
                            continue;
                        int max = Math.Max(blue, Math.Max(green, red));
                        if (blue == max) {//blue dominant
                            blues[2] += red;
                            blues[1] += green;
                            blues[0] += blue;
                            bluesCount += 1;
                        }
                        else if (green == max) {//green dominant
                            greens[2] += red;
                            greens[1] += green;
                            greens[0] += blue;
                            greensCount += 1;
                        }
                        else if (red == max) {//red dominant
                            reds[2] += red;
                            reds[1] += green;
                            reds[0] += blue;
                            redsCount += 1;
                        }
                        else {
                            Console.WriteLine("this should not happen! (in getReprColor)");
                        }
                        all[2] += red;
                        all[1] += green;
                        all[0] += blue;
                    }
                }
            }
            long[] totals = new long[] { 0, 0, 0 };
            int count = Math.Max(bluesCount, Math.Max(greensCount, redsCount));
            if (Math.Abs(bluesCount - greensCount) < useAllTolerance && Math.Abs(bluesCount - redsCount) < useAllTolerance && Math.Abs(greensCount - redsCount) < useAllTolerance)
                totals = all;
            else if (bluesCount >= greensCount && bluesCount >= redsCount) {
                totals = blues;
                double mixThreshold = bluesCount * (mixPercThreshold / 100.0);
                if (redsCount > mixThreshold) { //mix in red
                    totals[2] += reds[2];
                    totals[1] += reds[1];
                    totals[0] += reds[0];
                    count += redsCount;
                }
                if (greensCount > mixThreshold) { //mix in green
                    totals[2] += greens[2];
                    totals[1] += greens[1];
                    totals[0] += greens[0];
                    count += greensCount;
                }
            }
            else if (greensCount >= bluesCount && greensCount >= redsCount) {
                totals = greens;
                double mixThreshold = greensCount * (mixPercThreshold / 100.0);
                if (redsCount > mixThreshold) { //mix in red
                    totals[2] += reds[2];
                    totals[1] += reds[1];
                    totals[0] += reds[0];
                    count += redsCount;
                }
                if (bluesCount > mixThreshold) { //mix in blue
                    totals[2] += blues[2];
                    totals[1] += blues[1];
                    totals[0] += blues[0];
                    count += bluesCount;
                }
            }
            else if (redsCount >= bluesCount && redsCount >= greensCount) {
                totals = reds;
                double mixThreshold = redsCount * (mixPercThreshold / 100.0);
                if (bluesCount > mixThreshold) { //mix in blue
                    totals[2] += blues[2];
                    totals[1] += blues[1];
                    totals[0] += blues[0];
                    count += bluesCount;
                }
                if (greensCount > mixThreshold) { //mix in green
                    totals[2] += greens[2];
                    totals[1] += greens[1];
                    totals[0] += greens[0];
                    count += greensCount;
                }
            }
            else
                Console.WriteLine("this should not happen! (in getReprColor) #2");
            if (count == 0)//avoid dividing by zero
                return Color.Empty;
            int avgR = (int)(totals[2] / count);
            int avgG = (int)(totals[1] / count);
            int avgB = (int)(totals[0] / count);
            return System.Drawing.Color.FromArgb(avgR, avgG, avgB);
        }
    }
}
