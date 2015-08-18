using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.Observer.Colors;

namespace Antumbra.Glow.ExtensionFramework.Types
{
    public abstract class GlowDriver : GlowExtension
    {
        public abstract void RecmmndCoreSettings();
        public abstract void AttachColorObserver(AntumbraColorObserver observer);
        public int stepSleep { get; set; }
        public bool weighted { get; set; }
        public double newColorWeight { get; set; }
        public abstract GlowDriver Create();
    }
}
