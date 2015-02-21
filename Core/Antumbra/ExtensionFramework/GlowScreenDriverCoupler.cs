using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Antumbra.Glow;

namespace Antumbra.Glow.ExtensionFramework
{
    public class GlowScreenDriverCoupler : GlowDriver, AntumbraColorObserver
    //generates color using a GlowScreenGrabber
    //and a GlowScreenProcessor
    {
        public delegate void NewColorAvail(Color newCol, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private GlowScreenGrabber grabber;
        private GlowScreenProcessor processor;
        public override int id { get; set; }

        public GlowScreenDriverCoupler(GlowScreenGrabber grab, GlowScreenProcessor proc)
        {
            this.grabber = grab;
            this.processor = proc;
        }

        public override bool IsRunning
        {
            get { if (null != this.grabber && null != this.processor)
                    return this.grabber.IsRunning && this.processor.IsRunning;
                return false;
            }
        }
        public sealed override string Name
        { get { return "Glow Screen Driver Coupler"; } }
        public sealed override string Author
        { get { return "Team Antumbra"; } }
        public sealed override string Description
        { get {
            return "A GlowDriver that uses a GlowScreenGrabber and "
                 + "a GlowScreenProcessor to generate colors";
            }
        }

        public sealed override bool IsDefault
        {
            get { return true; }
        }
        public sealed override Version Version
        { get { return new Version("0.0.1"); } }

        public sealed override string Website
        {
            get { return "https://antumbra.io/docs/extensions/framework/GlowScreenDriverCoupler"; }//TODO make docs and change this accordingly
        }

        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        void AntumbraColorObserver.NewColorAvail(Color newCol, EventArgs args)
        {
            NewColorAvailEvent(newCol, args);//pass it up
        }

        public override bool Start()
        {
            if (this.grabber != null && this.processor != null) {
                if (this.processor.Start()) {
                    if (this.processor is AntumbraBitmapObserver)
                        this.grabber.AttachEvent((AntumbraBitmapObserver)this.processor);
                    this.processor.AttachEvent(this);
                    if (this.grabber.Start()) {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool Stop()
        {
            if (this.processor != null)
                this.processor.Stop();
            if (this.grabber != null)
                this.grabber.Stop();
            return true;
        }

        public override void RecmmndCoreSettings()
        {
            this.stepSleep = 0;
            if (this.grabber != null)
                this.grabber.RecmmndCoreSettings();
        }

        public override bool Settings()
        {
            AntumbraExtSettingsWindow win = new AntumbraExtSettingsWindow(this);
            win.Show();
            return true;
        }
    }
}
