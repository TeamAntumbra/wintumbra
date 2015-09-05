using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Logging;
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SinFade {

    [Export(typeof(GlowExtension))]
    public class SinFade : GlowIndependentDriver, Loggable {

        #region Private Fields

        private int deviceId;

        private Task driver;

        private long index;

        private bool running;

        #endregion Private Fields

        #region Public Delegates

        public delegate void NewColorAvail(Color16Bit newCol, int id, long index);

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
                return "A simple sin fade.";
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
            get { return Guid.Parse("31cae25b-72c0-4ffc-860b-234fb931bc15"); }
        }

        public override bool IsDefault {
            get { return false; }
        }

        public override bool IsRunning {
            get { return running; }
        }

        public override string Name {
            get { return "Sin Fade"; }
        }

        public override Version Version {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Website {
            get { return "https://antumbra.io/docs/extensions/driver/example"; }//TODO
        }

        #endregion Public Properties

        #region Public Constructors

        public SinFade() {
            AttachObserver(LoggerHelper.GetInstance());
        }

        #endregion Public Constructors

        #region Public Methods

        public override void AttachColorObserver(AntumbraColorObserver observer) {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgEvent += observer.NewLogMsgAvail;
        }

        public override GlowDriver Create() {
            return new SinFade();
        }

        public override void Dispose() {
            if(driver != null) {
                try {
                    driver.Dispose();
                } catch(InvalidOperationException ex) {
                    Log(ex.Message + '\n' + ex.StackTrace);
                }
            }
        }

        public override void RecmmndCoreSettings() {
            this.stepSleep = 50;
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
            this.running = false;
            if(this.driver != null) {
                if(this.driver.IsCompleted)
                    this.driver.Dispose();
                else {
                    this.driver.Wait(3000);
                    if(this.driver.IsCompleted)
                        this.driver.Dispose();
                    else
                        return false;
                }
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void Log(string msg) {
            if(NewLogMsgEvent != null) {
                NewLogMsgEvent("SinFade", msg);
            }
        }

        /// <summary>
        /// Target of the independent driver task.
        /// </summary>
        private void target() {
            double i = 0;
            while(running) {
                double value = Math.Abs(Math.Sin(i) * UInt16.MaxValue);
                ushort v = Convert.ToUInt16(value);
                Color16Bit result;
                result.red = v;
                result.green = v;
                result.blue = v;
                if(NewColorAvailEvent != null)
                    NewColorAvailEvent(result, deviceId, index++);
                Thread.Sleep(stepSleep / 3);
                i += .005;
            }
        }

        #endregion Private Methods
    }
}
