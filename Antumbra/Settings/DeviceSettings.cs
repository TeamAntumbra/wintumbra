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
    }
}
