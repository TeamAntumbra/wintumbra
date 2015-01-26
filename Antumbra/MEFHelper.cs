using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Antumbra.Glow.ExtensionFramework;
using System.Collections;
using System.Drawing;

namespace Antumbra.Glow
{
    class MEFHelper
    {
        private bool failed = false;
        private CompositionContainer container;
        private String path;

        [ImportMany]
        private IEnumerable<GlowExtension> plugins = null;

        //The Extension Bank
        private List<GlowDriver> AvailDrivers = null;
        private List<GlowScreenGrabber> AvailScreenDrivers = null;
        private List<GlowScreenProcessor> AvailScreenProcessors = null;
        private List<GlowDecorator> AvailDecorators = null;
        private List<GlowNotifier> AvailNotifiers = null;

        public MEFHelper(String pathToExtensions)
        {
            this.path = pathToExtensions;
            this.AvailDrivers = new List<GlowDriver>();
            this.AvailScreenDrivers = new List<GlowScreenGrabber>();
            this.AvailScreenProcessors = new List<GlowScreenProcessor>();
            this.AvailDecorators = new List<GlowDecorator>();
            this.AvailNotifiers = new List<GlowNotifier>();
            Compose();
            if (null == plugins) {
                Console.WriteLine("No extensions loaded, likely an extension loading exception occured.");
                failed = true;
                return;//no plugins loaded
            }
            int numExt = 0;
            foreach (var extension in plugins) {
                numExt += 1;
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
            DirectoryCatalog catalog = new DirectoryCatalog(this.path);// "*.glow.dll");
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

        public bool didFail()
        {
            return this.failed;
        }

        public GlowDriver GetDefaultDriver()
        {
            return this.AvailDrivers.First<GlowDriver>();//TODO change this, just for testing
        }

        public GlowScreenGrabber GetDefaultScreenDriver()
        {
            return this.AvailScreenDrivers.First<GlowScreenGrabber>();
        }

        public GlowScreenProcessor GetDefaultScreenProcessor()
        {
            return this.AvailScreenProcessors.First<GlowScreenProcessor>();
        }
    }
}
