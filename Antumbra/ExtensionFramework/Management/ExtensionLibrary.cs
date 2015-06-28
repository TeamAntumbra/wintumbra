﻿using System;
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
            this.AttachObserver(LoggerHelper.GetInstance());
            try {
                MEFHelper helper = new MEFHelper(this.path);
                if (helper.failed) {
                    this.Log("MEFHelper failed to initalize correctly.");
                    return;//cannot continue
                }
                this.AvailExtensions = new List<GlowExtension>();
                this.AvailDrivers = helper.AvailDrivers;
                this.AvailExtensions.AddRange(this.AvailDrivers);
                this.AvailGrabbers = helper.AvailScreenDrivers;
                this.AvailExtensions.AddRange(this.AvailGrabbers);
                this.AvailProcessors = helper.AvailScreenProcessors;
                this.AvailExtensions.AddRange(this.AvailProcessors);
                this.AvailFilters = helper.AvailFilters;
                this.AvailExtensions.AddRange(this.AvailFilters);
                this.AvailNotifiers = helper.AvailNotifiers;
                this.AvailExtensions.AddRange(this.AvailNotifiers);
                if (helper != null)
                    helper.Dispose();
                LogFoundExtensions();
                if (CollectionUpdateEvent != null)
                    CollectionUpdateEvent(this.AvailExtensions);
            }
            catch (TypeAccessException ex) {
                this.Log(ex.StackTrace + '\n' + ex.Message);
            }
            catch (TypeLoadException ex) {
                this.Log(ex.StackTrace + '\n' + ex.Message);
            }
        }

        public GlowExtension LookupExt(Guid id)
        {
            foreach (GlowExtension ext in this.AvailExtensions)
                if (ext.id.Equals(id)) {//it's a match
                    return ext.Create();//TODO share single capture instances
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
            result.ActiveProcessor = this.GetDefaultProcessor();
            foreach (GlowFilter filt in this.GetDefaultFilters())
                result.ActiveFilters.Add(filt);
            foreach (GlowNotifier notf in this.GetDefaultNotifiers())
                result.ActiveNotifiers.Add(notf);
            return result;
        }

        private GlowDriver GetDefaultDriver()
        {
            foreach (GlowDriver dvr in this.AvailDrivers)
                if (dvr.IsDefault)
                    return dvr;
            return null;
        }

        private GlowScreenGrabber GetDefaultGrabber()
        {
            foreach (GlowScreenGrabber gbr in this.AvailGrabbers)
                if (gbr.IsDefault)
                    return gbr;
            return null;
        }

        private GlowScreenProcessor GetDefaultProcessor()
        {
            foreach (GlowScreenProcessor pcr in this.AvailProcessors)
                if (pcr.IsDefault)
                    return pcr;
            return null;
        }

        private List<GlowFilter> GetDefaultFilters()
        {
            List<GlowFilter> result = new List<GlowFilter>();
            foreach (GlowFilter filt in this.AvailFilters)
                if (filt.IsDefault)
                    result.Add(filt);
            return result;
        }

        private List<GlowNotifier> GetDefaultNotifiers()
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
