using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Settings;
using System.Drawing;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Utility.Settings;
using Antumbra.Glow.ExtensionFramework.Types;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    /// <summary>
    /// Manages the Extensions for use with a Glow device
    /// </summary>
    public class ExtensionManager : AntumbraColorObserver, LogMsgObserver, Loggable,
                                    ToolbarNotificationObserver, ToolbarNotificationSource,
                                    GlowCommandObserver, GlowCommandSender, Savable//TODO add observer for notifiers
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
        public GlowDriver ActiveDriver { get; private set; }
        /// <summary>
        /// Active GlowScreenGrabber obj
        /// </summary>
        public GlowScreenGrabber ActiveGrabber { get; private set; }
        /// <summary>
        /// Active GlowScreenProcessor obj
        /// </summary>
        public GlowScreenProcessor ActiveProcessor { get; private set; }
        /// <summary>
        /// List of Active GlowDecorators
        /// </summary>
        public List<GlowDecorator> ActiveDecorators { get; private set; }
        /// <summary>
        /// List of Active GlowNotifiers
        /// </summary>
        public List<GlowNotifier> ActiveNotifiers { get; private set; }
        private ExtensionLibrary lib;
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
            this.lib = extLib;
            this.id = id;
            this.ActiveDriver = extLib.GetDefaultDriver();
            this.ActiveGrabber = extLib.GetDefaultGrabber();
            this.ActiveProcessor = extLib.GetDefaultProcessor();
            this.ActiveDecorators = extLib.GetDefaultDecorators();
            this.ActiveNotifiers = extLib.GetDefaultNotifiers();
        }

        public void SaveSettings()
        {
            Saver saver = Saver.GetInstance();
            saver.Save("ExtMgr", SerializeActiveExts());
        }

        public void LoadSettings(String settings)
        {
            String[] parts = settings.Split(',');
            this.ActiveDriver = (GlowDriver)this.lib.findExt(Guid.Parse(parts[0]));
            this.ActiveGrabber = (GlowScreenGrabber)this.lib.findExt(Guid.Parse(parts[1]));
            this.ActiveProcessor = (GlowScreenProcessor)this.lib.findExt(Guid.Parse(parts[2]));
            this.ActiveDecorators.Clear();
            foreach (String dec in parts[3].Split(' '))
                this.ActiveDecorators.Add((GlowDecorator)this.lib.findExt(Guid.Parse(dec)));
            this.ActiveNotifiers.Clear();
            foreach (String notf in parts[4].Split(' '))
                this.ActiveNotifiers.Add((GlowNotifier)this.lib.findExt(Guid.Parse(notf)));
        }

        public void ResetSettings()
        {
            //TODO load defaults
        }

        private String SerializeActiveExts()
        {
            String result = "";
            result += this.ActiveDriver.id.ToString() + ',';
            result += this.ActiveGrabber.id.ToString() + ',';
            result += this.ActiveProcessor.id.ToString() + ',';
            int count = this.ActiveDecorators.Count;
            for (int i = 0; i < count; i += 1) {
                GlowDecorator dec = this.ActiveDecorators[i];
                result += dec.id.ToString();
                if (i != count - 1)//not the last one
                    result += ' ';
            }
            result += ',';
            count = this.ActiveNotifiers.Count;
            for (int i = 0; i < count; i += 1) {
                GlowNotifier notf = this.ActiveNotifiers[i];
                result += notf.id.ToString();
                if (i != count - 1)//not the last one
                    result += ' ';
            }
            result += ',';
            return result;
        }

        public void UpdateExtension(Guid id)
        {
            GlowExtension ext = this.lib.findExt(id);
            if (ext == null)
                throw new Exception("Invalid Guid found while updating extension");
            if (ext is GlowDriver)
                this.ActiveDriver = (GlowDriver)ext;
            else if (ext is GlowScreenGrabber)
                this.ActiveGrabber = (GlowScreenGrabber)ext;
            else if (ext is GlowScreenProcessor)
                this.ActiveProcessor = (GlowScreenProcessor)ext;
            //decorators and notifiers are handled through their toggler
        }

        public bool ToggleDecOrNotf(Guid id)
        {
            GlowExtension ext = this.lib.findExt(id);
            if (ext == null)
                throw new Exception("Invalid extension id when toggling extension");
            bool isActive = false;
            if (ext is GlowDecorator) {
                GlowDecorator dec = (GlowDecorator)ext;
                isActive = CheckForActiveDec(dec.id);
                if (isActive)
                    RemoveDec(dec.id);
                else
                    this.ActiveDecorators.Add(dec);
            }
            else if (ext is GlowNotifier) {
                GlowNotifier notf = (GlowNotifier)ext;
                isActive = CheckForActiveNotf(notf.id);
                if (isActive)
                    RemoveNotf(notf.id);
                else
                    this.ActiveNotifiers.Add(notf);
            }
            return !isActive;
        }

        public bool GetExtSettingsWin(Guid id)
        {
            GlowExtension ext = this.lib.findExt(id);
            if (ext == null)
                throw new Exception("TODO make this a custom exception and catch it to open the default window");
            return ext.Settings();
        }

        private bool CheckForActiveDec(Guid id)
        {
            foreach (GlowDecorator dec in this.ActiveDecorators)
                if (dec.id.Equals(id))
                    return true;
            return false;
        }

        private void RemoveDec(Guid id)
        {
            GlowDecorator holder = null;
            foreach (GlowDecorator dec in this.ActiveDecorators)
                if (dec.id.Equals(id))
                    holder = dec;
            if (holder != null)
                this.ActiveDecorators.Remove(holder);
        }

        private bool CheckForActiveNotf(Guid id)
        {
            foreach (GlowNotifier notf in this.ActiveNotifiers)
                if (notf.id.Equals(id))
                    return true;
            return false;
        }

        private void RemoveNotf(Guid id)
        {
            GlowNotifier holder = null;
            foreach (GlowNotifier notf in this.ActiveNotifiers)
                if (notf.id.Equals(id))
                    holder = notf;
            if (holder != null)
                this.ActiveNotifiers.Remove(holder);
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
        public void AttachColorObserver(AntumbraColorObserver observer)
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
            this.ActiveDriver.AttachColorObserver(this);
            if (this.ActiveDriver is Loggable) {
                Loggable log = (Loggable)this.ActiveDriver;
                log.AttachLogObserver(this);
            }
            if (this.ActiveDriver is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)this.ActiveDriver;
                sender.AttachGlowCommandObserver(this);
            }
            if (this.ActiveDriver is ToolbarNotificationSource) {
                ToolbarNotificationSource src = (ToolbarNotificationSource)this.ActiveDriver;
                src.AttachToolbarNotifObserver(this);
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
                ActiveDriver = new GlowScreenDriverCoupler(ActiveGrabber, ActiveProcessor);
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
