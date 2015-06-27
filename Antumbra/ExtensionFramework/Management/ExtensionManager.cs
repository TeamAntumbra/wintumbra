﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.Colors;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Utility.Settings;
using Antumbra.Glow.ExtensionFramework.Types;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    /// <summary>
    /// Manages the Extensions for use with a Glow device
    /// </summary>
    public class ExtensionManager : AntumbraColorObserver, LogMsgObserver, Loggable,
                                    ToolbarNotificationObserver, ToolbarNotificationSource,
                                    GlowCommandObserver, GlowCommandSender, Savable,
                                    ConfigurationObserver//TODO add observer for notifiers
    {
        /// <summary>
        /// Delegate for NewColorAvailEvent, handles a new Color being available
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="args"></param>
        public delegate void NewColorAvail(Color16Bit newColor);
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
        /// The id for the GlowDevice relating to this ExtensionManager
        /// </summary>
        public int id { get; private set; }
        public ActiveExtensions activeExts { get; private set; }
        private ExtensionLibrary lib;
        private bool compoundDecoration;
        private int stepSleep, x, y, width, height, redBias, greenBias, blueBias;//local copies of just these rather than entire DeviceSettings obj
        private UInt16 maxBrightness;
        public const String configFileBase = "ActiveExtsDev_";
        private int captureThrottle;
        /// <summary>
        /// Constructor - Creates a new ExtensionManager relating to the GlowDevice
        /// with the same id as passed.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="settings"></param>
        public ExtensionManager(ExtensionLibrary extLib, int id)
        {
            this.lib = extLib;
            this.id = id;
            this.activeExts = new ActiveExtensions();
        }

        public void LoadActives(ActiveExtensions actives)
        {
            this.activeExts = actives;
        }

        public void Save()
        {
            if (this.activeExts != null) {
                Saver saver = Saver.GetInstance();
                saver.Save(ExtensionManager.configFileBase + this.id, this.activeExts.ToString());
            }
        }

        public void LoadSave(String settings)
        {
            this.Stop();
            if (this.activeExts == null) {
                this.activeExts = new ActiveExtensions();
            }
            try {
                String[] parts = settings.Split(',');
                this.activeExts.ActiveDriver = (GlowDriver)this.lib.findExt(Guid.Parse(parts[0]));
                this.activeExts.ActiveGrabber = (GlowScreenGrabber)this.lib.findExt(Guid.Parse(parts[1]));
                this.activeExts.ActiveProcessor = (GlowScreenProcessor)this.lib.findExt(Guid.Parse(parts[2]));
                this.activeExts.ActiveDecorators.Clear();
                foreach (String dec in parts[3].Split(' ')) {
                    if (dec.Equals(""))
                        break;
                    this.activeExts.ActiveDecorators.Add((GlowDecorator)this.lib.findExt(Guid.Parse(dec)));
                }
                this.activeExts.ActiveNotifiers.Clear();
                foreach (String notf in parts[4].Split(' ')) {
                    if (notf.Equals(""))
                        break;
                    this.activeExts.ActiveNotifiers.Add((GlowNotifier)this.lib.findExt(Guid.Parse(notf)));
                }
            }
            catch(Exception e) {
                this.NewLogMsgAvail("Ext Mgr", "Loading settings failed!" + e.StackTrace + '\n' + e.Message);
            }
        }

        void AntumbraColorObserver.NewColorAvail(Color16Bit newColor)
        {
            if (NewColorAvailEvent != null) {
                NewColorAvailEvent(newColor);
            }
        }

        public void Reset()
        {
            this.Stop();
            this.activeExts = this.lib.GetDefaults();
        }

        public void ConfigurationUpdate(Configurable config)
        {
            if (config is DeviceSettings) {//update local settings
                DeviceSettings settings = (DeviceSettings)config;
                this.compoundDecoration = settings.compoundDecoration;
                this.x = settings.x;
                this.y = settings.y;
                this.width = settings.width;
                this.height = settings.height;
                this.stepSleep = settings.stepSleep;
                this.maxBrightness = settings.maxBrightness;
                this.redBias = settings.redBias;
                this.greenBias = settings.greenBias;
                this.blueBias = settings.blueBias;
                this.captureThrottle = settings.captureThrottle;
            }
            //ignore ActiveExtensions events, already knows about it
        }

        public void UpdateExtension(Guid id)
        {
            GlowExtension ext = this.lib.findExt(id);
            if (ext == null)
                throw new Exception("Invalid Guid found while updating extension");
            if (ext is GlowDriver)
                this.activeExts.ActiveDriver = (GlowDriver)ext;
            else if (ext is GlowScreenGrabber)
                this.activeExts.ActiveGrabber = (GlowScreenGrabber)ext;
            else if (ext is GlowScreenProcessor)
                this.activeExts.ActiveProcessor = (GlowScreenProcessor)ext;
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
                    this.activeExts.ActiveDecorators.Add(dec);
            }
            else if (ext is GlowNotifier) {
                GlowNotifier notf = (GlowNotifier)ext;
                isActive = CheckForActiveNotf(notf.id);
                if (isActive)
                    RemoveNotf(notf.id);
                else
                    this.activeExts.ActiveNotifiers.Add(notf);
            }
            return !isActive;
        }

        public bool GetExtSettingsWin(Guid id)
        {
            GlowExtension ext = this.lib.findExt(id);
            if (ext == null)
                throw new Exception("TODO make this a custom exception for extension not found");
            return ext.Settings();
        }

        private bool CheckForActiveDec(Guid id)
        {
            foreach (GlowDecorator dec in this.activeExts.ActiveDecorators)
                if (dec.id.Equals(id))
                    return true;
            return false;
        }

        private void RemoveDec(Guid id)
        {
            GlowDecorator holder = null;
            foreach (GlowDecorator dec in this.activeExts.ActiveDecorators)
                if (dec.id.Equals(id))
                    holder = dec;
            if (holder != null)
                this.activeExts.ActiveDecorators.Remove(holder);
        }

        private bool CheckForActiveNotf(Guid id)
        {
            foreach (GlowNotifier notf in this.activeExts.ActiveNotifiers)
                if (notf.id.Equals(id))
                    return true;
            return false;
        }

        private void RemoveNotf(Guid id)
        {
            GlowNotifier holder = null;
            foreach (GlowNotifier notf in this.activeExts.ActiveNotifiers)
                if (notf.id.Equals(id))
                    holder = notf;
            if (holder != null)
                this.activeExts.ActiveNotifiers.Remove(holder);
        }
        /// <summary>
        /// Return a string describing the current active extensions
        /// </summary>
        /// <returns></returns>
        public string GetSetupDesc()
        {
            if (this.activeExts.ActiveDriver == null)
                return "The current driver is null. Extension loading probably failed.";
            string result = "Driver: ";
            if (this.activeExts.ActiveDriver is GlowScreenDriverCoupler) {
                if (this.activeExts.ActiveGrabber != null && this.activeExts.ActiveProcessor != null)
                    result += this.activeExts.ActiveDriver.ToString()
                        + "\nGrabber: " + this.activeExts.ActiveGrabber.ToString() + "\nProcessor: "
                        + this.activeExts.ActiveProcessor.ToString();
                else
                    result += this.activeExts.ActiveDriver.ToString();
            }
            else
                result += this.activeExts.ActiveDriver.ToString();
            result += "\nDecorators: ";
            foreach (var dec in this.activeExts.ActiveDecorators)
                result += dec.ToString() + ' ';
            result += "\nNotifiers: ";
            foreach (var notf in this.activeExts.ActiveNotifiers)
                result += notf.ToString() + ' ';
            return result;
        }
        /// <summary>
        /// Attach an AntumbraColorObserver to the NewColorAvailEvent for this ExtensionManager
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(AntumbraColorObserver observer)
        {
            NewColorAvailEvent += observer.NewColorAvail;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            NewGlowCommandEvent += observer.NewGlowCommandAvail;
        }

        public void RegisterDevice(int id)
        {
            //ignore, already have id
        }

        public void NewToolbarNotifAvail(int time, String title, String msg, int icon)
        {
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);
        }

        public void NewLogMsgAvail(String source, String msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent(source, msg);
        }

        public void NewGlowCommandAvail(GlowCommand command)
        {
            if (NewGlowCommandEvent != null)
                NewGlowCommandEvent(command);//pass it up
        }

        public Color16Bit ApplyBrightnessSettings(Color16Bit decorated)
        {
            UInt16 red = Convert.ToUInt16(((double)decorated.red / UInt16.MaxValue) * this.maxBrightness);
            UInt16 green = Convert.ToUInt16(((double)decorated.green / UInt16.MaxValue) * this.maxBrightness);
            UInt16 blue = Convert.ToUInt16(((double)decorated.blue / UInt16.MaxValue) * this.maxBrightness);
            return new Color16Bit(red, green, blue);
        }

        public Color16Bit ApplyDecorations(Color16Bit orig)
        {
            List<GlowDecorator> decs = this.activeExts.ActiveDecorators;
            int count = decs.Count;
            if (count == 0) {
                return orig;
            }
            if (this.compoundDecoration) {
                foreach (var dec in decs)//decorate
                    orig = dec.Decorate(orig);
                return orig;
            }
            //average decorators output
            int r = 0, g = 0, b = 0;
            foreach (var dec in decs) {
                Color16Bit decorated = dec.Decorate(orig);
                r += decorated.red;
                g += decorated.green;
                b += decorated.blue;
            }
            UInt16 red = Convert.ToUInt16(r / count);
            UInt16 green = Convert.ToUInt16(g / count);
            UInt16 blue = Convert.ToUInt16(b / count);
            return new Color16Bit(red, green, blue);
        }

        public Color16Bit ApplyWhiteBalance(Color16Bit orig)
        {
            int red = orig.red >> 8;
            int green = orig.green >> 8;
            int blue = orig.blue >> 8;
            red += redBias;
            green += greenBias;
            blue += blueBias;
            if (red > 255)
                red = 255;
            else
                if (red < 0)
                    red = 0;
            if (green > 255)
                green = 255;
            else
                if (green < 0)
                    green = 0;
            if (blue > 255)
                blue = 255;
            else
                if (blue < 0)
                    blue = 0;
            return new Color16Bit(System.Drawing.Color.FromArgb(red,green,blue));
        }

        /// <summary>
        /// Start active extensions
        /// </summary>
        /// <returns>True if successful, else false</returns>
        public bool Start()
        {
            if (!Verify())
                return false;
            GlowDriver activeDriver = this.activeExts.ActiveDriver;
            this.activeExts.ActiveDriver.AttachColorObserver(this);
            if (this.activeExts.ActiveDriver is Loggable) {
                Loggable log = (Loggable)this.activeExts.ActiveDriver;
                log.AttachObserver(this);
            }
            if (this.activeExts.ActiveDriver is GlowCommandSender) {
                GlowCommandSender sender = (GlowCommandSender)this.activeExts.ActiveDriver;
                sender.AttachObserver(this);
            }
            if (this.activeExts.ActiveDriver is ToolbarNotificationSource) {
                ToolbarNotificationSource src = (ToolbarNotificationSource)this.activeExts.ActiveDriver;
                src.AttachObserver(this);
            }
            if (!this.activeExts.ActiveDriver.Start())
                return false;
            foreach (var dec in this.activeExts.ActiveDecorators)
                if (!dec.Start())
                    return false;
            foreach (var notf in this.activeExts.ActiveNotifiers)
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
            if (this.activeExts.ActiveDriver == null)
                return false;
            if (this.activeExts.ActiveDriver is GlowScreenDriverCoupler) {//screen based driver selected
                if (null == this.activeExts.ActiveGrabber || this.activeExts.ActiveProcessor == null) {//no grabber or processor set
                    return false;
                }
                this.activeExts.ActiveGrabber.x = this.x;//set screen related settings for grabber
                this.activeExts.ActiveGrabber.y = this.y;
                this.activeExts.ActiveGrabber.width = this.width;
                this.activeExts.ActiveGrabber.height = this.height;
                this.activeExts.ActiveGrabber.captureThrottle = this.captureThrottle;
                this.activeExts.ActiveDriver = new GlowScreenDriverCoupler(this.activeExts.ActiveGrabber, this.activeExts.ActiveProcessor);
            }
            this.activeExts.ActiveDriver.stepSleep = this.stepSleep;
            return true;
        }
        /// <summary>
        /// Stop the active extensions
        /// </summary>
        /// <returns>True if successful, else false</returns>
        public bool Stop()
        {
            bool result = true;
            if (this.activeExts != null) {
                if (this.activeExts.ActiveDriver == null)
                    return false;
                if (!this.activeExts.ActiveDriver.Stop())//coupler will stop grabber & processor
                    result = false;
                foreach (var dec in this.activeExts.ActiveDecorators)
                    if (!dec.Stop())
                        result = false;
                foreach (var notf in this.activeExts.ActiveNotifiers)
                    if (!notf.Stop())
                        result = false;
            }
            return result;
        }
    }
}
