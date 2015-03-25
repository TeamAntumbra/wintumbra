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
                _x = value;
                Notify();
            }
        }
        private int _x;
        public int y
        {
            get { return _y; }
            set
            {
                _y = value;
                Notify();
            }
        }
        private int _y;
        public int width
        {
            get { return _width; }
            set
            {
                _width = value;
                Notify();
            }
        }
        private int _width;
        public int height
        {
            get { return _height; }
            set
            {
                _height = value;
                Notify();
            }
        }
        private int _height;
        public int stepSleep
        {
            get { return _stepSleep; }
            set
            {
                _stepSleep = value;
                Notify();
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
                _weightingEnabled = value;
                Notify();
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
                _newColorWeight = value;
                Notify();
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
                _compoundDecoration = value;
                Notify();
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
                _maxBrightness = value;
                Notify();
            }
        }
        private UInt16 _maxBrightness;
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
            this.maxBrightness = UInt16.MaxValue;
            Notify();
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
            this.compoundDecoration = Boolean.Parse(parts[9]);
            Notify();
        }
    }
}
