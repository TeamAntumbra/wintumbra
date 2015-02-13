using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Forms;

namespace Antumbra.Glow.ExtensionFramework
{
    /// <summary>
    /// Interface for any GlowExtension
    /// </summary>
    public interface IGlowExtension
    {
        /// <summary>
        /// The id for the extension. Assigned at runtime.
        /// </summary>
        int id { get; set; }

        /// <summary>
        /// The name of the extension.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The author(s) of the extension.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// A description of the extension.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// A website for information regarding the extension.
        /// </summary>
        string Website { get; }

        /// <summary>
        /// The version of the extension.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// The running status of the extension.
        /// </summary>
        bool IsRunning { get; }
        
        /// <summary>
        /// Returns true if the extension is a default, else false.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Start this Extension
        /// </summary>
        bool Start();

        /// <summary>
        /// Stop this Extension
        /// </summary>
        bool Stop();
    }

    /// <summary>
    /// Abstract definition of a Glow Extension
    /// </summary>
    public abstract class GlowExtension : IGlowExtension
    {
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// The id of the current extension
        /// </summary>
        public abstract int id { get; set; }

        /// <summary>
        /// The name of the current extension
        /// </summary>
        [ReadOnly(true)]
        public abstract string Name { get; }
        /// <summary>
        /// The author of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract string Author { get; }

        /// <summary>
        /// A description of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract string Description { get; }
        /// <summary>
        /// A website for further information
        /// </summary>
        [ReadOnly(true)]
        public abstract string Website { get; }
        /// <summary>
        /// The version of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract Version Version { get; }

        /// <summary>
        /// The running status of this extension
        /// </summary>
        [ReadOnly(true)]
        public abstract bool IsRunning { get; }

        /// <summary>
        /// Returns true if the extension is a default, else false.
        /// </summary>
        [ReadOnly(true)]
        public abstract bool IsDefault { get; }

        /// <summary>
        /// Ready/Start this extension
        /// </summary>
        public abstract bool Start();

        /// <summary>
        /// Stop this extension, clean up
        /// </summary>
        public abstract bool Stop();
    }

    public abstract class GlowDriver : GlowExtension
    {
        public abstract void RecmmndCoreSettings();
        public abstract void AttachEvent(AntumbraColorObserver observer);
        public int stepSleep { get; set; }
    }

    public abstract class GlowIndependentDriver : GlowDriver
    {
        //generates colors on its own
    }

    public abstract class GlowScreenGrabber : GlowExtension//observed by screen processor
        //special type of driver that deals with bitmaps captured from the screen
        //uses a GlowScreenProcessor to determine color to return
    {
        public abstract void AttachEvent(AntumbraBitmapObserver observer);
        public abstract void RecmmndCoreSettings();
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public abstract class GlowDecorator : GlowExtension
    {
        abstract public Color Decorate(Color origColor);//Returns decorated color
    }
    public abstract class GlowNotifier : GlowExtension//observed by core
    {
        public abstract void AttachEvent(AntumbraNotificationObserver observer);
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
    public abstract class GlowScreenProcessor : GlowExtension, AntumbraBitmapObserver
    {
        public abstract void NewBitmapAvail(object sender, EventArgs args);
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
