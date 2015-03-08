using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Extensions;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class ExtensionLibrary : GlowExtCollection
    {
        public delegate void CollectionUpdate(List<GlowExtension> exts);
        public event CollectionUpdate CollectionUpdateEvent;
        public List<GlowDriver> AvailDrivers { get; private set; }
        public List<GlowScreenGrabber> AvailGrabbers { get; private set; }
        public List<GlowScreenProcessor> AvailProcessors { get; private set; }
        public List<GlowDecorator> AvailDecorators { get; private set; }
        public List<GlowNotifier> AvailNotifiers { get; private set; }
        public List<GlowExtension> AvailExtensions { get; private set; }
        public bool ready { get; private set; }
        public ExtensionLibrary(string path)
        {
            MEFHelper helper = new MEFHelper(path);
            if (helper.failed) {
                this.ready = false;
                return;//cannot continue
            }
            this.AvailExtensions = new List<GlowExtension>();
            this.AvailDrivers = helper.AvailDrivers;
            this.AvailExtensions.AddRange(this.AvailDrivers);
            this.AvailGrabbers = helper.AvailScreenDrivers;
            this.AvailExtensions.AddRange(this.AvailGrabbers);
            this.AvailProcessors = helper.AvailScreenProcessors;
            this.AvailExtensions.AddRange(this.AvailProcessors);
            this.AvailDecorators = helper.AvailDecorators;
            this.AvailExtensions.AddRange(this.AvailDecorators);
            this.AvailNotifiers = helper.AvailNotifiers;
            this.AvailExtensions.AddRange(this.AvailNotifiers);
            helper.Dispose();
            if (CollectionUpdateEvent != null)
                CollectionUpdateEvent(this.AvailExtensions);
            this.ready = true;
        }

        public void NotifyObservers()
        {
            CollectionUpdateEvent(this.AvailExtensions);
        }

        public void AttachGlowExtCollectionObserver(GlowExtCollectionObserver observer)
        {
            CollectionUpdateEvent += observer.LibraryUpdate;
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

        public GlowExtension findExt(Guid id)
        {
            foreach (var e in AvailExtensions)
                if (e.id.Equals(id))
                    return e;
            return null;
        }
    }
}
