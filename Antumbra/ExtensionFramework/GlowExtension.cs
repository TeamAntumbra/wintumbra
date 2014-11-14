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
        public abstract bool ready();//get ready, respond true if ready
    }

    public abstract class GlowDriver : GlowExtension//, IObservable<Color>
    {
        //abstract public Color getColor();//main driver method (called repeatedly to get colors)
        public abstract bool start();//start driver
        public override String Type { get { return "Driver"; } }
        //public abstract IDisposable Subscribe(IObserver<Color> observer);
        public abstract void AttachEvent(AntumbraColorObserver observer);
    }

    public abstract class GlowIndependentDriver : GlowDriver
    {
        //generates colors on its own
    }

    public abstract class GlowScreenGrabber : GlowExtension//, IObservable<Bitmap> //observed by screen processor
        //special type of driver that deals with bitmaps captured from the screen
        //uses a GlowScreenProcessor to determine color to return
    {
        //abstract public GlowScreenProcessor ScreenProcessor { get; }//return processor for this screen driver
        public sealed override string Type { get { return "Screen Grabber"; } }
        //public abstract IDisposable Subscribe(IObserver<Bitmap> screenProcessor);//register observer
        public abstract void AttachEvent(AntumbraBitmapObserver observer);
        public abstract bool start();//start capturing
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
    public abstract class GlowScreenProcessor : GlowExtension//, IObserver<Bitmap>, IObservable<Color>
    { //child must inherit from AntumbraBitmapObserver to get screens from the grabber
        public sealed override String Type { get { return "Screen Processor"; } }
        //public abstract IDisposable Subscribe(IObserver<Color> observer);//register observer
        //public void OnCompleted() { }
        //public void OnError(Exception error) { }
        //public abstract void OnNext(Bitmap screen);
        public abstract void AttachEvent(AntumbraColorObserver observer);
    }

    public interface AntumbraColorObserver
    {
        void NewColorAvail(object sender, EventArgs args);
    }

    public interface AntumbraBitmapObserver
    {
        void NewBitmapAvail(object sender, EventArgs args);
    }

    public interface AntumbraNotificationObserver
    {
        void NewNotificationAvail(object sender, EventArgs args);
    }
}
