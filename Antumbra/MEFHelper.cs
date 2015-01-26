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
    public class MEFHelper
    {
        private bool failed = false;
        private CompositionContainer container;
        private String path;

        [ImportMany]
        private IEnumerable<GlowExtension> plugins;

        //The Extension Bank
        private List<GlowDriver> AvailDrivers;
        private List<GlowScreenGrabber> AvailScreenDrivers;
        private List<GlowScreenProcessor> AvailScreenProcessors;
        private List<GlowDecorator> AvailDecorators;
        private List<GlowNotifier> AvailNotifiers;

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

        public bool didFail()
        {
            return this.failed;
        }

        public string[] GetNamesOfAvailIndependentDrivers()
        {
            List<string> result = new List<string>();
            foreach(var ext in this.AvailDrivers.ToList<GlowDriver>())
                result.Add(ext.Name);
            return result.ToArray<string>();
        }

        public string[] GetNamesOfAvailScreenGrabbers()
        {
            List<string> result = new List<string>();
            foreach (var ext in this.AvailScreenDrivers.ToList<GlowScreenGrabber>())
                result.Add(ext.Name);
            return result.ToArray<string>();
        }

        public string[] GetNamesOfAvailScreenProcessors()
        {
            List<string> result = new List<string>();
            foreach (var ext in this.AvailScreenProcessors.ToList<GlowScreenProcessor>())
                result.Add(ext.Name);
            return result.ToArray<string>();
        }

        public string[] GetNamesOfAvailDecorators()
        {
            List<string> result = new List<string>();
            foreach (var ext in this.AvailDecorators.ToList<GlowDecorator>())
                result.Add(ext.Name);
            return result.ToArray<string>();
        }

        public string[] GetNamesOfAvailNotifiers()
        {
            List<string> result = new List<string>();
            foreach (var ext in this.AvailNotifiers.ToList<GlowNotifier>())
                result.Add(ext.Name);
            return result.ToArray<string>();
        }

        public GlowDriver GetDriver(string name)
        {
            foreach (var ext in this.AvailDrivers.ToList<GlowDriver>())
                if (ext.Name.Equals(name))
                    return ext;
            return null;
        }

        public GlowScreenGrabber GetScreenGrabber(string name)
        {
            foreach (var ext in this.AvailScreenDrivers.ToList<GlowScreenGrabber>())
                if (ext.Name.Equals(name))
                    return ext;
            return null;
        }

        public GlowScreenProcessor GetScreenProcessor(string name)
        {
            foreach (var ext in this.AvailScreenProcessors.ToList<GlowScreenProcessor>())
                if (ext.Name.Equals(name))
                    return ext;
            return null;
        }

        public GlowDecorator GetDecorator(string name)
        {
            foreach (var ext in this.AvailDecorators.ToList<GlowDecorator>())
                if (ext.Name.Equals(name))
                    return ext;
            return null;
        }

        public GlowNotifier GetNotifier(string name)
        {
            foreach (var ext in this.AvailNotifiers.ToList<GlowNotifier>())
                if (ext.Name.Equals(name))
                    return ext;
            return null;
        }

        public GlowDriver GetDefaultDriver()
        {
            return this.AvailDrivers.First<GlowDriver>();
        }

        public GlowScreenGrabber GetDefaultScreenGrabber()
        {
            return this.AvailScreenDrivers.First<GlowScreenGrabber>();
        }

        public GlowScreenProcessor GetDefaultScreenProcessor()
        {
            return this.AvailScreenProcessors.First<GlowScreenProcessor>();
        }
    }
}
