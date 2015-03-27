using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.View;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Observer.Configuration;

namespace Antumbra.Glow.Controller
{
    public class WhiteBalanceWindowController : ConfigurationObserver
    {
        private WhiteBalanceWindow view;
        private List<GlowDevice> devices;
        public WhiteBalanceWindowController(GlowDevice dev)
        {
            this.devices = new List<GlowDevice>();
            this.devices.Add(dev);
            dev.AttachObserver(this);
        }

        public WhiteBalanceWindowController(List<GlowDevice> devs)
        {
            this.devices = new List<GlowDevice>();
            foreach (var dev in devs) {
                this.devices.Add(dev);
                dev.AttachObserver(this);
            }
        }

        public void ConfigurationUpdate(Configurable config)
        {
            if (config is Settings.DeviceSettings && this.view != null) {
                Settings.DeviceSettings settings = (Settings.DeviceSettings)config;
                this.view.SetR(settings.redBias);
                this.view.SetG(settings.greenBias);
                this.view.SetB(settings.blueBias);
            }
        }

        private void Init()
        {
            this.view = new WhiteBalanceWindow();
            GlowDevice dev = this.devices.First<GlowDevice>();
            this.view.SetR(dev.settings.redBias);
            this.view.SetG(dev.settings.greenBias);
            this.view.SetB(dev.settings.blueBias);
            this.view.rDownBtn_ClickEvent += new EventHandler(redDownHandler);
            this.view.gDownBtn_ClickEvent += new EventHandler(greenDownHandler);
            this.view.bDownBtn_ClickEvent += new EventHandler(blueDownHandler);
            this.view.rUpBtn_ClickEvent += new EventHandler(redUpHandler);
            this.view.gUpBtn_ClickEvent += new EventHandler(greenUpHandler);
            this.view.bUpBtn_clickEvent += new EventHandler(blueUpHandler);
            this.view.closeBtn_ClickEvent += new EventHandler(closeBtnHandler);
        }

        private void closeBtnHandler(object sender, EventArgs args)
        {
            this.view.Close();
        }

        private void redDownHandler(object sender, EventArgs args)
        {
            foreach (GlowDevice dev in devices)
                dev.settings.redBias -= 1;
        }

        private void greenDownHandler(object sender, EventArgs args)
        {
            foreach (GlowDevice dev in devices)
                dev.settings.greenBias -= 1;
        }

        private void blueDownHandler(object sender, EventArgs args)
        {
            foreach (GlowDevice dev in devices)
                dev.settings.blueBias -= 1;
        }

        private void redUpHandler(object sender, EventArgs args)
        {
            foreach (GlowDevice dev in devices)
                dev.settings.redBias += 1;
        }

        private void greenUpHandler(object sender, EventArgs args)
        {
            foreach (GlowDevice dev in devices)
                dev.settings.greenBias += 1;
        }

        private void blueUpHandler(object sender, EventArgs args)
        {
            foreach (GlowDevice dev in devices)
                dev.settings.blueBias += 1;
        }

        public void Show()
        {
            if (this.view == null || this.view.IsDisposed)
                Init();
            this.view.Show();
        }
    }
}
