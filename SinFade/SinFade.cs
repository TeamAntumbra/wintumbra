using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace SinFade
{
    [Export(typeof(GlowExtension))]
    public class SinFade : GlowIndependentDriver
    {
        public delegate void NewColorAvail(Color16Bit newCol, int id, long index);
        public event NewColorAvail NewColorAvailEvent;
        private Task driver;
        private bool running;
        private long index;
        private int deviceId;

        public override GlowExtension Create()
        {
            return new SinFade();
        }

        public override int devId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
            }
        }

        public override Guid id
        {
            get { return Guid.Parse("31cae25b-72c0-4ffc-860b-234fb931bc15"); }
        }

        public override bool IsDefault
        {
            get { return false; }
        }
        
        public override void AttachColorObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool Start()
        {
            running = true;
            index = long.MinValue;
            driver = new Task(target);
            driver.Start();
            return true;
        }

        public override bool IsRunning
        {
            get { return running; }
        }

        public override bool Settings()
        {
            return false;
        }

        public override void Dispose()
        {
            if (driver != null) {
                driver.Dispose();
            }
        }

        /// <summary>
        /// Target of the independent driver task.
        /// </summary>
        private void target()
        {
            double i = 0;
            while (running) {
                double value = Math.Abs(Math.Sin(i) * UInt16.MaxValue);
                ushort v = Convert.ToUInt16(value);
                Color16Bit result = new Color16Bit(v, v, v);
                if(NewColorAvailEvent != null)
                    NewColorAvailEvent(result, deviceId, index++);
                if (v == 0)
                    Thread.Sleep(this.stepSleep * 39);
                Thread.Sleep(this.stepSleep);
                i += .03;
            }
        }

        public override bool Stop()
        {
            this.running = false;
            if (this.driver != null) {
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
                else {
                    this.driver.Wait(3000);
                    if (this.driver.IsCompleted)
                        this.driver.Dispose();
                    else
                        return false;
                }
            }
            return true;
        }

        public override string Name
        {
            get { return "Sin Fade"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override string Description
        {
            get
            {
                return "A simple sin fade.";
            }
        }

        public override string Website
        {
            get { return "https://antumbra.io/docs/extensions/driver/example"; }//TODO
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 50;
        }
    }
}
