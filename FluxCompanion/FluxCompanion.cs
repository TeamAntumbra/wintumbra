using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FluxCompanion {

    [Export(typeof(GlowExtension))]
    public class FluxCompanion : GlowIndependentDriver {

        #region Private Fields

        private int deviceId;

        private Task driver;

        private long index;

        private bool running;

        #endregion Private Fields

        #region Public Delegates

        public delegate void NewColorAvail(Color16Bit newColor, int id, long index);

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
                return "An indpendent Glow driver meant to output warm colors based off the time of day, "
                    + "similar to the software F.lux.";
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
            get { return Guid.Parse("9d8efbe1-e33d-4047-a687-001883d5a124"); }
        }

        public override bool IsDefault {
            get { return false; }
        }

        public override bool IsRunning {
            get { return this.running; }
        }

        public override string Name {
            get { return "Flux Companion"; }
        }

        public override Version Version {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website {
            get { throw new NotImplementedException(); }
        }

        #endregion Public Properties

        #region Public Methods

        public override void AttachColorObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override GlowDriver Create() {
            return new FluxCompanion();
        }

        public override void Dispose() {
            if(driver != null) {
                driver.Dispose();
            }
        }

        public override void RecmmndCoreSettings() {
            stepSleep = 1000;
        }

        public override bool Settings() {
            return false;
        }

        public override bool Start() {
            running = true;
            driver = new Task(target);
            driver.Start();
            return true;
        }

        public override bool Stop() {
            this.running = false;
            if(driver != null) {
                if(driver.IsCompleted)
                    driver.Dispose();
                else {
                    driver.Wait(2000);
                    if(driver.IsCompleted)
                        driver.Dispose();
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private Color16Bit ConvertKelvinToColor(int kelvin) {/*http://www.tannerhelland.com/4435/convert-temperature-rgb-algorithm-code/ */
            int temp = kelvin / 100;
            int red = 0;
            int green = 0;
            if(temp <= 66) {
                red = 255;
                green = temp;
                green = (int)(99.4708025861 * Math.Log(green) - 161.1195681661);
                if(green < 0)
                    green = 0;
                else
                    if(green > 255)
                        green = 255;
            } else {
                red = temp - 60;
                red = (int)(329.698727446 * (Math.Pow(red, -0.1332047592)));
                if(red < 0)
                    red = 0;
                else
                    if(red > 255)
                        red = 255;
                green = temp - 60;
                green = (int)(288.1221695283 * Math.Pow(green, -0.0755148492));
                if(green < 0)
                    green = 0;
                else
                    if(green > 255)
                        green = 255;
            }
            int blue = 0;
            if(temp >= 66)
                blue = 255;
            else {
                blue = temp - 10;
                blue = (int)(138.5177312231 * Math.Log(blue) - 305.0447927307);
                if(blue < 0)
                    blue = 0;
                else
                    if(blue > 255)
                        blue = 255;
            }
            UInt16 r = Convert.ToUInt16(red);
            UInt16 g = Convert.ToUInt16(green);
            UInt16 b = Convert.ToUInt16(blue);
            Color16Bit result;
            result.red = r;
            result.green = g;
            result.blue = b;
            return result;
        }

        private int ConvertTimeToKelvin(int hour, int min, int sec) {
            int totalSec = sec + (60 * min) + (3600 * hour);
            int minKelvin = 1000;
            int maxKelvin = 40000;//TODO make configurable in settings
            double oneDay = 86400.0;
            double percDone = totalSec / oneDay;
            if(percDone > .75) {//getting darker
                percDone = (percDone - .75) * 4;//percent into last quarter
                return (int)((maxKelvin * (1.0 - percDone)) + (minKelvin * percDone));
            } else if(percDone >= .25) {//middle half of the day
                return maxKelvin;//as bright as possible
            } else {//first quarter of day - getting brighter
                percDone *= 4;//percent done with first quarter of day
                return (int)((maxKelvin * percDone) + (minKelvin * (1.0 - percDone)));
            }
        }

        private void target() {
            while(IsRunning) {
                DateTime now = DateTime.Now;
                int sec = now.Second;
                int min = now.Minute;
                int hour = now.Hour;
                NewColorAvailEvent(ConvertKelvinToColor(ConvertTimeToKelvin(hour, min, sec)), deviceId, index++);
                Thread.Sleep(stepSleep);
            }
        }

        #endregion Private Methods
    }
}
