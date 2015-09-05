using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HSVFade {

    [Export(typeof(GlowExtension))]
    public class HSVFade : GlowIndependentDriver {

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
            get { return "A driver that does a continuous HSV color sweep."; }
        }

        public override int devId {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public override Guid id {
            get { return Guid.Parse("8360550b-d599-4f0f-8806-bc323f9ce547"); }
        }

        public override bool IsDefault {
            get { return false; }
        }

        public override bool IsRunning {
            get { return running; }
        }

        public override string Name {
            get { return "HSV Fade"; }
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
            return new HSVFade();
        }

        public override void Dispose() {
            if(driver != null) {
                driver.Dispose();
            }
        }

        public override void RecmmndCoreSettings() {
            stepSleep = 100;
        }

        public override bool Settings() {
            return false;
        }

        public override bool Start() {
            running = true;
            index = long.MinValue;
            driver = new Task(target);
            driver.Start();
            return true;
        }

        public override bool Stop() {
            running = false;
            if(driver != null) {
                if(driver.IsCompleted)
                    driver.Dispose();
                else {
                    driver.Wait(3000);
                    if(driver.IsCompleted)
                        driver.Dispose();
                    else
                        return false;
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void target() {
            int h = 0;
            while(this.IsRunning) {
                h += 1;
                h %= 360;
                HslColor col = new HslColor(h, 1, .5);
                if(NewColorAvailEvent != null) {
                    NewColorAvailEvent(Color16BitUtil.FromRGBColor(col.ToRgbColor()), deviceId, index++);
                }
                if(stepSleep != 0)
                    Thread.Sleep(stepSleep);
            }
        }

        #endregion Private Methods
    }
}
