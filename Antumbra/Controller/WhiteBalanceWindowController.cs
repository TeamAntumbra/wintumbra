using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Antumbra.Glow.View;
using Antumbra.Glow.Settings;
using Antumbra.Glow.Observer.Connection;

namespace Antumbra.Glow.Controller
{
    public class WhiteBalanceWindowController : ConnectionEventObserver
    {
        private Dictionary<int, WhiteBalanceWindow> views;
        private SettingsManager settingsManager;
        private Color control;
        public WhiteBalanceWindowController(SettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            views = new Dictionary<int,WhiteBalanceWindow>();
            control = new Utility.HslColor(0, 0, .5).ToRgbColor();
        }

        public void ConnectionUpdate(int devCount)
        {
            DisposeAll();
            for (int i = 0; i < devCount; i += 1) {
                Init(i);
            }
        }

        private void DisposeAll()
        {
            foreach(WhiteBalanceWindow win in views.Values) {
                win.Close();
                win.Dispose();
            }
            views.Clear();
        }

        private void Init(int id)
        {
            WhiteBalanceWindow view = new WhiteBalanceWindow(id);
            view.ColorWheelChangedEvent += new WhiteBalanceWindow.ColorWheelChanged(ColorWheelChangedHandler);
            view.closeBtn_ClickEvent += new EventHandler(closeBtnHandler);
            DeviceSettings settings = settingsManager.getSettings(id);
            Color newColor = Color.FromArgb(control.R - settings.redBias, control.G - settings.greenBias, control.B - settings.blueBias);
            view.SetColor(newColor);
            views[id] = view;
        }

        private void ColorWheelChangedHandler(Color newColor, int id)
        {
            SettingsDelta Delta = new SettingsDelta();
            Delta.changes[SettingValue.REDBIAS] = Convert.ToInt16(control.R - newColor.R);
            Delta.changes[SettingValue.GREENBIAS] = Convert.ToInt16(control.G - newColor.G);
            Delta.changes[SettingValue.BLUEBIAS] = Convert.ToInt16(control.B - newColor.B);
            settingsManager.getSettings(id).ApplyChanges(Delta);
        }

        private void closeBtnHandler(object sender, EventArgs args)
        {
            foreach (WhiteBalanceWindow view in views.Values) {
                view.Close();
            }
        }

        public void Show()
        {
            foreach (WhiteBalanceWindow view in views.Values) {
                view.Show();
            }
        }
    }
}
