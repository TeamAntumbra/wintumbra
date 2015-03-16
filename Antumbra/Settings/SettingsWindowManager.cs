using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Observer.Extensions;
using Antumbra.Glow.Observer.GlowCommands;
using Antumbra.Glow.Observer.ToolbarNotifications;
using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.ExtensionFramework;

namespace Antumbra.Glow.Settings
{
    public class SettingsWindowManager : GlowExtCollectionObserver, GlowCommandObserver, ToolbarNotificationObserver,
                                         ToolbarNotificationSource, GlowCommandSender
    {
        public delegate void NewToolbarNotif(int time, String title, String msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommand(GlowCommand cmd);
        public event NewGlowCommand NewGlowCommandAvailEvent;
        private List<SettingsWindowController> controllers;
        private String productVersion;
        private BasicExtSettingsWinFactory basicWinFactory;
        private ExtensionLibrary lib;
        public SettingsWindowManager(String productVersion, ExtensionLibrary lib)
        {
            this.lib = lib;
            this.lib.AttachObserver(this);//register for library updates
            this.basicWinFactory = new BasicExtSettingsWinFactory(this.lib);
            this.productVersion = productVersion;
            this.controllers = new List<SettingsWindowController>();
        }

        public void LibraryUpdate(List<GlowExtension> exts)
        {

        }

        public SettingsWindowController CreateAndAddNewController(GlowDevice dev)
        {
            SettingsWindowController found = FindController(dev.id);
            if (found != null)
                return found;//no need to make a new one
            SettingsWindowController cont = new SettingsWindowController(dev, this.productVersion, this.basicWinFactory);
            this.lib.AttachObserver(cont);
            this.lib.NotifyObservers();//force inital update for this controller
            cont.AttachObserver((ToolbarNotificationObserver)this);
            cont.AttachObserver((GlowCommandObserver)this);
            dev.AttachObserver(cont);
            dev.Notify();//force inital update
            this.controllers.Add(cont);
            return cont;
        }

        public void NewGlowCommandAvail(GlowCommand cmd)
        {

        }

        public void AttachObserver(GlowCommandObserver observer)
        {
            this.NewGlowCommandAvailEvent += observer.NewGlowCommandAvail;
        }

        public void RegisterDevice(int id)
        {
            //ignore, simply uses this interface to pass through events to toolbar controller
        }

        public void NewToolbarNotifAvail(int time, String title, String msg, int icon)
        {

        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public bool Show(int id)
        {
            SettingsWindowController cont = FindController(id);
            if (cont != null)
                cont.Show();
            return false;
        }

        private SettingsWindowController FindController(int id)
        {
            foreach (SettingsWindowController cont in controllers)
                if (cont.id.Equals(id))
                    return cont;
            return null;
        }
    }
}
