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
        public List<GlowScreenGrabber> AvailScreenGrabbers { get; private set; }
        public List<GlowScreenProcessor> AvailScreenProcessors { get; private set; }
        public List<GlowFilter> AvailFilters { get; private set; }
        public List<GlowNotifier> AvailNotifiers { get; private set; }

        public MEFHelper(String pathToExtensions)
        {
            failed = false;
            path = pathToExtensions;
            AvailDrivers = new List<GlowDriver>();
            AvailDrivers.Add(new GlowScreenDriverCoupler(null, null));//add coupler placeholder
            AvailScreenGrabbers = new List<GlowScreenGrabber>();
            AvailScreenProcessors = new List<GlowScreenProcessor>();
            AvailFilters = new List<GlowFilter>();
            AvailNotifiers = new List<GlowNotifier>();

            Compose();
            if (null == extensions) {
                failed = true;
                return;//no plugins loaded
            }
            foreach (GlowExtension extension in extensions) {//TODO: Investigate using Dictionary<Type, List<GlowExtension>> to hold extensions
                if (extension is GlowDriver) {
                    AvailDrivers.Add((GlowDriver)extension);
                }
                else if (extension is GlowScreenGrabber) {
                    AvailScreenGrabbers.Add((GlowScreenGrabber)extension);
                }
                else if (extension is GlowScreenProcessor) {
                    AvailScreenProcessors.Add((GlowScreenProcessor)extension);
                }
                else if (extension is GlowFilter) {
                    AvailFilters.Add((GlowFilter)extension);
                }
                else if (extension is GlowNotifier) {
                    AvailNotifiers.Add((GlowNotifier)extension);
                }
            }
        }

        private void Compose()
        {
            DirectoryCatalog catalog = new DirectoryCatalog(this.path, "*.glow.dll");
            container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public void Dispose()
        {
            if (this.container != null) {
                this.container.Dispose();
            }
        }
    }
}
