using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antumbra.Glow.ExtensionFramework.Types;
using Antumbra.Glow.Observer.Logging;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class ExtensionLibrary : Loggable
    {
        public delegate void NewLogMsgAvail(string source, string msg);
        public event NewLogMsgAvail NewLogMsgAvailEvent;

        private MEFHelper MefHelper;
        private List<GlowDriver> Drivers;
        private List<GlowScreenGrabber> Grabbers;
        private List<GlowScreenProcessor> Processors;
        private List<GlowFilter> Filters;
        private List<GlowNotifier> Notifiers;
        private List<GlowExtension> Extensions;

        public ExtensionLibrary()
        {
            AttachObserver(LoggerHelper.GetInstance());
            MefHelper = new MEFHelper();
            Drivers = new List<GlowDriver>();
            Grabbers = new List<GlowScreenGrabber>();
            Processors = new List<GlowScreenProcessor>();
            Filters = new List<GlowFilter>();
            Notifiers = new List<GlowNotifier>();
            Extensions = new List<GlowExtension>();
        }

        public void Update()
        {
            Drivers.Clear();
            Grabbers.Clear();
            Processors.Clear();
            Filters.Clear();
            Notifiers.Clear();

            Dictionary<Type, List<GlowExtension>> ExtensionBank = MefHelper.LoadExtensions();
            Type current = typeof(GlowExtension);
            Extensions = ExtensionBank[current];
            current = typeof(GlowDriver);
            Drivers = ExtensionBank[current].Cast<GlowDriver>().ToList();
            current = typeof(GlowScreenGrabber);
            Grabbers = ExtensionBank[current].Cast<GlowScreenGrabber>().ToList();
            current = typeof(GlowScreenProcessor);
            Processors = ExtensionBank[current].Cast<GlowScreenProcessor>().ToList();
            current = typeof(GlowFilter);
            Filters = ExtensionBank[current].Cast<GlowFilter>().ToList();
            current = typeof(GlowNotifier);
            Notifiers = ExtensionBank[current].Cast<GlowNotifier>().ToList();
        }

        public GlowDriver LookupDriver(Guid id)
        {
            foreach (GlowDriver driver in Drivers) {
                if (driver.id.Equals(id)) {//it's a match
                    return driver.Create();
                }
            }
            return null;//not found
        }

        public GlowScreenGrabber LookupGrabber(Guid id)
        {
            foreach (GlowScreenGrabber grabber in Grabbers) {
                if (grabber.id.Equals(id)) {//it's a match
                    return grabber.Create();
                }
            }
            return null;//not found
        }

        public GlowScreenProcessor LookupProcessor(Guid id)
        {
            foreach (GlowScreenProcessor proc in Processors) {
                if (proc.id.Equals(id)) {//it's a match
                    return proc.Create();
                }
            }
            return null;//not found
        }

        public GlowFilter LookupFilter(Guid id)
        {
            foreach (GlowFilter filt in Filters) {
                if (filt.id.Equals(id)) {//it's a match
                    return filt.Create();
                }
            }
            return null;//not found
        }

        public GlowNotifier LookupNotifier(Guid id)
        {
            foreach (GlowNotifier notf in Notifiers) {
                if (notf.id.Equals(id)) {//it's a match
                    return notf.Create();
                }
            }
            return null;//not found
        }

        public void AttachObserver(LogMsgObserver observer)
        {
            NewLogMsgAvailEvent += observer.NewLogMsgAvail;
        }

        private void Log(string msg)
        {
            if (NewLogMsgAvailEvent != null)
                NewLogMsgAvailEvent("Extension Library", msg);
        }
    }
}
