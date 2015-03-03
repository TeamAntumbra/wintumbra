using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Settings;
using System.Drawing;
using Antumbra.Glow.Logging;
using Antumbra.Glow.ToolbarNotifications;
using Antumbra.Glow.GlowCommands;

namespace Antumbra.Glow.ExtensionFramework
{
    /// <summary>
    /// Manages the Extensions for use with a Glow device
    /// </summary>
    public class ExtensionManager : AntumbraColorObserver, LogMsgObserver, Loggable,
                                    ToolbarNotificationObserver, ToolbarNotificationSource,
                                    GlowCommandObserver, GlowCommandSender//TODO add observer for notifiers
    {
        /// <summary>
        /// Delegate for NewColorAvailEvent, handles a new Color being available
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="args"></param>
        public delegate void NewColorAvail(Color newColor, EventArgs args);
        /// <summary>
        /// NewColorAvail Event, occurs when a new color is available
        /// </summary>
        public event NewColorAvail NewColorAvailEvent;
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvailEvent;
        public delegate void NewToolbarNotif(int time, String title, String msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommand(GlowCommand command);
        public event NewGlowCommand NewGlowCommandEvent;
        /// <summary>
        /// DeviceSettings obj for the GlowDevice relating to this ExtensionManager
        /// </summary>
        private DeviceSettings settings;
        /// <summary>
        /// The id for the GlowDevice relating to this ExtensionManager
        /// </summary>
        public int id { get; private set; }
        /// <summary>
        /// Active GlowDriver obj
        /// </summary>
        public GlowDriver ActiveDriver { get; set; }
        /// <summary>
        /// Active GlowScreenGrabber obj
        /// </summary>
        public GlowScreenGrabber ActiveGrabber { get; set; }
        /// <summary>
        /// Active GlowScreenProcessor obj
        /// </summary>
        public GlowScreenProcessor ActiveProcessor { get; set; }
        /// <summary>
        /// List of Active GlowDecorators
        /// </summary>
        public List<GlowDecorator> ActiveDecorators { get; set; }
        /// <summary>
        /// List of Active GlowNotifiers
        /// </summary>
        public List<GlowNotifier> ActiveNotifiers { get; set; }
        /// <summary>
        /// Constructor - Creates a new ExtensionManager relating to the GlowDevice
        /// with the same id as passed.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="settings"></param>
        public ExtensionManager(ExtensionLibrary extLib, int id, DeviceSettings settings)
        {
            this.settings = settings;
            this.id = id;
            this.ActiveDriver = extLib.GetDefaultDriver();
            this.ActiveGrabber = extLib.GetDefaultGrabber();
            this.ActiveProcessor = extLib.GetDefaultProcessor();
            this.ActiveDecorators = extLib.GetDefaultDecorators();
            this.ActiveNotifiers = extLib.GetDefaultNotifiers();
        }
        /// <summary>
        /// Return a string describing the current active extensions
        /// </summary>
        /// <returns></returns>
        public string GetSetupDesc()
        {
            if (this.ActiveDriver == null)
                return "The current driver is null. Extension loading probably failed.";
            string result = "Driver: ";
            if (ActiveDriver is GlowScreenDriverCoupler) {
                if (ActiveGrabber != null && ActiveProcessor != null)
                    result += ActiveDriver.ToString()
                        + "\nGrabber: " + ActiveGrabber.ToString() + "\nProcessor: "
                        + ActiveProcessor.ToString();
                else
                    result += ActiveDriver.ToString();
            }
            else
                result += ActiveDriver.ToString();
            result += "\nDecorators: ";
            foreach (var dec in ActiveDecorators)
                result += dec.ToString() + ' ';
            result += "\nNotifiers: ";
            foreach (var notf in ActiveNotifiers)
                result += notf.ToString() + ' ';
            return result;
        }
        /// <summary>
        /// Attach an AntumbraColorObserver to the NewColorAvailEvent for this ExtensionManager
        /// </summary>
        /// <param name="observer"></param>
        public void AttachEvent(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        public void AttachLogObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachToolbarNotifObserver(ToolbarNotificationObserver observer)
        {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachGlowCommandObserver(GlowCommandObserver observer)
        {
            NewGlowCommandEvent += observer.NewGlowCommandAvail;
        }

        public void RegisterDevice(int id)
        {
            //ignore, already have id
            if (this.id != id)
                throw new Exception("137 ext mgr");
        }

        public void NewToolbarNotifAvail(int time, String title, String msg, int icon)
        {
            NewToolbarNotifAvailEvent(time, title, msg, icon);
        }

        public void NewLogMsgAvail(String source, String msg)
        {
            NewLogMsgAvailEvent(source, msg);
        }

        public void NewGlowCommandAvail(GlowCommand command)
        {
            NewGlowCommandEvent(command);//pass it up to core
        }
        /// <summary>
        /// Event handler for the NewColorAvail event
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="args"></param>
        void AntumbraColorObserver.NewColorAvail(Color newColor, EventArgs args)
        {
            if (ActiveDecorators.Count == 0) {
                NewColorAvailEvent(newColor, args);//no decoration to do
                return;
            }
            if (this.settings.compoundDecoration) {
                foreach (var dec in ActiveDecorators)//decorate
                    newColor = dec.Decorate(newColor);
                NewColorAvailEvent(newColor, args);
                return;
            }
            //average decorators output
            int r = 0, g = 0, b = 0;
            foreach (var dec in ActiveDecorators) {
                Color decorated = dec.Decorate(newColor);
                r += decorated.R;
                g += decorated.G;
                b += decorated.B;
            }
            int count = ActiveDecorators.Count;
            NewColorAvailEvent(Color.FromArgb(r / count, g / count, b / count), args);
        }
        /// <summary>
        /// Start active extensions
        /// </summary>
        /// <returns>True if successful, else false</returns>
        public bool Start()
        {
            if (!Verify())
                return false;
            this.ActiveDriver.AttachEvent(this);
            if (this.ActiveDriver is Loggable) {
                Loggable log = (Loggable)this.ActiveDriver;
                log.AttachLogObserver(this);
            }
            if (this.ActiveDriver is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)this.ActiveDriver;
                sender.AttachGlowCommandObserver(this);
            }
            if (!this.ActiveDriver.Start())
                return false;
            foreach (var dec in ActiveDecorators)
                if (!dec.Start())
                    return false;
            foreach (var notf in ActiveNotifiers)
                if (!notf.Start())
                    return false;
            return true;
        }
        /// <summary>
        /// Verify the activated extensions
        /// </summary>
        /// <returns>True if verified, else false</returns>
        private bool Verify()
        {
            if (this.settings == null)
                return false;
            if (ActiveDriver is GlowScreenDriverCoupler) {//screen based driver selected
                if (null == ActiveGrabber || null == ActiveProcessor) {//no grabber or processor set
                    return false;
                }
                ActiveGrabber.x = this.settings.x;//set screen related settings for grabber
                ActiveGrabber.y = this.settings.y;
                ActiveGrabber.width = this.settings.width;
                ActiveGrabber.height = this.settings.height;
                Guid placeHolder = ActiveDriver.id;
                ActiveDriver = new GlowScreenDriverCoupler(ActiveGrabber, ActiveProcessor);
                ActiveDriver.id = placeHolder;
            }
            ActiveDriver.stepSleep = settings.stepSleep;
            return true;
        }
        /// <summary>
        /// Stop the active extensions
        /// </summary>
        /// <returns>True if successful, else false</returns>
        public bool Stop()
        {
            bool result = true;
            if (!this.ActiveDriver.Stop())//coupler will stop grabber & processor
                result = false;
            foreach (var dec in ActiveDecorators)
                if (!dec.Stop())
                    result = false;
            foreach (var notf in ActiveNotifiers)
                if (!notf.Stop())
                    result = false;
            return result;
        }
    }
}
