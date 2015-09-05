using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.Settings;
using Antumbra.Glow.View;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Antumbra.Glow.Controller {

    public class WhiteBalanceWindowController : ConnectionEventObserver, ConfigurationChangeAnnouncer {

        #region Private Fields

        private System.Windows.Forms.FormClosingEventHandler closingHandler;

        private Color control;

        private SettingsManager settingsManager;

        private Dictionary<int, WhiteBalanceWindow> views;

        #endregion Private Fields

        #region Public Constructors

        public WhiteBalanceWindowController(SettingsManager settingsManager,
                                            System.Windows.Forms.FormClosingEventHandler closingHandler) {
            this.closingHandler = closingHandler;
            this.settingsManager = settingsManager;
            views = new Dictionary<int, WhiteBalanceWindow>();
            control = new Utility.HslColor(0, 0, .5).ToRgbColor();
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewConfigChange(SettingsDelta Delta);

        #endregion Public Delegates

        #region Public Events

        public event NewConfigChange NewConfigChangeEvent;

        #endregion Public Events

        #region Public Methods

        public void AttachObserver(ConfigurationChanger observer) {
            NewConfigChangeEvent += observer.ConfigChange;
        }

        public void ConnectionUpdate(int devCount) {
            DisposeAll();
            for(int i = 0; i < devCount; i += 1) {
                Init(i, closingHandler);
            }
        }

        public bool IsOpen() {
            foreach(WhiteBalanceWindow window in views.Values) {
                if(!window.IsDisposed) {
                    return true;
                }
            }
            return false;
        }

        public void Show() {
            foreach(KeyValuePair<int, WhiteBalanceWindow> viewPair in views) {
                if(viewPair.Value.IsDisposed) {
                    Init(viewPair.Key, closingHandler);
                    views[viewPair.Key].Show();
                } else {
                    viewPair.Value.Show();
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void AnnounceConfigChange(SettingsDelta Delta) {
            if(NewConfigChangeEvent != null) {
                NewConfigChangeEvent(Delta);
            }
        }

        private void CloseBtnClicked(object sender, EventArgs args) {
            if(sender != null && sender is int) {
                views[(int)sender].Close();
            }
        }

        private void ColorWheelChangedHandler(Color newColor, int id) {
            SettingsDelta Delta = new SettingsDelta(id);
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
            AnnounceConfigChange(Delta);
        }

        private void DisposeAll() {
            foreach(WhiteBalanceWindow win in views.Values) {
                win.Close();
                win.Dispose();
            }
            views.Clear();
        }

        private void Init(int id, System.Windows.Forms.FormClosingEventHandler closingHandler) {
            WhiteBalanceWindow view = new WhiteBalanceWindow(id);
            view.FormClosing += closingHandler;
            view.closeBtn_ClickEvent += new EventHandler(CloseBtnClicked);
            view.ColorWheelChangedEvent += new WhiteBalanceWindow.ColorWheelChanged(ColorWheelChangedHandler);
            DeviceSettings settings = settingsManager.getSettings(id);
            int r = control.R + settings.redBias;
            r = r > Byte.MaxValue ? Byte.MaxValue : r;
            int g = control.G + settings.greenBias;
            g = g > Byte.MaxValue ? Byte.MaxValue : g;
            int b = control.B + settings.blueBias;
            b = b > Byte.MaxValue ? Byte.MaxValue : b;
            Color newColor = Color.FromArgb(r, g, b);
            view.SetColor(newColor);
            views[id] = view;
        }

        #endregion Private Methods
    }
}
