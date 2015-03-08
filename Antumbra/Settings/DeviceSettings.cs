using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;
using Antumbra.Glow.Utility.Settings;

namespace Antumbra.Glow.Settings
{
    public class DeviceSettings : Savable
    {
        public int id { get; private set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int stepSleep { get; set; }//for the driver
        public bool weightingEnabled { get; set; }//for the output loop
        public double newColorWeight { get; set; }//for the output loop
        public bool compoundDecoration { get; set; }
        public DeviceSettings(int id)
        {
            ResetSettings();
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
            result += this.compoundDecoration.ToString() + '\n';
            return result;
        }

        public void SaveSettings()
        {
            Saver saver = Saver.GetInstance();
            saver.Save(this.id.ToString(), SerializeSettings());
        }

        public void ResetSettings()
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
            this.compoundDecoration = true;
        }

        public void LoadSettings(String settings)
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
            this.compoundDecoration = Boolean.Parse(parts[8]);
        }
    }
}
