using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Extensions;
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class ExtensionLibrary : GlowExtCollection, Loggable
    {
        public delegate void NewLogMsgAvail(string source, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;
        public delegate void CollectionUpdate(List<GlowExtension> exts);
        public event CollectionUpdate CollectionUpdateEvent;
        private List<GlowDriver> AvailDrivers;
        private List<GlowScreenGrabber> AvailGrabbers;
        private List<GlowScreenProcessor> AvailProcessors;
        private List<GlowFilter> AvailFilters;
        private List<GlowNotifier> AvailNotifiers;
        private List<GlowExtension> AvailExtensions;
        private String path;
        public ExtensionLibrary(String path)
        {
            this.path = path;
            AttachObserver(LoggerHelper.GetInstance());
            MEFHelper helper = new MEFHelper(this.path);
            if (helper.failed) {
                throw new Exception("MEFHelper failed to initalize correctly.");
            }
            AvailExtensions = new List<GlowExtension>();
            AvailDrivers = helper.AvailDrivers;
            AvailExtensions.AddRange(AvailDrivers);
            AvailGrabbers = helper.AvailScreenGrabbers;
            AvailExtensions.AddRange(AvailGrabbers);
            AvailProcessors = helper.AvailScreenProcessors;
            AvailExtensions.AddRange(AvailProcessors);
            AvailFilters = helper.AvailFilters;
            AvailExtensions.AddRange(AvailFilters);
            AvailNotifiers = helper.AvailNotifiers;
            AvailExtensions.AddRange(AvailNotifiers);
            if (helper != null)
                helper.Dispose();
            LogFoundExtensions();
            if (CollectionUpdateEvent != null)
                CollectionUpdateEvent(this.AvailExtensions);
        }

        public GlowExtension LookupExt(Guid id)
        {
            foreach (GlowExtension ext in AvailExtensions) {
                if (ext.id.Equals(id)) {//it's a match
                    return ext.Create();
                }
            }
            return null;//not found
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            this.NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        private void LogFoundExtensions()
        {
            this.Log("Found Extensions:");
            LogExtensions("Drivers", this.AvailDrivers.ToList<GlowExtension>());
            LogExtensions("Screen Grabbers", this.AvailGrabbers.ToList<GlowExtension>());
            LogExtensions("Screen Processors", this.AvailProcessors.ToList<GlowExtension>());
            LogExtensions("Filters", this.AvailFilters.ToList<GlowExtension>());
            LogExtensions("Notifiers", this.AvailNotifiers.ToList<GlowExtension>());
        }

        private void LogExtensions(String type, List<GlowExtension> exts)
        {
            this.Log("Found " + type + ":");
            foreach (var ext in exts)
                this.Log("\t" + ext.ToString());
        }

        private void Log(string msg)
        {
            if (this.NewLogMsgAvailEvent != null)
                this.NewLogMsgAvailEvent("Extension Library", msg);
        }

        public void NotifyObservers()
        {
            if (CollectionUpdateEvent != null)
                CollectionUpdateEvent(this.AvailExtensions);
        }

        public void AttachObserver(GlowExtCollectionObserver observer)
        {
            CollectionUpdateEvent += observer.LibraryUpdate;
        }

        public Settings.ActiveExtensions GetDefaults()
        {
            Settings.ActiveExtensions result = new Settings.ActiveExtensions();
            result.ActiveDriver = this.GetDefaultDriver();
            result.ActiveGrabber = this.GetDefaultGrabber();
            result.ActiveProcessors = this.GetDefaultProcessor();
            result.ActiveFilters = this.GetDefaultFilters();
            result.ActiveNotifiers = this.GetDefaultNotifiers();
            return result;
        }

        private GlowDriver GetDefaultDriver()
        {
            foreach (GlowDriver dvr in this.AvailDrivers)
                if (dvr.IsDefault)
                    return (GlowDriver)dvr.Create();
            return null;
        }

        private GlowScreenGrabber GetDefaultGrabber()
        {
            foreach (GlowScreenGrabber gbr in this.AvailGrabbers)
                if (gbr.IsDefault)
                    return (GlowScreenGrabber)gbr.Create();
            return null;
        }

        private List<GlowScreenProcessor> GetDefaultProcessor()
        {
            List<GlowScreenProcessor> result = new List<GlowScreenProcessor>();
            foreach (GlowScreenProcessor pcr in this.AvailProcessors)
                if (pcr.IsDefault)
                    result.Add((GlowScreenProcessor)pcr.Create());
            return result;
        }

        private List<GlowFilter> GetDefaultFilters()
        {
            List<GlowFilter> result = new List<GlowFilter>();
            foreach (GlowFilter filt in this.AvailFilters)
                if (filt.IsDefault)
                    result.Add((GlowFilter)filt.Create());
            return result;
        }

        private List<GlowNotifier> GetDefaultNotifiers()
        {
            List<GlowNotifier> result = new List<GlowNotifier>();
            foreach (GlowNotifier notf in this.AvailNotifiers)
                if (notf.IsDefault)
                    result.Add((GlowNotifier)notf.Create());
            return result;
        }

        public GlowExtension findExt(Guid id)
        {
            foreach (var e in AvailExtensions)
                if (e.id.Equals(id))
                    return e.Create();
            return null;
        }
    }
}
