using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Logging;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleGlowDriver {

    [Export(typeof(GlowExtension))]
    public class ExampleGlowDriver : GlowIndependentDriver, Loggable {

        #region Private Fields

        private int deviceId;

        private Task driver;

        private long index;

        private bool running;

        #endregion Private Fields

        #region Public Delegates

        public delegate void NewColorAvail(Color16Bit newColor, int id, long index);

        public delegate void NewLogMsg(String source, String msg);

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
                return "A super simple implementation example " +
                       "of a Glow Driver extension that always " +
                       "returns Random Colors! :)";
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
            get { return Guid.Parse("2c186c2f-3464-49b5-8737-50830231ff20"); }
        }

        public override bool IsDefault {
            get { return false; }
        }

        public override bool IsRunning {
            get { return running; }
        }

        public override string Name {
            get { return "Example Glow Driver"; }
        }

        public override Version Version {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website {
            get { return "https://antumbra.io/docs/extensions/driver/example"; }
        }

        #endregion Public Properties

        #region Public Methods

        public override void AttachColorObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgEvent += new NewLogMsg(observer.NewLogMsgAvail);
        }

        public override GlowDriver Create() {
            return new ExampleGlowDriver();
        }

        public override void Dispose() {
            if(driver != null) {
                driver.Dispose();
            }
        }

        public override void RecmmndCoreSettings() {
            this.stepSleep = 50;
        }

        public override bool Settings() {
            return false;//no custom window
        }

        public override bool Start() {
            index = long.MinValue;
            driver = new Task(target);
            driver.Start();
            running = true;
            return true;
        }

        public override bool Stop() {
            running = false;
            if(driver != null) {
                if(driver.IsCompleted)
                    driver.Dispose();
                else {
                    driver.Wait(1000);
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
            Random rnd = new Random();
            while(this.running) {
                //do stuff (logic of driver)
                UInt16 val = Convert.ToUInt16(rnd.Next(UInt16.MaxValue));
                Color16Bit result;
                result.red = val;
                result.green = val;
                result.blue = val;
                //report new color event
                try {
                    NewColorAvailEvent(result, deviceId, index++);
                } catch(Exception e) {
                    NewLogMsgEvent(this.Name, e.ToString());
                }
                Thread.Sleep(this.stepSleep);
            }
        }

        #endregion Private Methods
    }
}
