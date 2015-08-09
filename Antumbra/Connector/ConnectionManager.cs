using Antumbra.Glow.ExtensionFramework.Management;
using Antumbra.Glow.Observer.Connection;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.ToolbarNotifications;
using System;
using System.Collections.Generic;

namespace Antumbra.Glow.Connector
{
    /// <summary>
    /// Manages dealing with connected Glow units via the SerialConnector class.
    /// </summary>
    public class ConnectionManager : ToolbarNotificationSource, Loggable, ConnectionEventSource, IDisposable
    {
        public delegate void NewToolbarNotif(int time, string title, string msg, int icon);
        public event NewToolbarNotif NewToolbarNotifAvailEvent;
        public delegate void NewLogMsgAvail(string title, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public delegate void NewConnectionEventAvail(int devCount);
        public event NewConnectionEventAvail NewConnectionEventAvailEvent;
        public int GlowsFound { get; private set; }

        private SerialConnector Connector;
        private List<GlowDevice> Glows;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        public ConnectionManager(int vid, int pid)
        {
            AttachObserver(LoggerHelper.GetInstance());
            Connector = new SerialConnector(vid, pid);
            Glows = new List<GlowDevice>();
            Update();
        }

        /// <summary>
        /// Update Glow device connections
        /// </summary>
        private void Update()
        {
            int len = this.Connector.UpdateDeviceList();
            for (int i = 0; i < len; i += 1) {
                GlowDevice device;
                device.info = Connector.GetDeviceInfo(i);
                device.id = i;
                int err;
                device.dev = Connector.OpenDevice(device.info, out err);
                device.status = -1;
                Log("Device " + i + " opened with response status " + err);
                Glows.Add(device);
            }
            GlowsFound = Glows.Count;
            if (NewConnectionEventAvailEvent != null) {
                NewConnectionEventAvailEvent(GlowsFound);
            }
        }

        /// <summary>
        /// Attach a ToolbarNotificationObserver to this object
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(ToolbarNotificationObserver observer)
        {
            this.NewToolbarNotifAvailEvent += observer.NewToolbarNotifAvail;
        }

        /// <summary>
        /// Attach a LogMsgObserver to this object
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        /// <summary>
        /// Attach a ConnectionEventObserver to this object
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(ConnectionEventObserver observer)
        {
            this.NewConnectionEventAvailEvent += observer.ConnectionUpdate;
        }

        /// <summary>
        /// Send a Color16Bit to a specified physical Glow device
        /// </summary>
        /// <param name="newColor"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int sendColor(Antumbra.Glow.Observer.Colors.Color16Bit newColor, int id)
        {
            if (id == -1) {
                int status = -1;
                for (var i = 0; i < GlowsFound; i += 1) {
                    GlowDevice device = Glows[i];
                    status = sendColor(newColor.red, newColor.green, newColor.blue, device.id);
                    device.status = status;
                    if (status != 0) {
                        break;
                    }
                }
                return status;
            }
            return sendColor(newColor.red, newColor.green, newColor.blue, id);
        }

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            if (this.NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("Device Manager", msg);
        }

        public void Dispose()
        {
            CloseAll();
            FreeList();
        }

        /// <summary>
        /// Send the specified UInt16 rgb values to the specified physical Glow device
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private int sendColor(UInt16 r, UInt16 g, UInt16 b, int id)
        {
            GlowDevice activeDev = Glows[id];
            int err;
            if (activeDev.dev == IntPtr.Zero) {//needs opening
                activeDev.dev = this.Connector.OpenDevice(activeDev.info, out err);
                if (err != 0) {//error occured
                    activeDev.status = err;
                    return err;
                }
            }
            int status = this.Connector.SetDeviceColor(activeDev.id, activeDev.dev, r, g, b);
            activeDev.status = status;
            return status;
        }

        /// <summary>
        /// Close all connections
        /// </summary>
        private void CloseAll()
        {
            foreach (var active in this.Glows) {
                IntPtr ptr = active.dev;
                if (!ptr.Equals(IntPtr.Zero))//actually open?
                    this.Connector.CloseDevice(active.dev);
            }
        }

        /// <summary>
        /// Free the connections list
        /// </summary>
        private void FreeList()
        {
            this.Connector.FreeList();
        }

        /// <summary>
        /// Represents a physical Antumbra|Glow unit.
        /// Holds the values needed to identify and send colors to it.
        /// </summary>
        private struct GlowDevice
        {
            /// <summary>
            /// Device pointer
            /// </summary>
            public IntPtr dev;
            /// <summary>
            /// Info pointer
            /// </summary>
            public IntPtr info;
            /// <summary>
            /// The id of this device as given by the manager upon creation
            /// </summary>
            public int id;
            /// <summary>
            /// Last know status of this device
            /// </summary>
            public int status; 
        }
    }
}
