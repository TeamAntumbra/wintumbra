using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SinFade
{
    [Export(typeof(GlowExtension))]
    public class SinFade : GlowIndependentDriver
    {
        public delegate void NewColorAvail(Color newCol, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private Task driver;
        private bool running;
        public override int id { get; set; }

        public override bool IsDefault
        {
            get { return false; }
        }
        
        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool Start()
        {
            this.running = true;
            this.driver = new Task(target);
            this.driver.Start();
            return true;
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        public override bool Settings()
        {
            return false;
        }

        /// <summary>
        /// Target of the independent driver task.
        /// </summary>
        private void target()
        {
            bool up = true;
            int value = 0;
            while (running) {
                Color result = Color.FromArgb(value, value, value);
                try {
                    NewColorAvailEvent(result, EventArgs.Empty);
                    Thread.Sleep(this.stepSleep);
                    if (up)
                        if (value == 255)
                            up = false;//turn around
                        else
                            value += 1;
                    else//down
                        if (value == 0)
                            up = true;//turn around
                        else
                            value -= 1;
                }
                catch (System.NullReferenceException) { }
            }
        }

        public override bool Stop()
        {
            bool result = true;
            if (this.driver != null) {
                this.driver.Wait(1000);
                if (!this.driver.IsCompleted)
                    result = false;
                this.driver = null;
            }
            this.running = false;
            return result;
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
            get { return new Version("0.0.1"); }
        }

        public override string Description
        {
            get
            {
                return "A simle sin fade.";
            }
        }

        public override string Website
        {
            get { return "https://antumbra.io/docs/extensions/driver/example"; }//TODO
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 1000;
        }
    }
}
