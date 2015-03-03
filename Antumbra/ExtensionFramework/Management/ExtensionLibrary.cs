using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Types;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class ExtensionLibrary
    {
        public List<GlowDriver> AvailDrivers { get; private set; }
        public List<GlowScreenGrabber> AvailGrabbers { get; private set; }
        public List<GlowScreenProcessor> AvailProcessors { get; private set; }
        public List<GlowDecorator> AvailDecorators { get; private set; }
        public List<GlowNotifier> AvailNotifiers { get; private set; }
        public bool ready { get; private set; }
        public ExtensionLibrary(string path)
        {
            MEFHelper helper = new MEFHelper(path);
            if (helper.failed) {
                this.ready = false;
                return;//cannot continue
            }
            List<GlowExtension> extensions = new List<GlowExtension>();
            this.AvailDrivers = helper.AvailDrivers;
            extensions.AddRange(this.AvailDrivers);
            this.AvailGrabbers = helper.AvailScreenDrivers;
            extensions.AddRange(this.AvailGrabbers);
            this.AvailProcessors = helper.AvailScreenProcessors;
            extensions.AddRange(this.AvailProcessors);
            this.AvailDecorators = helper.AvailDecorators;
            extensions.AddRange(this.AvailDecorators);
            this.AvailNotifiers = helper.AvailNotifiers;
            extensions.AddRange(this.AvailNotifiers);
            helper.Dispose();
            AssignGuids(extensions);
            this.ready = true;
        }

        public GlowDriver GetDefaultDriver()
        {
            foreach (GlowDriver dvr in this.AvailDrivers)
                if (dvr.IsDefault)
                    return dvr;
            return null;
        }

        public GlowScreenGrabber GetDefaultGrabber()
        {
            foreach (GlowScreenGrabber gbr in this.AvailGrabbers)
                if (gbr.IsDefault)
                    return gbr;
            return null;
        }

        public GlowScreenProcessor GetDefaultProcessor()
        {
            foreach (GlowScreenProcessor pcr in this.AvailProcessors)
                if (pcr.IsDefault)
                    return pcr;
            return null;
        }

        public List<GlowDecorator> GetDefaultDecorators()
        {
            List<GlowDecorator> result = new List<GlowDecorator>();
            foreach (GlowDecorator dec in this.AvailDecorators)
                if (dec.IsDefault)
                    result.Add(dec);
            return result;
        }

        public List<GlowNotifier> GetDefaultNotifiers()
        {
            List<GlowNotifier> result = new List<GlowNotifier>();
            foreach (GlowNotifier notf in this.AvailNotifiers)
                if (notf.IsDefault)
                    result.Add(notf);
            return result;
        }

        private void AssignGuids(List<GlowExtension> exts)
        {
            foreach (var ext in exts)
                ext.id = Guid.NewGuid();
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
    }
}
