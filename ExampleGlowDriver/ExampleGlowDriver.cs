using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework;
using System.ComponentModel.Composition;
using System.Threading;

namespace ExampleGlowDriver
{
    [Export(typeof(GlowExtension))]
    public class ExampleGlowDriver : GlowIndependentDriver
    {
        //public event EventHandler NewColor;//occurs when a new color is available
        private Thread driver;

        public ExampleGlowDriver()
        {
            
        }

        public override bool ready()
        {
            this.driver = new Thread(new ThreadStart(threadLogic));
            return true;
        }

        public override bool start()
        {
            this.driver.Start();
            return true;
        }

        private void threadLogic()
        {
            Random rnd = new Random();
            while (true) {
                //do stuff (logic of driver)
                Color result = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                //report new color event
            }
        }

        public bool stop()
        {
            this.driver.Abort();
            this.driver = null;
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

        public override string Version
        {
            get { return "V0.1.0"; }
        }

        public override string Description
        {
            get { return "A super simple implementation example " +
                         "of a Glow Driver extension that always " +
                         "returns Random Colors! :)"; }
        }
    }
}
