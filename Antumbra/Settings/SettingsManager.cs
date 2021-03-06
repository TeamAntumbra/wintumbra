﻿using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.Saving;
using System;
using System.Collections.Generic;

namespace Antumbra.Glow.Settings {

    /// <summary>
    /// Manages DeviceSettings objects and the bounding of all capture zones.
    /// </summary>
    public class SettingsManager : Loggable, ConnectionEventObserver, ConfigurationObserver, Configurable,
                                   ConfigurationChanger {

        #region Private Fields

        private int boundHeight;

        private int boundWidth;

        private Dictionary<int, DeviceSettings> Settings;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsManager() {
            AttachObserver(LoggerHelper.GetInstance());
            Settings = new Dictionary<int, DeviceSettings>();
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void NewConfigUpdate(Configurable config);

        public delegate void NewLogMsg(String source, String msg);

        #endregion Public Delegates

        #region Public Events

        public event NewConfigUpdate NewConfigUpdateAvail;

        public event NewLogMsg NewLogMsgAvail;

        #endregion Public Events

        #region Public Properties

        public int boundX { get; private set; }
        public int boundY { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Attach a ConfigurationObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(ConfigurationObserver observer) {
            NewConfigUpdateAvail += observer.ConfigurationUpdate;
        }

        /// <summary>
        /// Attach a LogMsgObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvail += observer.NewLogMsgAvail;
        }

        public void ConfigChange(SettingsDelta Delta) {
            getSettings(Delta.id).ConfigChange(Delta);
        }

        /// <summary>
        /// Handle a ConfigurationUpdate
        /// </summary>
        /// <param name="config"></param>
        public void ConfigurationUpdate(Configurable config) {
            if(NewConfigUpdateAvail != null) {
                NewConfigUpdateAvail(config);
            }
        }

        /// <summary>
        /// Handle ConnectionUpdateEvent
        /// </summary>
        /// <param name="deviceCount"></param>
        public void ConnectionUpdate(int deviceCount) {
            Save(-1);
            Settings.Clear();
            for(var i = 0; i < deviceCount; i += 1) {
                DeviceSettings DeviceSettings = new DeviceSettings(i);
                DeviceSettings.Load();
                DeviceSettings.AttachObserver((ConfigurationObserver)this);
                if(NewConfigUpdateAvail != null) {
                    NewConfigUpdateAvail(DeviceSettings);
                }
                Settings[i] = DeviceSettings;
            }
        }

        /// <summary>
        /// Get a specific device's DeviceSettings object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeviceSettings getSettings(int id) {
            return Settings[id];
        }

        /// <summary>
        /// Save DeviceSettings objects
        /// </summary>
        /// <param name="id">id of device or -1 for all</param>
        public void Save(int id) {
            Saver saver = Saver.GetInstance();
            if(id == -1) {
                foreach(DeviceSettings devSettings in Settings.Values) {
                    devSettings.Save();
                }
            } else {
                Settings[id].Save();
            }
        }

        /// <summary>
        /// Update the bounding box which contains all device's mirroring zones
        /// </summary>
        public void UpdateBoundingBox() {
            int minTop = int.MaxValue, maxBot = int.MinValue, minLeft = int.MaxValue, maxRight = int.MinValue;
            foreach(int id in Settings.Keys) {
                DeviceSettings settings = Settings[id];
                int left = settings.x;
                int right = settings.width + left;
                int top = settings.y;
                int bot = top + settings.height;
                minTop = minTop < top ? minTop : top;
                maxBot = maxBot > bot ? maxBot : bot;
                minLeft = minLeft < left ? minLeft : left;
                maxRight = maxRight > right ? maxRight : right;
            }
            boundX = minLeft;
            boundY = minTop;
            boundWidth = maxRight - minLeft;
            boundHeight = maxBot - minTop;
            foreach(int id in Settings.Keys) {
                SettingsDelta Delta = new SettingsDelta(id);
                Delta.changes[SettingValue.BOUNDX] = boundX;
                Delta.changes[SettingValue.BOUNDY] = boundY;
                Delta.changes[SettingValue.BOUNDWIDTH] = boundWidth;
                Delta.changes[SettingValue.BOUNDHEIGHT] = boundHeight;
                ConfigChange(Delta);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Log(string msg) {
            if(NewLogMsgAvail != null) {
                NewLogMsgAvail("Settings Manager", msg);
            }
        }

        #endregion Private Methods
    }
}
