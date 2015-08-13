using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using System.Reflection;

namespace HSVFade
{
    [Export(typeof(GlowExtension))]
    public class HSVFade : GlowIndependentDriver
    {
        public delegate void NewColorAvail(Color16Bit newColor, int id, long index);
        public event NewColorAvail NewColorAvailEvent;
        private Task driver;
        private bool running;
        private long index;
        private int deviceId;

        public override Guid id
        {
            get { return Guid.Parse("8360550b-d599-4f0f-8806-bc323f9ce547"); }
        }

        public override int devId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public override bool IsRunning
        {
            get { return running; }
        }

        public override string Name
        {
            get { return "HSV Fade"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A driver that does a continuous HSV color sweep."; }
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override bool IsDefault
        {
            get { return false; }
        }

        public override bool Settings()
        {
            return false;
        }

        public override GlowExtension Create()
        {
            return new HSVFade();
        }

        public override bool Start()
        {
            running = true;
            index = long.MinValue;
            driver = new Task(target);
            driver.Start();
            return true;
        }

        private void target()
        {
            int h = 0;
            while (this.IsRunning) {
                h += 1;
                h %= 360;
                HslColor col = new HslColor(h, 1, .5);
                NewColorAvailEvent(new Color16Bit(col.ToRgbColor()), deviceId, index++);
                if (stepSleep != 0)
                    Thread.Sleep(stepSleep);
            }
        }

        public override void AttachColorObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool Stop()
        {
            running = false;
            if (driver != null) {
                if (driver.IsCompleted)
                    driver.Dispose();
                else {
                    driver.Wait(3000);
                    if (driver.IsCompleted)
                        driver.Dispose();
                    else
                        return false;
                }
            }
            return true;
        }

        public override void RecmmndCoreSettings()
        {
            stepSleep = 100;
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }
        }

        public override void Dispose()
        {
            driver.Dispose();
        }
    }
}
