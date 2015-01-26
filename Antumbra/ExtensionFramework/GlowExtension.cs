using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Runtime.Serialization;

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
    public interface IGlowExtension
    {   
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The author(s) of the plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// A description of the plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// A website for information regarding the plugin.
        /// </summary>
        string Website { get; }

        /// <summary>
        /// The version of the plugin.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Start this Plugin
        /// </summary>
        bool Start();

        /// <summary>
        /// Stop this Plugin
        /// </summary>
        bool Stop();
    }

    [DataContract]
    public abstract class GlowExtension : IGlowExtension
    {
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// The name of the current plugin
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Name", Order = -600)]
        [DataMember]
        public abstract string Name { get; }
        /// <summary>
        /// The author of this plugin
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Author", Order = -500)]
        [DataMember]
        public abstract string Author { get; }

        /// <summary>
        /// A description of this plugin
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Description", Order = -400)]
        [DataMember]
        public abstract string Description { get; }
        /// <summary>
        /// A website for further information
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Website", Order = -300)]
        [DataType(DataType.Url)]
        [DataMember]
        public abstract string Website { get; }
        /// <summary>
        /// The version of this plugin
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Version", Order = -200)]
        [DataMember]
        public abstract Version Version { get; }

        /// <summary>
        /// Start this Plugin
        /// </summary>
        public abstract bool Start();

        /// <summary>
        /// Stop this Plugin
        /// </summary>
        public abstract bool Stop();
    }

    /*public abstract class GlowExtension
    {
        abstract public String Name { get; }
        abstract public String Author { get; }
        abstract public String Version { get; }
        abstract public String Description { get; }
        abstract public String Type { get; }
        abstract public String Website { get; }
        public abstract bool Start();
        public abstract bool Stop();
        //public abstract bool ready();//get ready, respond true if ready
    }*/

    public abstract class GlowDriver : GlowExtension//, IObservable<Color>
    {
        //abstract public Color getColor();//main driver method (called repeatedly to get colors)
        //public abstract bool start();//start driver
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
        //public abstract IDisposable Subscribe(IObserver<Bitmap> screenProcessor);//register observer
        public abstract void AttachEvent(AntumbraBitmapObserver observer);
    }

    public abstract class GlowDecorator : GlowExtension
    {
        abstract public Color Decorate(Color origColor);//Returns decorated color
    }
    public abstract class GlowNotifier : GlowExtension, IObservable<Notification> //observed by core
    {
        public abstract IDisposable Subscribe(IObserver<Notification> observer);
        //run color sequence configured for the notification
        //called after an Update() call returns true
        //public abstract IDisposable Subscribe(IObserver<Notification> notificationHandler);
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
