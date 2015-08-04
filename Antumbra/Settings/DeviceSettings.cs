using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Utility.Saving;
using Antumbra.Glow.Observer.Configuration;
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.Settings
{
    public class DeviceSettings : Savable, Configurable, Loggable
    {
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

        private static readonly String FILE_NAME_PREFIX = "Dev_Settings_";
        private String fileName;

        public DeviceSettings(int id)
        {
            this.id = id;
            this.AttachObserver(LoggerHelper.GetInstance());
            this.fileName = FILE_NAME_PREFIX + this.id.ToString();
        }

        private String SerializeSettings()//TODO serialize entire object instead of CSV
        {
            String result = "";
            result += this.id.ToString() + ',';
            result += this.x.ToString() + ',';
            result += this.y.ToString() + ',';
            result += this.width.ToString() + ',';
            result += this.height.ToString() + ',';
            result += this.stepSleep.ToString() + ',';
            result += this.weightingEnabled.ToString() + ',';
            result += this.newColorWeight.ToString() + ',';
            result += this.maxBrightness.ToString() + ',';
            result += this.redBias.ToString() + ',';
            result += this.greenBias.ToString() + ',';
            result += this.blueBias.ToString() + ',';
            result += this.captureThrottle.ToString() + '\n';
            return result;
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        public void Save()
        {
            Saver saver = Saver.GetInstance();
            saver.Save(id.ToString(), SerializeSettings());
        }

        public void Load()
        {
            //TODO load serialized values from fileName
        }

        public void AttachObserver(ConfigurationObserver o)
        {
            ConfigChangeEvent += o.ConfigurationUpdate;
        }

        public void Notify()
        {
            if (ConfigChangeEvent != null)
                ConfigChangeEvent(this);
        }

        public void Reset()//reset everything except power state
        {
            this.id = id;
            this.x = Screen.PrimaryScreen.Bounds.X;
            this.y = Screen.PrimaryScreen.Bounds.Y;
            this.width = Screen.PrimaryScreen.Bounds.Width;
            this.height = Screen.PrimaryScreen.Bounds.Height;
            this.stepSleep = 1;
            this.weightingEnabled = true;
            this.newColorWeight = .05;
            this.redBias = 0;
            this.greenBias = 0;
            this.blueBias = 0;
            this.maxBrightness = UInt16.MaxValue;
        }

        private void Log(String msg)
        {
            if (this.NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("DeviceSettings id#" + this.id, msg);
            }
        }
    }
}
