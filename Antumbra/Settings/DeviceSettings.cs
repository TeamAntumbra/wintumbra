using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Utility.Settings;
using Antumbra.Glow.Observer.Configuration;

namespace Antumbra.Glow.Settings
{
    public class DeviceSettings : Savable, Configurable
    {
        public delegate void ConfigurationChange(Configurable settings);
        public event ConfigurationChange ConfigChangeEvent;
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
        public bool compoundDecoration
        {
            get
            {
                return _compoundDecoration;
            }
            set
            {
                if (value != _compoundDecoration) {
                    _compoundDecoration = value;
                    Notify();
                }
            }
        }
        private bool _compoundDecoration;
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
        public DeviceSettings(int id)
        {
            Reset();
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
            result += this.compoundDecoration.ToString() + '\n';
            return result;
        }

        public void Save()
        {
            Saver saver = Saver.GetInstance();
            saver.Save(this.id.ToString(), SerializeSettings());
        }

        public void AttachObserver(ConfigurationObserver o)
        {
            this.ConfigChangeEvent += o.ConfigurationUpdate;
        }

        public void Notify()
        {
            if (ConfigChangeEvent != null)
                ConfigChangeEvent(this);
        }

        public void Reset()
        {
            this.id = id;
            this.x = 0;
            this.y = 0;
            var bounds = Screen.PrimaryScreen.Bounds;
            this.width = bounds.Width;
            this.height = bounds.Height;
            this.stepSleep = 1;
            this.weightingEnabled = true;
            this.newColorWeight = .05;
            this.compoundDecoration = false;
            this.redBias = 0;
            this.greenBias = 0;
            this.blueBias = 0;
            this.maxBrightness = UInt16.MaxValue;
        }

        public void LoadSave(String settings)
        {
            String[] parts = settings.Split(',');
            this.id = int.Parse(parts[0]);
            this.x = int.Parse(parts[1]);
            this.y = int.Parse(parts[2]);
            this.width = int.Parse(parts[3]);
            this.height = int.Parse(parts[4]);
            this.stepSleep = int.Parse(parts[5]);
            this.weightingEnabled = Boolean.Parse(parts[6]);
            this.newColorWeight = double.Parse(parts[7]);
            this.maxBrightness = UInt16.Parse(parts[8]);
            this.redBias = int.Parse(parts[9]);
            this.greenBias = int.Parse(parts[10]);
            this.blueBias = int.Parse(parts[11]);
            this.compoundDecoration = Boolean.Parse(parts[9]);
            Notify();
        }
    }
}
