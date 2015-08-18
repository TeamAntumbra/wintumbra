using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Logging;
using System;
using System.Runtime.Serialization;

namespace Antumbra.Glow.Settings
{
    /// <summary>
    /// Holds a Glow device's settings
    /// Note: This class is Savable despite it not implementing the Savable interface
    ///       The caveat here is that it cannot save itself
    /// </summary>
    [Serializable()]
    public class DeviceSettings : Configurable, Loggable, ISerializable
    {
        public const String FILE_NAME_PREFIX = "Dev_Settings_";

        public delegate void ConfigurationChange(Configurable settings);
        public event ConfigurationChange ConfigChangeEvent;
        public delegate void NewLogMsgAvail(String source, String msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public int id { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int boundX { get; private set; }
        public int boundY { get; private set; }
        public int boundWidth { get; private set; }
        public int boundHeight { get; private set; }
        public int stepSleep { get; private set; }
        public bool weightingEnabled { get; private set; }
        public double newColorWeight { get; private set; }
        public double maxBrightness { get; private set; }
        public Int16 redBias { get; private set; }
        public Int16 greenBias { get; private set; }
        public Int16 blueBias { get; private set; }
        public int captureThrottle { get; private set; }

        private String fileName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Device id</param>
        public DeviceSettings(int id)
        {
            this.id = id;
            AttachObserver(LoggerHelper.GetInstance());
            fileName = FILE_NAME_PREFIX + id;
            Reset();
        }

        public void Reset()
        {
            x = 0;
            y = 0;
            width = 0;
            height = 0;
            boundX = 0;
            boundY = 0;
            boundWidth = 0;
            boundHeight = 0;
            stepSleep = 1;
            captureThrottle = 100;
            redBias = 0;
            greenBias = 0;
            blueBias = 0;
            maxBrightness = 1.0;
            weightingEnabled = true;
            newColorWeight = 0.05;
            Notify();
        }

        /// <summary>
        /// Attach a LogMsgObserver to this object
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        /// <summary>
        /// Attach a ConfigurationObserver to this objectS
        /// </summary>
        /// <param name="o"></param>
        public void AttachObserver(ConfigurationObserver o)
        {
            ConfigChangeEvent += o.ConfigurationUpdate;
        }

        /// <summary>
        /// Notify any ConfigurationObservers of the current config
        /// </summary>
        public void Notify()
        {
            if (ConfigChangeEvent != null) {
                ConfigChangeEvent(this);
            }
        }

        public void ApplyChanges(SettingsDelta delta)
        {
            foreach (SettingValue variable in delta.changes.Keys) {
                switch (variable) {
                    case SettingValue.BLUEBIAS:
                        blueBias = (Int16)delta.changes[SettingValue.BLUEBIAS];
                        break;
                    case SettingValue.BOUNDHEIGHT:
                        boundHeight = (int)delta.changes[SettingValue.BOUNDHEIGHT];
                        break;
                    case SettingValue.BOUNDWIDTH:
                        boundWidth = (int)delta.changes[SettingValue.BOUNDWIDTH];
                        break;
                    case SettingValue.BOUNDX:
                        boundX = (int)delta.changes[SettingValue.BOUNDX];
                        break;
                    case SettingValue.BOUNDY:
                        boundY = (int)delta.changes[SettingValue.BOUNDY];
                        break;
                    case SettingValue.CAPTURETHROTTLE:
                        captureThrottle = (int)delta.changes[SettingValue.CAPTURETHROTTLE];
                        break;
                    case SettingValue.GREENBIAS:
                        greenBias = (Int16)delta.changes[SettingValue.GREENBIAS];
                        break;
                    case SettingValue.HEIGHT:
                        height = (int)delta.changes[SettingValue.HEIGHT];
                        break;
                    case SettingValue.MAXBRIGHTNESS:
                        maxBrightness = (double)delta.changes[SettingValue.MAXBRIGHTNESS];
                        break;
                    case SettingValue.NEWCOLORWEIGHT:
                        newColorWeight = (double)delta.changes[SettingValue.NEWCOLORWEIGHT];
                        break;
                    case SettingValue.REDBIAS:
                        redBias = (Int16)delta.changes[SettingValue.REDBIAS];
                        break;
                    case SettingValue.STEPSLEEP:
                        stepSleep = (int)delta.changes[SettingValue.STEPSLEEP];
                        break;
                    case SettingValue.WEIGHTINGENABLED:
                        weightingEnabled = (bool)delta.changes[SettingValue.WEIGHTINGENABLED];
                        break;
                    case SettingValue.WIDTH:
                        width = (int)delta.changes[SettingValue.WIDTH];
                        break;
                    case SettingValue.X:
                        x = (int)delta.changes[SettingValue.X];
                        break;
                    case SettingValue.Y:
                        y = (int)delta.changes[SettingValue.Y];
                        break;
                    default:
                        throw new ArgumentException("Unknown SettingValue " + variable);
                }
            }
            Notify();
        }

        /// <summary>
        /// Serialize this object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cnxt"></param>
        public DeviceSettings(SerializationInfo info, StreamingContext cnxt)
        {
            redBias = info.GetInt16("redBias");
            greenBias = info.GetInt16("greenBias");
            blueBias = info.GetInt16("blueBias");
            maxBrightness = info.GetDouble("maxBrightness");
            x = (int)info.GetValue("x", typeof(int));
            y = (int)info.GetValue("y", typeof(int));
            width = (int)info.GetValue("width", typeof(int));
            height = (int)info.GetValue("height", typeof(int));
            boundX = (int)info.GetValue("boundX", typeof(int));
            boundY = (int)info.GetValue("boundY", typeof(int));
            boundWidth = (int)info.GetValue("boundWidth", typeof(int));
            boundHeight = (int)info.GetValue("boundHeight", typeof(int));
            captureThrottle = (int)info.GetValue("captureThrottle", typeof(int));
            stepSleep = (int)info.GetValue("stepSleep", typeof(int));
            weightingEnabled = info.GetBoolean("weightingEnabled");
            newColorWeight = info.GetDouble("newColorWeight");
        }

        /// <summary>
        /// Deserialize this object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cnxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext cnxt)
        {
            info.AddValue("redBias", redBias);
            info.AddValue("greenBias", greenBias);
            info.AddValue("blueBias", blueBias);
            info.AddValue("maxBrightness", maxBrightness);
            info.AddValue("x", x);
            info.AddValue("y", y);
            info.AddValue("width", width);
            info.AddValue("height", height);
            info.AddValue("boundX", boundX);
            info.AddValue("boundY", boundY);
            info.AddValue("boundWidth", boundWidth);
            info.AddValue("boundHeight", boundHeight);
            info.AddValue("captureThrottle", captureThrottle);
            info.AddValue("stepSleep", stepSleep);
            info.AddValue("weightingEnabled", weightingEnabled);
            info.AddValue("newColorWeight", newColorWeight);
        }

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="msg"></param>
        private void Log(String msg)
        {
            if (this.NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("DeviceSettings id#" + this.id, msg);
            }
        }
    }
}
