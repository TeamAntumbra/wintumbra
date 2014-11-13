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
        //public event EventHandler NewColor;//occurs when a new color is available
    }

    public abstract class GlowIndependentDriver : GlowDriver
    {
        //generates colors on its own
    }

    public abstract class GlowScreenDriverCoupler : GlowDriver
    {
        //encapsulates a GlowScreenDriver and GlowScreenProcessor and handles both of them
        //to generate colors for Glow
    }

    public abstract class GlowScreenDriver : GlowDriver, IObservable<Bitmap> //observed by screen processor
        //special type of driver that deals with bitmaps captured from the screen
        //uses a GlowScreenProcessor to determine color to return
    {
        //abstract public GlowScreenProcessor ScreenProcessor { get; }//return processor for this screen driver
        abstract public void captureTarget();//target method for capture thread (defines thread logic)
        public sealed override string Type { get { return "Screen Driver"; } }
        public abstract IDisposable Subscribe(IObserver<Bitmap> screenProcessor);//register observer
        //public event EventHandler NewScreen;//occurs when a new screen capture is available
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
        abstract public Color Process(Bitmap bm);//returns color based off Bitmap of screen
        public sealed override String Type { get { return "Screen Processor"; } }
        public abstract IDisposable Subscribe(IObserver<Color> observer);//register observer
        public event EventHandler NewColor;//occurs when a new color is available
        public abstract void HandleNewScreen(object sender, EventArgs args);
        public abstract void OnCompleted();//shut down and clean up
        public abstract void OnError(Exception e);//Handle exception
        public abstract void OnNext(Bitmap screen);//Handle new screen
    }
}
