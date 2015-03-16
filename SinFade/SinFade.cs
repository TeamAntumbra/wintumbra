using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
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
        public delegate void NewColorAvail(Color newCol, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private Task driver;
        private bool running;

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
            this.stepSleep = 100;
        }
    }
}
