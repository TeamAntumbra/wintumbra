using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace FluxCompanion
{
    [Export(typeof(GlowExtension))]
    public class FluxCompanion : GlowIndependentDriver
    {
        public delegate void NewColorAvail(Color16Bit newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private bool running;
        private Task driver;

        public override Guid id
        {
            get { return Guid.Parse("9d8efbe1-e33d-4047-a687-001883d5a124"); }
        }

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
            this.running = true;
            this.driver = new Task(target);
            this.driver.Start();
            return true;
        }

        public override bool Settings()
        {
            return false;
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 1000;
        }

        public override void AttachColorObserver(AntumbraColorObserver observer)
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

        private Color16Bit ConvertKelvinToColor(int kelvin)
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
                else 
                    if (green > 255)
                       green = 255;
            }
            else {
                red = temp - 60;
                red = (int)(329.698727446 * (Math.Pow(red, -0.1332047592)));
                if (red < 0)
                    red = 0;
                else 
                    if (red > 255)
                        red = 255;
                green = temp - 60;
                green = (int)(288.1221695283 * Math.Pow(green,-0.0755148492));
                if (green < 0)
                    green = 0;
                else 
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
                else 
                    if (blue > 255)
                        blue = 255;
            }
            UInt16 r = Convert.ToUInt16(red);
            UInt16 g = Convert.ToUInt16(green);
            UInt16 b = Convert.ToUInt16(blue);
            return new Color16Bit(r, g, b);
        }

        public override bool Stop()
        {
            this.running = false;
            if (this.driver != null) {
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
                else {
                    this.driver.Wait(2000);
                    if (this.driver.IsCompleted)
                        this.driver.Dispose();
                }
            }
            return true;
        }

        public override string Description
        {
            get { return "An indpendent Glow driver meant to output warm colors based off the time of day, "
                + "similar to the software F.lux."; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }
    }
}
