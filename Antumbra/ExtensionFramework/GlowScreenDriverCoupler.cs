using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Antumbra.Glow;

namespace Antumbra.Glow.ExtensionFramework
{
    public class GlowScreenDriverCoupler : GlowDriver, AntumbraColorObserver//IObserver<Color>
    //generates color using a GlowScreenGrabber
    //and a GlowScreenProcessor
    {
        public delegate void NewColorAvail(object sender, EventArgs args);
        public event NewColorAvail NewColorAvailEvent;
        private GlowScreenGrabber grabber;
        private GlowScreenProcessor processor;
        private List<IObserver<Color>> observers;
        private AntumbraCore core;
        private Dictionary<string, object> settings;

        public GlowScreenDriverCoupler(AntumbraCore core, GlowScreenGrabber grab, GlowScreenProcessor proc)
        {
            this.core = core;
            this.grabber = grab;
            this.processor = proc;
            this.observers = new List<IObserver<Color>>();
            this.settings = new Dictionary<string, object>();
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
        public sealed override Version Version
        { get { return new Version("0.0.1"); } }

        public sealed override string Website
        {
            get { return "https://antumbra.io/docs/extensions/framework/GlowScreenDriverCoupler"; }//TODO make docs and change this accordingly
        }

        public sealed override Dictionary<string, object> Settings
        {
            get
            {
                return this.settings;
            }
            set
            {
                this.settings = Settings;
            }
        }

        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        void AntumbraColorObserver.NewColorAvail(object sender, EventArgs args)
        {
            NewColorAvailEvent(sender, args);//pass it up
        }

        public override bool Start()
        {
            bool ready = false;
            if (this.grabber != null && this.processor != null) {
                /*this.grabber.Subscribe(this.processor);
                this.processor.Subscribe(this);*/
                if (this.processor is AntumbraBitmapObserver)
                    this.grabber.AttachEvent((AntumbraBitmapObserver)this.processor);
                this.processor.AttachEvent(this);
                ready = true;
            }
            if (!ready)
                return false;
            //get ready and start
            return this.grabber.Start();
        }

        public override bool Stop()
        {
            return false;//TODO clean up
        }
    }
}
