using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Logging;
using Antumbra.Glow.Observer.Saving;
using System;
using System.Runtime.Serialization;

namespace Antumbra.Glow.Settings {

    /// <summary>
    /// Holds a Glow device's settings
    /// Note: This class is Savable despite it not implementing the Savable interface The caveat
    ///       here is that it cannot save itself
    /// </summary>
    [Serializable()]
    public class DeviceSettings : Configurable, Loggable, ISerializable, Savable, ConfigurationChanger {

        #region Public Fields

        public const String FILE_NAME_PREFIX = "Dev_Settings_";

        #endregion Public Fields

        #region Private Fields

        private String fileName;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Device id</param>
        public DeviceSettings(int id) {
            this.id = id;
            AttachObserver(LoggerHelper.GetInstance());
            fileName = FILE_NAME_PREFIX + id;
            Reset();
        }

        /// <summary>
        /// Serialize this object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cnxt"></param>
        public DeviceSettings(SerializationInfo info, StreamingContext cnxt) {
            redBias = info.GetByte("redBias");
            greenBias = info.GetByte("greenBias");
            blueBias = info.GetByte("blueBias");
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

        #endregion Public Constructors

        #region Public Delegates

        public delegate void ConfigurationChange(Configurable settings);

        public delegate void NewLogMsgAvail(String source, String msg);

        #endregion Public Delegates

        #region Public Events

        public event ConfigurationChange ConfigChangeEvent;

        public event NewLogMsgAvail NewLogMsgAvailEvent;

        #endregion Public Events

        #region Public Properties

        public byte blueBias { get; private set; }
        public int boundHeight { get; private set; }
        public int boundWidth { get; private set; }
        public int boundX { get; private set; }
        public int boundY { get; private set; }
        public int captureThrottle { get; private set; }
        public byte greenBias { get; private set; }
        public int height { get; private set; }
        public int id { get; private set; }
        public double maxBrightness { get; private set; }
        public double newColorWeight { get; private set; }
        public byte redBias { get; private set; }
        public int stepSleep { get; private set; }
        public bool weightingEnabled { get; private set; }
        public int width { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void ApplyChanges(SettingsDelta delta) {
            Log(delta.changes.Count + " settings updated.\t" + delta.ToString());
            foreach(SettingValue variable in delta.changes.Keys) {
                switch(variable) {
                    case SettingValue.BLUEBIAS:
                        blueBias = Convert.ToByte(delta.changes[SettingValue.BLUEBIAS]);
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
                        greenBias = Convert.ToByte(delta.changes[SettingValue.GREENBIAS]);
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
                        redBias = Convert.ToByte(delta.changes[SettingValue.REDBIAS]);
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
                        Log("Unknown SettingValue " + variable);
                        break;
                }
            }
            Save();
            Notify();
        }

        /// <summary>
        /// Attach a LogMsgObserver to this object
        /// </summary>
        /// <param name="observer"></param>
        public void AttachObserver(LogMsgObserver observer) {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        /// <summary>
        /// Attach a ConfigurationObserver to this objectS
        /// </summary>
        /// <param name="o"></param>
        public void AttachObserver(ConfigurationObserver o) {
            ConfigChangeEvent += o.ConfigurationUpdate;
        }

        public void ConfigChange(SettingsDelta Delta) {
            ApplyChanges(Delta);
        }

        /// <summary>
        /// Deserialize this object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="cnxt"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext cnxt) {
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

        public void Load() {
            DeviceSettings loaded = (DeviceSettings)Saver.GetInstance().Load(DeviceSettings.FILE_NAME_PREFIX + id);
            if(loaded != null) {
                x = loaded.x;
                y = loaded.y;
                width = loaded.width;
                height = loaded.height;
                boundX = loaded.boundX;
                boundY = loaded.boundY;
                boundWidth = loaded.boundWidth;
                boundHeight = loaded.boundHeight;
                redBias = loaded.redBias;
                greenBias = loaded.greenBias;
                blueBias = loaded.blueBias;
                captureThrottle = loaded.captureThrottle;
                maxBrightness = loaded.maxBrightness;
                weightingEnabled = loaded.weightingEnabled;
                newColorWeight = loaded.newColorWeight;
                stepSleep = loaded.stepSleep;
            }
        }

        /// <summary>
        /// Notify any ConfigurationObservers of the current config
        /// </summary>
        public void Notify() {
            if(ConfigChangeEvent != null) {
                ConfigChangeEvent(this);
            }
        }

        public void Reset() {
            x = 0;
            y = 0;
            width = 800;
            height = 800;
            boundX = 0;
            boundY = 0;
            boundWidth = 0;
            boundHeight = 0;
            stepSleep = 0;
            captureThrottle = 20;
            redBias = 0;
            greenBias = 0;
            blueBias = 0;
            maxBrightness = 1.0;
            weightingEnabled = true;
            newColorWeight = 0.20;
            Notify();
        }

        public void Save() {
            Saver.GetInstance().Save(FILE_NAME_PREFIX + id, this);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Log a message
        /// </summary>
        /// <param name="msg"></param>
        private void Log(String msg) {
            if(this.NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("DeviceSettings id#" + this.id, msg);
            }
        }

        #endregion Private Methods
    }
}
