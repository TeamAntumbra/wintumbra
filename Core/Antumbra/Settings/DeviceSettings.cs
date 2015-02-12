using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antumbra.Glow.ExtensionFramework;

namespace Antumbra.Glow.Settings
{
    public class DeviceSettings
    {
        public bool active { get; set; }
        public int id { get; private set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Screen screen { get; set; }
        public int stepSleep { get; set; }//for the driver
        public bool weightingEnabled { get; set; }//for the output loop
        public double newColorWeight { get; set; }//for the output loop
        public DeviceSettings(int id)
        {
            this.active = (id == 0);//default on if first device found
            this.id = id;
            this.x = 0;
            this.y = 0;
            this.screen = Screen.PrimaryScreen;
            this.width = this.screen.Bounds.Width;
            this.height = this.screen.Bounds.Height;
            this.stepSleep = 0;
            this.weightingEnabled = true;
            this.newColorWeight = .05;
        }
    }
}
