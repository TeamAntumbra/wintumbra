using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Utility.Settings;
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
        public bool compoundFilter
        {
            get
            {
                return _compoundFilter;
            }
            set
            {
                if (value != _compoundFilter) {
                    _compoundFilter = value;
                    Notify();
                }
            }
        }
        private bool _compoundFilter;
        public UInt16 maxBrightness
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
        private UInt16 _maxBrightness;
        public int redBias
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
        private int _redBias;
        public int greenBias
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
        private int _greenBias;
        public int blueBias
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
        private int _blueBias;
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
        public DeviceSettings(int id)
        {
            this.id = id;
            this.AttachObserver(LoggerHelper.GetInstance());
        }

        private String SerializeSettings()
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
            result += this.compoundFilter.ToString() + ',';
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
            this.compoundFilter = false;
            this.redBias = 0;
            this.greenBias = 0;
            this.blueBias = 0;
            this.maxBrightness = UInt16.MaxValue;
        }

        public Object LoadSave(String settings)
        {
            try {
                String[] parts = settings.Split(',');
                id = int.Parse(parts[0]);
                x = int.Parse(parts[1]);
                y = int.Parse(parts[2]);
                width = int.Parse(parts[3]);
                height = int.Parse(parts[4]);
                stepSleep = int.Parse(parts[5]);
                weightingEnabled = Boolean.Parse(parts[6]);
                newColorWeight = double.Parse(parts[7]);
                maxBrightness = UInt16.Parse(parts[8]);
                redBias = int.Parse(parts[9]);
                greenBias = int.Parse(parts[10]);
                blueBias = int.Parse(parts[11]);
                compoundFilter = Boolean.Parse(parts[12]);
                captureThrottle = int.Parse(parts[13]);
            }
            catch (Exception e) {
                this.Log("Loading settings failed!" + e.StackTrace + '\n' + e.Message);
                this.Reset();
            }
            this.Notify();
            return null;//unused
        }

        private void Log(String msg)
        {
            if (this.NewLogMsgAvailEvent != null) {
                NewLogMsgAvailEvent("DeviceSettings id#" + this.id, msg);
            }
        }
    }
}
