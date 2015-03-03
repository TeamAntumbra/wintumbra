using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;
using System.Drawing;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using System.Reflection;

namespace ColorClock
{
    [Export(typeof(GlowExtension))]
    public class ColorClock : GlowDriver
    {
        public delegate void NewColorAvail(Color newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private Task driver;
        private bool running = false;
        public override Guid id { get; set; }

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
            get { return "Color Clock"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A time based color clock with unique colors "
                       + "for every second of the day."; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website
        {
            get { return "https://antumbra.io/docs/extensions/drivers/colorClock"; }
        }

        public override void AttachColorObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool Start()
        {
            this.running = true;
            this.driver = new Task(driverTarget);
            this.driver.Start();
            return true;
        }

        public override bool Settings()
        {
            return false;//no custom settings
        }

        public override bool Stop()
        {
            this.running = false;
            if (null != this.driver) {
                this.driver.Wait(2000);
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
            }
            return true;
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 1000;
        }

        private void driverTarget()
        {
            while (this.running) {
                if (NewColorAvailEvent == null) {}//no one is listening, do nothing...
                else
                    NewColorAvailEvent(getTimeColor(DateTime.Now), EventArgs.Empty);
                Thread.Sleep(this.stepSleep);
            }
        }

        private Color getTimeColor(DateTime time)
        {
            double secondsFromStart = (time - DateTime.Today).TotalSeconds;//0-86400
            double fraction = secondsFromStart / 86400.0;
            double[] hsv = new double[3];
            hsv[0] = (double)(secondsFromStart%3600);//12 rotations over half a day
            hsv[1] = (double)(2.0*Math.Abs(fraction-.5));//0 -> 1 -> 0 over a day
            if (fraction < .25)//first 4th
                hsv[2] = fraction / .25;//up to 1
            else if (fraction > .75)//last 4th
                hsv[2] = (1-fraction)/.25;//down to 0
            else//middle 8ths
                hsv[2] = 1;
            int[] values = HSVRGGConverter.HSVToRGB(hsv[0], hsv[1], hsv[2]);
            return Color.FromArgb(values[0], values[1], values[2]);
        }
    }
}
