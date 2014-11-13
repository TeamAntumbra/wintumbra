using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Antumbra.Glow.ExtensionFramework //NOTE FOR NOW IGNORE THE SEPARATE FILES AND ONLY USE THE STUFF HERE FOR EXTENSIONS!
{
  /*  public interface GlowExtension//basis for all Glow Extensions
    {
        String Name { get; }
        String Author { get; }
        String Version { get; }
        String Description { get; }
        String Type { get; }//one of the following: Decorator, Driver, Notifier, ScreenProcessor
    }*/

    public abstract class GlowExtension
    {
        abstract public String Name { get; }
        abstract public String Author { get; }
        abstract public String Version { get; }
        abstract public String Description { get; }
        abstract public String Type { get; }
    }

    public abstract class GlowDriver : GlowExtension, IObservable<Color>
    {
        //abstract public Color getColor();//main driver method (called repeatedly to get colors)
        public abstract bool start();//start driver
        public override String Type { get { return "Driver"; } }
        public abstract IDisposable Subscribe(IObserver<Color> observer);
    }

    public abstract class GlowIndependentDriver : GlowDriver
    {
        //generates colors on its own
    }

    public class GlowScreenDriverCoupler : GlowDriver, IObserver<Color>
        //generates color using a GlowScreenGrabber
        //and a GlowScreenProcessor
    {
        public GlowScreenGrabber grabber { get; set; }
        public GlowScreenProcessor processor { get; set; }
        private List<IObserver<Color>> observers;
        private AntumbraCore core;

        public GlowScreenDriverCoupler(AntumbraCore core)
        {
            this.core = core;
            this.observers = new List<IObserver<Color>>();
        }

        public sealed override string Name
        { get { return "Glow Screen Driver Coupler"; } }
        public sealed override string Author
        { get { return "Team Antumbra"; } }
        public sealed override string Description
        { get { return "A GlowDriver that uses a GlowScreenGrabber and "
                     + "a GlowScreenProcessor to generate colors"; } }
        public sealed override string Version
        { get { return "V0.0.1"; } }

        public override IDisposable Subscribe(IObserver<Color> observer)
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

        public override bool start()
        {
            if (this.grabber != null && this.processor != null) {
                if (this.grabber.ready() && this.processor.ready()) {//grabber & processor started correctly
                    this.grabber.Subscribe(this.processor);
                    this.processor.Subscribe(this);
                }
            }
            return false;
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(Color newColor)
        {

        }
    }

    public abstract class GlowScreenGrabber : GlowExtension, IObservable<Bitmap> //observed by screen processor
        //special type of driver that deals with bitmaps captured from the screen
        //uses a GlowScreenProcessor to determine color to return
    {
        //abstract public GlowScreenProcessor ScreenProcessor { get; }//return processor for this screen driver
        public sealed override string Type { get { return "Screen Grabber"; } }
        public abstract IDisposable Subscribe(IObserver<Bitmap> screenProcessor);//register observer
        public abstract bool ready();//get ready
    }

    public abstract class GlowDecorator : GlowExtension
    {
        abstract public Color Decorate(Color origColor);//Returns decorated color
        public sealed override String Type { get { return "Decorator"; } }
    }
    public abstract class GlowNotifier : GlowExtension, IObservable<Notification> //observed by core
    {
        abstract public Boolean Update();//returns true if notification exists
        abstract public Boolean Notify(AntumbraCore controller);
        //run color sequence configured for the notification
        //called after an Update() call returns true
        public sealed override String Type { get { return "Notifier"; } }
        public abstract IDisposable Subscribe(IObserver<Notification> notificationHandler);
        //public event EventHandler Notification;//occurs when a notification is detected
    }
    public struct Notification
    {
        private String NotiName;
        private String NotiDetails;

        public Notification(String Name, String Details)
        {
            this.NotiName = Name;
            this.NotiDetails = Details;
        }

        public String Name
        { get { return this.NotiName; } }

        public String Details
        { get { return this.NotiDetails; } }
    }
    public abstract class GlowScreenProcessor : GlowExtension, IObserver<Bitmap>, IObservable<Color>
    {
        public sealed override String Type { get { return "Screen Processor"; } }
        public abstract IDisposable Subscribe(IObserver<Color> observer);//register observer
        public abstract bool ready();//set up
        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public abstract void OnNext(Bitmap screen);
    }
}
