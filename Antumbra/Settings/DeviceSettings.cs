using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Observer.Saving;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Logging;
using System.Runtime.Serialization;

namespace Antumbra.Glow.Settings
{
    [Serializable()]
    public class DeviceSettings : Savable, Configurable, Loggable, ISerializable
    {
        public const String FILE_NAME_PREFIX = "Dev_Settings_";

        public delegate void ConfigurationChange(Configurable settings);
        public event ConfigurationChange ConfigChangeEvent;
        public delegate void NewLogMsgAvail(String source, String msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public int id { get; private set; }
        public int x
        {
            get { return _x; }
            set
            {
                if (value != _x) {
                    _x = value;
                    Notify();
                }
            }
        }
        private int _x;
        public int y
        {
            get { return _y; }
            set
            {
                if (value != _y) {
                    _y = value;
                    Notify();
                }
            }
        }
        private int _y;
        public int width
        {
            get { return _width; }
            set
            {
                if (value != _width) {
                    _width = value;
                    Notify();
                }
            }
        }
        private int _width;
        public int height
        {
            get { return _height; }
            set
            {
                if (value != _height) {
                    _height = value;
                    Notify();
                }
            }
        }
        private int _height;
        public int boundX
        {
            get { return _boundX; }
            set
            {
                if (value != _boundX) {
                    _boundX = value;
                    Notify();
                }
            }
        }
        private int _boundX;
        public int boundY
        {
            get { return _boundY; }
            set
            {
                if (value != _boundY) {
                    _boundY = value;
                    Notify();
                }
            }
        }
        private int _boundY;
        public int boundWidth
        {
            get { return _boundWidth; }
            set
            {
                if (value != _boundWidth) {
                    _boundWidth = value;
                    Notify();
                }
            }
        }
        private int _boundWidth;
        public int boundHeight
        {
            get { return _boundHeight; }
            set
            {
                if (value != _boundHeight) {
                    _boundHeight = value;
                    Notify();
                }
            }
        }
        private int _boundHeight;
        public int stepSleep
        {
            get { return _stepSleep; }
            set
            {
                if (value != _stepSleep) {
                    _stepSleep = value;
                    Notify();
                }
            }
        }
        private int _stepSleep;
        public bool weightingEnabled
        {
            get
            {
                return _weightingEnabled;
            }
            set
            {
                if (value != _weightingEnabled) {
                    _weightingEnabled = value;
                    Notify();
                }
            }
        }
        private bool _weightingEnabled;
        public double newColorWeight
        {
            get
            {
                return _newColorWeight;
            }
            set
            {
                if (value != _newColorWeight) {
                    _newColorWeight = value;
                    Notify();
                }
            }
        }
        private double _newColorWeight;
        public double maxBrightness
        {
            get
            {
                return _maxBrightness;
            }
            set
            {
                if (value != _maxBrightness) {
                    _maxBrightness = value;
                    Notify();
                }
            }
        }
        private double _maxBrightness;
        public Int16 redBias
        {
            get
            {
                return _redBias;
            }
            set
            {
                if (value != _redBias) {
                    _redBias = value;
                    Notify();
                }
            }
        }
        private Int16 _redBias;
        public Int16 greenBias
        {
            get
            {
                return _greenBias;
            }
            set
            {
                if (value != _greenBias) {
                    _greenBias = value;
                    Notify();
                }
            }
        }
        private Int16 _greenBias;
        public Int16 blueBias
        {
            get
            {
                return _blueBias;
            }
            set
            {
                if (value != _blueBias) {
                    _blueBias = value;
                    Notify();
                }
            }
        }
        private Int16 _blueBias;
        public bool powerState
        {
            get
            {
                return _powerState;
            }
            set
            {
                if (value != _powerState) {
                    _powerState = value;
                    Notify();
                }
            }
        }
        private bool _powerState;

        public int captureThrottle
        {
            get
            {
                return _captureThrottle;
            }
            set
            {
                if (value != _captureThrottle) {
                    _captureThrottle = value;
                    Notify();
                }
            }
        }
        private int _captureThrottle;
        private String fileName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Device id</param>
        public DeviceSettings(int id)
        {
            this.id = id;
            this.AttachObserver(LoggerHelper.GetInstance());
            this.fileName = FILE_NAME_PREFIX + this.id.ToString();
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
        /// Serialize and save this object
        /// </summary>
        public void Save()
        {
            Saver.GetInstance().Save(FILE_NAME_PREFIX + id.ToString(), this);
        }

        /// <summary>
        /// Alert developer this method should not be used here.
        /// </summary>
        public void Load()
        {
            throw new NotImplementedException("SettingsManager should load and replace this object rather than calling its Load()");
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
            if (ConfigChangeEvent != null)
                ConfigChangeEvent(this);
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
