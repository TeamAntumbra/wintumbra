﻿using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.Saving;
using System;
using System.Collections.Generic;

namespace Antumbra.Glow.Settings
{
    /// <summary>
    /// Manages DeviceSettings objects and the bounding of all capture zones.
    /// </summary>
    public class SettingsManager : Loggable, ConnectionEventObserver, ConfigurationObserver, Configurable
    {
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvail;
        public delegate void NewConfigUpdate(Configurable config);
        public event NewConfigUpdate NewConfigUpdateAvail;
        private Dictionary<int, DeviceSettings> Settings;
        private int boundX, boundY, boundWidth, boundHeight;
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsManager()
        {
            AttachObserver(LoggerHelper.GetInstance());
            Settings = new Dictionary<int, DeviceSettings>();
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
        /// Update the bounding box which contains all device's mirroring zones
        /// </summary>
        public void UpdateBoundingBox()
        {
            int minTop = int.MaxValue, maxBot = 0, minLeft = int.MaxValue, maxRight = 0;
            foreach (int id in Settings.Keys) {
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
            SettingsDelta Delta = new SettingsDelta();
            Delta.changes[SettingValue.BOUNDX] = boundX;
            Delta.changes[SettingValue.BOUNDY] = boundY;
            Delta.changes[SettingValue.BOUNDWIDTH] = boundWidth;
            Delta.changes[SettingValue.BOUNDHEIGHT] = boundHeight;
            foreach (DeviceSettings settings in Settings.Values) {
                settings.ApplyChanges(Delta);
            }
        }

        /// <summary>
        /// Handle ConnectionUpdateEvent
        /// </summary>
        /// <param name="deviceCount"></param>
        public void ConnectionUpdate(int deviceCount)
        {
            SaveAll();
            Settings.Clear();
            for (var i = 0; i < deviceCount; i += 1) {
                Settings[i] = new DeviceSettings(i);
                Load(i);
                Settings[i].AttachObserver((ConfigurationObserver)this);
                if (NewConfigUpdateAvail != null) {
                    NewConfigUpdateAvail(Settings[i]);
                }
            }
        }


        /// <summary>
        /// Save all DeviceSettings objects
        /// </summary>
        public void SaveAll()
        {
            Saver saver = Saver.GetInstance();
            foreach (DeviceSettings settings in Settings.Values) {
                saver.Save(DeviceSettings.FILE_NAME_PREFIX + settings.id, settings);
            }
        }

        /// <summary>
        /// Handle a ConfigurationUpdate
        /// </summary>
        /// <param name="config"></param>
        public void ConfigurationUpdate(Configurable config)
        {
            if (NewConfigUpdateAvail != null) {
                NewConfigUpdateAvail(config);
            }
        }

        /// <summary>
        /// Attach a ConfigurationObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(ConfigurationObserver observer)
        {
            NewConfigUpdateAvail += observer.ConfigurationUpdate;
        }

        /// <summary>
        /// Attach a LogMsgObserver
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvail += observer.NewLogMsgAvail;
        }

        /// <summary>
        /// Load a DeviceSettings object
        /// </summary>
        /// <param name="id">The desired device ID</param>
        private void Load(int id)
        {
            Saver saver = Saver.GetInstance();
            DeviceSettings loaded = (DeviceSettings)saver.Load(DeviceSettings.FILE_NAME_PREFIX + id);
            if (loaded != null) {
                Settings[id] = loaded;
            }
        }

        private void Log(string msg)
        {
            if (NewLogMsgAvail != null) {
                NewLogMsgAvail("Settings Manager", msg);
            }
        }
    }
}
