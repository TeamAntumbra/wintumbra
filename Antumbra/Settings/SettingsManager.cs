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
    public class SettingsManager : Loggable, ConnectionEventObserver
    {
        public delegate void NewLogMsg(String source, String msg);
        public event NewLogMsg NewLogMsgAvail;
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
            foreach (DeviceSettings settings in Settings.Values) {
                settings.boundX = boundX;
                settings.boundY = boundY;
                settings.boundWidth = boundWidth;
                settings.boundHeight = boundHeight;
            }
        }

        /// <summary>
        /// Handle ConnectionUpdateEvent
        /// </summary>
        /// <param name="deviceCount"></param>
        public void ConnectionUpdate(int deviceCount)
        {
            Settings.Clear();
            for (var i = 0; i < deviceCount; i += 1) {
                if (!Settings.ContainsKey(i)) {
                    Settings[i] = new DeviceSettings(i);
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
            Settings[id] = (DeviceSettings)saver.Load(DeviceSettings.FILE_NAME_PREFIX + id);
        }
    }
}
