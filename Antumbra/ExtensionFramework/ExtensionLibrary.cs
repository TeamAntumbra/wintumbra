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
        public bool ready { get; private set; }
        private string extPath;
        public ExtensionLibrary(string path)
        {
            this.extPath = path;
            this.ready = false;
            Update();
        }

        public GlowExtension findExt(Guid id)
        {
            foreach (var e in AvailDrivers)
                if (e.id.Equals(id))
                    return e;
            foreach (var e in AvailGrabbers)
                if (e.id.Equals(id))
                    return e;
            foreach (var e in AvailProcessors)
                if (e.id.Equals(id))
                    return e;
            foreach (var e in AvailDecorators)
                if (e.id.Equals(id))
                    return e;
            foreach (var e in AvailNotifiers)
                if (e.id.Equals(id))
                    return e;
            return null;
        }

        private void Update()
        {
            this.ready = false;
            var mef = new MEFHelper(extPath);
            if (mef.failed) {
                return;
            }
            this.AvailDrivers = mef.AvailDrivers;
            this.AvailGrabbers = mef.AvailScreenDrivers;
            this.AvailProcessors = mef.AvailScreenProcessors;
            this.AvailDecorators = mef.AvailDecorators;
            this.AvailNotifiers = mef.AvailNotifiers;
            mef.Dispose();
            this.ready = true;
        }
    }
}
