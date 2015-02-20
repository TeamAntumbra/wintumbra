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

namespace ExampleGlowDriver
{
    [Export(typeof(GlowExtension))]
    public class ExampleGlowDriver : GlowIndependentDriver
    {
        public delegate void NewColorAvail(Color newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private Task driver;
        private bool running;
        private AntumbraExtSettingsWindow settings;
        public override int id { get; set; }

        public override bool IsDefault
        {
            get { return false; }
        }

        public override bool Settings()
        {
            return false;//no custom window
        }
        
        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public override bool Setup()
        {
            return true;//no settings obj to setup
        }

        public override bool Start()
        {
            this.driver = new Task(target);
            this.driver.Start();
            this.running = true;
            return true;
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }

        private void target()
        {
            Random rnd = new Random();
            while (true) {
                //do stuff (logic of driver)
                Color result = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                //report new color event
                try {
                    NewColorAvailEvent(result, EventArgs.Empty);
                }
                catch (System.NullReferenceException) { }
                Thread.Sleep(this.stepSleep);
            }
        }

        public override bool Stop()
        {
            bool result = true;
            if (this.settings != null)
                this.settings.Dispose();
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
            get { return "Example Glow Driver"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override Version Version
        {
            get { return new Version("0.1.0"); }
        }

        public override string Description
        {
            get
            {
                return "A super simple implementation example " +
                       "of a Glow Driver extension that always " +
                       "returns Random Colors! :)";
            }
        }

        public override string Website
        {
            get { return "https://antumbra.io/docs/extensions/driver/example"; }
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 50;
        }
    }
}
