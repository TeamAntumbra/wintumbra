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
using Antumbra.Glow.Controller;
using Antumbra.Glow.View;

namespace Antumbra.Glow.Settings
{
    public class AdvancedSettingsWindowManager : GlowCommandObserver, ToolbarNotificationObserver,
                                         ToolbarNotificationSource, GlowCommandSender
    {
        public delegate void NewToolbarNotif(int time, String title, String msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewGlowCommand(GlowCommand cmd);
        public event NewGlowCommand NewGlowCommandAvailEvent;
        private List<AdvancedSettingsWindowController> controllers;
        private String productVersion;
        private AntumbraExtSettingsWindow.ExtWindowFactory basicWinFactory;
        private ExtensionLibrary lib;
        public AdvancedSettingsWindowManager(String productVersion, ExtensionLibrary lib)
        {
            this.lib = lib;
            this.basicWinFactory = new AntumbraExtSettingsWindow.ExtWindowFactory(this.lib);
            this.productVersion = productVersion;
            this.controllers = new List<AdvancedSettingsWindowController>();
        }

        public AdvancedSettingsWindowController CreateAndAddNewController(GlowDevice dev)
        {
            AdvancedSettingsWindowController found = FindController(dev.id);
            if (found != null)
                return found;//no need to make a new one
            AdvancedSettingsWindowController cont = new AdvancedSettingsWindowController(dev, this.productVersion, this.basicWinFactory);
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
            if (NewGlowCommandAvailEvent != null)
                NewGlowCommandAvailEvent(cmd);//pass up
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
            if (NewToolbarNotifAvailEvent != null)
                NewToolbarNotifAvailEvent(time, title, msg, icon);//pass up
        }

        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        public bool Show(int id)
        {
            AdvancedSettingsWindowController cont = FindController(id);
            if (cont != null) {
                cont.Show();
                return true;
            }
            return false;
        }

        private AdvancedSettingsWindowController FindController(int id)
        {
            foreach (AdvancedSettingsWindowController cont in controllers)
                if (cont.id.Equals(id))
                    return cont;
            return null;
        }

        public void CleanUp()
        {
            foreach (AdvancedSettingsWindowController cont in this.controllers)
                cont.CleanUp();
        }
    }
}
