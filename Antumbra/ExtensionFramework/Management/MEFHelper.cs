using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections;
using System.Drawing;
using Antumbra.Glow.ExtensionFramework.Types;

namespace Antumbra.Glow.ExtensionFramework.Management
{
    public class MEFHelper : IDisposable
    {
        public bool failed { get; private set; }
        private CompositionContainer container;
        private String path;

        [ImportMany]
        private IEnumerable<GlowExtension> extensions;

        //The Extension Bank
        public List<GlowDriver> AvailDrivers { get; private set; }
        public List<GlowScreenGrabber> AvailScreenDrivers { get; private set; }
        public List<GlowScreenProcessor> AvailScreenProcessors { get; private set; }
        public List<GlowFilter> AvailFilters { get; private set; }
        public List<GlowNotifier> AvailNotifiers { get; private set; }

        public MEFHelper(String pathToExtensions)
        {
            this.failed = false;
            this.path = pathToExtensions;
            this.AvailDrivers = new List<GlowDriver>();
            this.AvailDrivers.Add(new GlowScreenDriverCoupler(null, null));//add coupler placeholder
            this.AvailScreenDrivers = new List<GlowScreenGrabber>();
            this.AvailScreenProcessors = new List<GlowScreenProcessor>();
            this.AvailFilters = new List<GlowFilter>();
            this.AvailNotifiers = new List<GlowNotifier>();

            Compose();
            if (null == extensions) {
                failed = true;
                return;//no plugins loaded
            }
            foreach (var extension in extensions) {
                if (extension is GlowDriver) {
                    this.AvailDrivers.Add((GlowDriver)extension);
                }
                else if (extension is GlowScreenGrabber) {
                    this.AvailScreenDrivers.Add((GlowScreenGrabber)extension);
                }
                else if (extension is GlowScreenProcessor) {
                    this.AvailScreenProcessors.Add((GlowScreenProcessor)extension);
                }
                else if (extension is GlowFilter) {
                    this.AvailFilters.Add((GlowFilter)extension);
                }
                else if (extension is GlowNotifier) {
                    this.AvailNotifiers.Add((GlowNotifier)extension);
                }
            }
        }

        private void Compose()
        {
            DirectoryCatalog catalog = new DirectoryCatalog(this.path, "*.glow.dll");
            this.container = new CompositionContainer(catalog);
            this.container.ComposeParts(this);
        }

        public GlowDriver GetDefaultDriver()
        {
            foreach (var drv in this.AvailDrivers)
                if (drv.IsDefault)
                    return drv;
            return null;
        }

        public GlowScreenGrabber GetDefaultGrabber()
        {
            foreach (var gbbr in this.AvailScreenDrivers)
                if (gbbr.IsDefault)
                    return gbbr;
            return null;
        }

        public GlowScreenProcessor GetDefaultProcessor()
        {
            foreach (var pcsr in this.AvailScreenProcessors)
                if (pcsr.IsDefault)
                    return pcsr;
            return null;
        }

        public List<GlowFilter> GetDefaultFilters()
        {
            List<GlowFilter> result = new List<GlowFilter>();
            foreach (var filt in this.AvailFilters)
                if (filt.IsDefault)
                    result.Add(filt);
            return result;
        }

        public List<GlowNotifier> GetDefaultNotifiers()
        {
            List<GlowNotifier> result = new List<GlowNotifier>();
            foreach (var notf in this.AvailNotifiers)
                if (notf.IsDefault)
                    result.Add(notf);
            return result;
        }

        public void Dispose()
        {
            if (this.container != null)
                this.container.Dispose();
        }
    }
}
