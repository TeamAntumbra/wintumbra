using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using System.Windows.Forms;
using System.Reflection;
using Antumbra.Glow.Observer.Logging;

namespace AntumbraSmartScreenProcessor
{
    [Export(typeof(GlowExtension))]
    public class AntumbraSmartScreenProcessor : GlowScreenProcessor, Loggable
    {
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;
        public delegate void NewColorAvail(Color16Bit newColor);
        public event NewColorAvail NewColorAvailEvent;
        private bool running = false;
        private SmartProcSettingsWindow settings;
        public override bool IsDefault
        {
            get { return true; }
        }

        public override Guid id
        {
            get { return Guid.Parse("f4f3692d-a405-4dca-82fd-a9f3e9f93afe"); }
        }

        public override String Name
        {
            get { return "Antumbra Smart Screen Processor"; }
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
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override void AttachObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool Start()
        {
            this.running = true;
            return true;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgEvent += new NewLogMsg(observer.NewLogMsgAvail);
        }

        public override bool Settings()
        {
            this.settings = new SmartProcSettingsWindow(this);
            this.settings.Show();
            this.settings.useAllTxt.Text = Properties.Settings.Default.useAllTol.ToString();
            this.settings.minBrightTxt.Text = Properties.Settings.Default.minBright.ToString();
            this.settings.minMixTxt.Text = Properties.Settings.Default.minMixPerc.ToString();
            this.settings.scaleFactorTxt.Text = Properties.Settings.Default.scaleFactor.ToString();
            this.settings.scaleFactorTxt.TextChanged += new EventHandler(scaleFactorChanged);
            this.settings.useAllTxt.TextChanged += new EventHandler(useAllChanged);
            this.settings.minBrightTxt.TextChanged += new EventHandler(minBrightChanged);
            this.settings.minMixTxt.TextChanged += new EventHandler(minMixChanged);
            return true;
        }

        private void ApplyBtnClick(object sender, EventArgs args)
        {
            Properties.Settings.Default.Save();
        }

        private void useAllChanged(object sender, EventArgs args)
        {
            int i;
            TextBox box = (TextBox)sender;
            if (int.TryParse(box.Text, out i))
                Properties.Settings.Default.useAllTol = i;
        }

        private void minMixChanged(object sender, EventArgs args)
        {
            int i;
            TextBox box = (TextBox)sender;
            if (int.TryParse(box.Text, out i))
                Properties.Settings.Default.minMixPerc = i;
        }

        private void minBrightChanged(object sender, EventArgs args)
        {
            int i;
            TextBox box = (TextBox)sender;
            if (int.TryParse(box.Text, out i))
                Properties.Settings.Default.minBright = i;
        }

        private void scaleFactorChanged(object sender, EventArgs args)
        {
            int i;
            TextBox box = (TextBox)sender;
            if (int.TryParse(box.Text, out i))
                Properties.Settings.Default.scaleFactor = i;
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
            try {
                NewColorAvailEvent(Process(bm));
            }
            catch (Exception e) {
                if (e is ThreadAbortException) { }//swallow exception
                else
                    NewLogMsgEvent(this.Name, e.ToString());
            }
            finally {
                bm.Dispose();
            }
        }

        public Color16Bit Process(Bitmap screen)
        {
            if (screen == null) {
                NewLogMsgEvent(this.Name, "Null bitmap passed to process.");
                return null;
            }
            try {
                return SmartCalculateReprColor(screen, (int)Properties.Settings.Default["useAllTol"], (int)Properties.Settings.Default["minMixPerc"],
                    (int)Properties.Settings.Default["minBright"], (int)Properties.Settings.Default["scaleFactor"]);
            }
            catch (Exception e) {
                if (e is ThreadAbortException) { }//swallow, cause clutter in logs
                else {
                    NewLogMsgEvent(this.Name, e.ToString());
                }
                return null;
            }
            finally {
                screen.Dispose();
            }
        }

        private Color16Bit SmartCalculateReprColor(Bitmap bm, int useAllTolerance, int mixPercThreshold, int minBrightness, int scaleDownFactor)
        {
            if (bm == null)//sanity check
                return null;
            int newWidth = bm.Width / scaleDownFactor;
            int newHeight = bm.Height / scaleDownFactor;
            Bitmap small = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(small))
                g.DrawImage(bm, 0, 0, newWidth, newHeight);
            bm = small;
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
            small.Dispose();
            bm.Dispose();
            if (count == 0)//avoid dividing by zero
                return null;
            int avgR = (int)(totals[2] / count);
            int avgG = (int)(totals[1] / count);
            int avgB = (int)(totals[0] / count);
            return new Color16Bit(System.Drawing.Color.FromArgb(avgR, avgG, avgB));
        }
    }
}
