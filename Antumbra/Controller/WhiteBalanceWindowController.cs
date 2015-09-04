using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.View;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Observer.Connection;

namespace Antumbra.Glow.Controller {
    public class WhiteBalanceWindowController : ConnectionEventObserver {
        private Dictionary<int, WhiteBalanceWindow> views;
        private SettingsManager settingsManager;
        private Color control;
        public WhiteBalanceWindowController(SettingsManager settingsManager) {
            this.settingsManager = settingsManager;
            views = new Dictionary<int, WhiteBalanceWindow>();
            control = new Utility.HslColor(0, 0, .5).ToRgbColor();
        }

        public bool IsOpen() {
            foreach(WhiteBalanceWindow window in views.Values) {
                if(window.Visible) {
                    return true;
                }
            }
            return false;
        }

        public void ConnectionUpdate(int devCount) {
            DisposeAll();
            for(int i = 0; i < devCount; i += 1) {
                Init(i);
            }
        }

        private void DisposeAll() {
            foreach(WhiteBalanceWindow win in views.Values) {
                win.Close();
                win.Dispose();
            }
            views.Clear();
        }

        private void Init(int id) {
            WhiteBalanceWindow view = new WhiteBalanceWindow(id);
            view.ColorWheelChangedEvent += new WhiteBalanceWindow.ColorWheelChanged(ColorWheelChangedHandler);
            view.closeBtn_ClickEvent += new EventHandler(closeBtnHandler);
            DeviceSettings settings = settingsManager.getSettings(id);
            int r = control.R - settings.redBias;
            r = r < 0 ? 0 : r;
            int g = control.G - settings.greenBias;
            g = g < 0 ? 0 : g;
            int b = control.B - settings.blueBias;
            b = b < 0 ? 0 : b;
            Color newColor = Color.FromArgb(r, g, b);
            view.SetColor(newColor);
            views[id] = view;
        }

        private void ColorWheelChangedHandler(Color newColor, int id) {
            SettingsDelta Delta = new SettingsDelta();
            int red = newColor.R - control.R;
            int green = newColor.G - control.G;
            int blue = newColor.B - control.B;
            if(red > 0) {
                red = red >= 128 ? 127 : red;
                Delta.changes[SettingValue.REDBIAS] = red;
            }
            if(green > 0) {
                green = green >= 128 ? 127 : green;
                Delta.changes[SettingValue.GREENBIAS] = green;
            }
            if(blue > 0) {
                blue = blue >= 128 ? 127 : blue;
                Delta.changes[SettingValue.BLUEBIAS] = blue;
            }
            settingsManager.getSettings(id).ApplyChanges(Delta);
        }

        private void closeBtnHandler(object sender, EventArgs args) {
            foreach(WhiteBalanceWindow view in views.Values) {
                view.Close();
            }
        }

        public void Show() {
            foreach(WhiteBalanceWindow view in views.Values) {
                view.Show();
            }
        }
    }
}
