using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections;
using System.Drawing;

namespace Antumbra.Glow.ExtensionFramework
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
        public List<GlowDecorator> AvailDecorators { get; private set; }
        public List<GlowNotifier> AvailNotifiers { get; private set; }

        public MEFHelper(String pathToExtensions)
        {
            this.failed = false;
            this.path = pathToExtensions;
            this.AvailDrivers = new List<GlowDriver>();
            this.AvailDrivers.Add(new GlowScreenDriverCoupler(null, null));//add coupler placeholder
            this.AvailScreenDrivers = new List<GlowScreenGrabber>();
            this.AvailScreenProcessors = new List<GlowScreenProcessor>();
            this.AvailDecorators = new List<GlowDecorator>();
            this.AvailNotifiers = new List<GlowNotifier>();

            Compose();
            if (null == extensions) {
                Console.WriteLine("No extensions loaded, likely an extension loading exception occured.");
                failed = true;
                return;//no plugins loaded
            }
            int numExt = 0;
            foreach (var extension in extensions) {
                numExt += 1;
                extension.id = numExt;
                Console.WriteLine("Extension Found: " + extension.Name);
                if (extension is GlowDriver) {
                    Console.WriteLine("Type: Driver");
                    this.AvailDrivers.Add((GlowDriver)extension);
                }
                else if (extension is GlowScreenGrabber) {
                    Console.WriteLine("Type: Screen Grabber");
                    this.AvailScreenDrivers.Add((GlowScreenGrabber)extension);
                }
                else if (extension is GlowScreenProcessor) {
                    Console.WriteLine("Type: Screen Processor");
                    this.AvailScreenProcessors.Add((GlowScreenProcessor)extension);
                }
                else if (extension is GlowDecorator) {
                    Console.WriteLine("Type: Decorator");
                    this.AvailDecorators.Add((GlowDecorator)extension);
                }
                else if (extension is GlowNotifier) {
                    Console.WriteLine("Type: Notifier");
                    this.AvailNotifiers.Add((GlowNotifier)extension);
                }
                else {
                    Console.WriteLine("Ignoring Extension - invalid type");
                }
            }
            if (numExt == 0) {
                Console.WriteLine("No extensions found.");
                this.failed = true;
            }
        }

        private void Compose()
        {
            DirectoryCatalog catalog = new DirectoryCatalog(this.path, "*.glow.dll");
            this.container = new CompositionContainer(catalog);
            try {
                this.container.ComposeParts(this);
            }
            catch (System.Reflection.ReflectionTypeLoadException reflectionTypeLoadException) {
                Console.WriteLine("An exception occured while loading extensions. Now printing:");
                foreach (Exception exception in reflectionTypeLoadException.LoaderExceptions) {
                    Console.WriteLine('\n' + exception.ToString() + '\n');
                }
            }
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

        public List<GlowDecorator> GetDefaultDecorators()
        {
            List<GlowDecorator> result = new List<GlowDecorator>();
            foreach (var dctr in this.AvailDecorators)
                if (dctr.IsDefault)
                    result.Add(dctr);
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
            this.container.Dispose();
        }
    }
}
