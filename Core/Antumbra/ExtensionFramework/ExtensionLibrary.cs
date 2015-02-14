using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antumbra.Glow.ExtensionFramework
{
    public class ExtensionLibrary
    {
        public List<GlowDriver> AvailDrivers { get; private set; }
        public List<GlowScreenGrabber> AvailGrabbers { get; private set; }
        public List<GlowScreenProcessor> AvailProcessors { get; private set; }
        public List<GlowDecorator> AvailDecorators { get; private set; }
        public List<GlowNotifier> AvailNotifiers { get; private set; }
        private string extPath;
        public ExtensionLibrary(string path)
        {
            this.extPath = path;
            Update();
        }

        public void Update()
        {
            var mef = new MEFHelper(extPath);
            this.AvailDrivers = mef.AvailDrivers;
            this.AvailGrabbers = mef.AvailScreenDrivers;
            this.AvailProcessors = mef.AvailScreenProcessors;
            this.AvailDecorators = mef.AvailDecorators;
            this.AvailNotifiers = mef.AvailNotifiers;
            mef.Dispose();
        }
    }
}
