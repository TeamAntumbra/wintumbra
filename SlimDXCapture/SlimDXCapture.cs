using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework;
using System.ComponentModel.Composition;
using SlimDX.Direct3D9;
using System.Reflection;
using System.Drawing;
using SlimDX;

namespace SlimDXCapture
{
    [Export(typeof(GlowExtension))]
    public class SlimDXCapture : GlowScreenGrabber
    {
        private bool running;
        private Task driver;
        private Surface surf;
        public delegate void NewScreenAvail(Bitmap screen, EventArgs args);
        public event NewScreenAvail NewScreenAvailEvent;
        public override Guid id { get; set; }
        public override string Name
        {
            get { return "SlimDX Capture"; }
        }

        public override string Author
        {
            get { return "Team Antumbra"; }
        }

        public override string Description
        {
            get { return "A capture extension that uses the SlimDX runtime."; }
        }

        public override string Website
        {
            get { throw new NotImplementedException(); }    
        }

        public override bool Start()
        {
            this.driver = new Task(new Action(Target));
            this.running = true;
            this.driver.Start();
            return true;
        }

        public override void AttachEvent(AntumbraBitmapObserver observer)
        {
            this.NewScreenAvailEvent += new NewScreenAvail(observer.NewBitmapAvail);
        }

        private void Target()
        {
            while (this.running) {
                try {
                    SlimDXCapturer capt = new SlimDXCapturer();
                    surf = capt.getScreenShot(this.width, this.height);
                    Bitmap screen = GetBitmapFromSurface(surf);
                    NewScreenAvailEvent(screen, EventArgs.Empty);
                    screen.Dispose();
                }
                catch (SlimDX.Direct3D9.Direct3D9Exception) { }//swallow exceptions (TODO change once implemented log listener in core)
                finally {
                    surf.Dispose();
                }
            }
        }

        private Bitmap GetBitmapFromSurface(Surface s)
        {
            return new Bitmap(Surface.ToStream(s, ImageFileFormat.Bmp));
            //return new Bitmap(SlimDX.Direct3D9.Surface.ToStream(s, SlimDX.Direct3D9.ImageFileFormat.Bmp));
        }

        public override bool Stop()
        {
            this.running = false;
            if (this.driver != null) {
                if (this.driver.IsCompleted)
                    this.driver.Dispose();
                else {
                    this.driver.Wait(2000);
                    if (this.driver.IsCompleted)
                        this.driver.Dispose();
                }
            }
            return true;
        }

        public override bool Settings()
        {
            return false;
        }

        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public override bool IsDefault
        {
            get { return false; }
        }

        public override bool IsRunning
        {
            get { return this.running; }
        }
    }
}
