using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Colors;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Antumbra.Glow.Observer.Logging;

namespace ExampleGlowDriver
{
    [Export(typeof(GlowExtension))]
    public class ExampleGlowDriver : GlowIndependentDriver, Loggable
    {
        public delegate void NewColorAvail(Color16Bit newColor, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgEvent;
        private Task driver;
        private bool running;

        public override bool IsDefault
        {
            get { return false; }
        }

        public override Guid id
        {
            get { return Guid.Parse("2c186c2f-3464-49b5-8737-50830231ff20"); }
        }

        public override bool Settings()
        {
            return false;//no custom window
        }
        
        public override void AttachColorObserver(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgEvent += new NewLogMsg(observer.NewLogMsgAvail);
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
            while (this.running) {
                //do stuff (logic of driver)
                UInt16 val = Convert.ToUInt16(rnd.Next(UInt16.MaxValue));
                Color16Bit result = new Color16Bit(val, val, val);
                //report new color event
                try {
                    NewColorAvailEvent(result, EventArgs.Empty);
                }
                catch (Exception e) {
                    NewLogMsgEvent(this.Name, e.ToString());
                }
                Thread.Sleep(this.stepSleep);
            }
        }

        public override bool Stop()
        {
            this.running = false;
            if (this.driver != null) {
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
                else {
                    this.driver.Wait(1000);
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
            get { return "Example Glow Driver"; }
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
