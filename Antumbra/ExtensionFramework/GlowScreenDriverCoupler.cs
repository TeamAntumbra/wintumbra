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

        public GlowScreenDriverCoupler(AntumbraCore core, GlowScreenGrabber grab, GlowScreenProcessor proc)
        {
            this.core = core;
            this.grabber = grab;
            this.processor = proc;
            this.observers = new List<IObserver<Color>>();
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
        public sealed override string Version
        { get { return "V0.0.1"; } }

        public override void AttachEvent(AntumbraColorObserver observer)
        {
            this.NewColorAvailEvent += new NewColorAvail(observer.NewColorAvail);
        }

        void AntumbraColorObserver.NewColorAvail(object sender, EventArgs args)
        {
            NewColorAvailEvent(sender, args);//pass it up
        }

        /*public override IDisposable Subscribe(IObserver<Color> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);
            return new Unsubscriber(this.observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Color>> _observers;
            private IObserver<Color> _observer;

            public Unsubscriber(List<IObserver<Color>> observers, IObserver<Color> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
        */
        public override bool ready()
        {
            if (this.grabber != null && this.processor != null) {
                if (this.grabber.ready() && this.processor.ready()) {//grabber & processor started correctly
                    /*this.grabber.Subscribe(this.processor);
                    this.processor.Subscribe(this);*/
                    if (this.processor is AntumbraBitmapObserver)
                        this.grabber.AttachEvent((AntumbraBitmapObserver)this.processor);
                    this.processor.AttachEvent(this);
                    return true;
                }
            }
            return false;
        }

        public override bool start()
        {
            //get ready and start
            return this.grabber.start();
        }

  /*      public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(Color newColor)
        {
            this.core.SetColorTo(newColor);
        }*/
    }
}
