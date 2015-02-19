using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using System.Threading;
using System.Windows.Forms;

namespace FluxCompanion
{
    [Export(typeof(GlowExtension))]
    public class FluxCompanion : GlowIndependentDriver
    {
        public delegate void NewColorAvail(Color newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        public override int id { get; set; }
        private bool running;
        private Task driver;
        private AntumbraExtSettingsWindow settings;

        public override bool IsRunning
        {
            get { return this.running; }
        }
        public override string Name
        {
            get { return "Flux Companion"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override bool IsDefault
        {
            get { return false; }
        }

        public override bool Start()
        {
            this.stepSleep = 2000;
            this.running = true;
            this.driver = new Task(target);
            this.driver.Start();
            return true;
        }

        public override void Settings()
        {
            this.settings = new AntumbraExtSettingsWindow(this);
            this.settings.Show();
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 60000;//60 sec
        }

        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        private void target()
        {
            while (this.IsRunning) {
                DateTime now = DateTime.Now;
                int sec = now.Second;
                int min = now.Minute;
                int hour = now.Hour;
                NewColorAvailEvent(ConvertKelvinToColor(ConvertTimeToKelvin(hour, min, sec)), EventArgs.Empty);
                Thread.Sleep(this.stepSleep);
            }
        }

        private int ConvertTimeToKelvin(int hour, int min, int sec)
        {
            int totalSec = sec + (60 * min) + (3600 * hour);
            int minKelvin = 1000;
            int maxKelvin = 40000;//TODO make configurable in settings
            double oneDay = 86400.0;
            double percDone = totalSec / oneDay;
            if (percDone > .75) {//getting darker
                percDone = (percDone - .75) * 4;//percent into last quarter
                return (int)((maxKelvin * (1.0 - percDone)) + (minKelvin * percDone));
            }
            else if (percDone >= .25) {//middle half of the day
                return maxKelvin;//as bright as possible
            }
            else {//first quarter of day - getting brighter
                percDone *= 4;//percent done with first quarter of day
                return (int)((maxKelvin * percDone) + (minKelvin * (1.0 - percDone)));
            }
        }

        private Color ConvertKelvinToColor(int kelvin)
        {/*http://www.tannerhelland.com/4435/convert-temperature-rgb-algorithm-code/ */
            int temp = kelvin / 100;
            int red = 0;
            int green = 0;
            if (temp <= 66) {
                red = 255;
                green = temp;
                green = (int)(99.4708025861 * Math.Log(green) - 161.1195681661);
                if (green < 0)
                    green = 0;
                if (green > 255)
                    green = 255;
            }
            else {
                red = temp - 60;
                red = (int)(329.698727446 * (Math.Pow(red, -0.1332047592)));
                if (red < 0)
                    red = 0;
                if (red > 255)
                    red = 255;
                green = temp - 60;
                green = (int)(288.1221695283 * Math.Pow(green,-0.0755148492));
                if (green < 0)
                    green = 0;
                if (green > 255)
                    green = 255;
            }
            int blue = 0;
            if (temp >= 66)
                blue = 255;
            else {
                blue = temp - 10;
                blue = (int)(138.5177312231 * Math.Log(blue) - 305.0447927307);
                if (blue < 0)
                    blue = 0;
                if (blue > 255)
                    blue = 255;
            }
            return Color.FromArgb(red, green, blue);
        }

        public override bool Stop()
        {
            if (this.settings != null)
                this.settings.Dispose();
            this.running = false;
            return true;
        }

        public override string Description
        {
            get { return "An indpendent Glow driver meant to output warm colors based off the time of day, "
                + "similar to the software F.lux."; }
        }

        public override Version Version
        {
            get { return new Version("0.0.1"); }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }
    }
}
