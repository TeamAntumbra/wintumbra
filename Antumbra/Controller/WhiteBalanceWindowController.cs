using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.View;
using Antumbra.Glow.Connector;
using Antumbra.Glow.Observer.Configuration;

namespace Antumbra.Glow.Controller
{
    public class WhiteBalanceWindowController : ConfigurationObserver
    {
        private WhiteBalanceWindow view;
        private List<GlowDevice> devices;
        private Color control;
        public WhiteBalanceWindowController(GlowDevice dev)
        {
            this.devices = new List<GlowDevice>();
            this.devices.Add(dev);
            dev.AttachObserver(this);
            this.control = new Utility.HslColor(0, 0, .5).ToRgbColor();
        }

        public WhiteBalanceWindowController(List<GlowDevice> devs)
        {
            this.devices = new List<GlowDevice>();
            foreach (var dev in devs) {
                this.devices.Add(dev);
                dev.AttachObserver(this);
            }
            this.control = new Utility.HslColor(0, 0, .5).ToRgbColor();
        }

        public void ConfigurationUpdate(Configurable config)
        {
            if (config is Settings.DeviceSettings && this.view != null) {
                Settings.DeviceSettings settings = (Settings.DeviceSettings)config;
                SetColorFromBias(settings.redBias, settings.greenBias, settings.blueBias);
            }
        }

        private void SetColorFromBias(int redBias, int greenBias, int blueBias)
        {
            Color newColor = Color.FromArgb(control.R - redBias, control.G - greenBias, control.B - blueBias);
            this.view.SetColor(newColor);
        }

        private void Init()
        {
            this.view = new WhiteBalanceWindow();
            GlowDevice dev = this.devices.First<GlowDevice>();
            SetColorFromBias(dev.settings.redBias, dev.settings.greenBias, dev.settings.blueBias);
            this.view.ColorWheelChangedEvent += new WhiteBalanceWindow.ColorWheelChanged(ColorWheelChangedHandler);
            this.view.closeBtn_ClickEvent += new EventHandler(closeBtnHandler);
        }

        private void ColorWheelChangedHandler(Color newColor)
        {
            foreach (GlowDevice dev in devices) {
                
                dev.settings.redBias = control.R - newColor.R;
                dev.settings.greenBias = control.G - newColor.G;
                dev.settings.blueBias = control.B - newColor.B;
            }
        }

        private void closeBtnHandler(object sender, EventArgs args)
        {
            this.view.Close();
        }

        public void Show()
        {
            if (this.view == null || this.view.IsDisposed)
                Init();
            this.view.Show();
        }
    }
}
