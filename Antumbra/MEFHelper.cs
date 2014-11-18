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
            foreach (var extension in plugins) {
                Console.WriteLine("Extension Found: " + extension.Name);
                if (extension.Type == null) {
                    Console.WriteLine("Ignoring Extension - null type");
                }
                else if (extension.Type.Equals("Driver")) {
                    Console.WriteLine("Type: Driver");
                    this.AvailDrivers.Add((GlowDriver)extension);
                }
                else if (extension.Type.Equals("Screen Grabber")) {
                    Console.WriteLine("Type: Screen Grabber");
                    this.AvailScreenDrivers.Add((GlowScreenGrabber)extension);
                }
                else if (extension.Type.Equals("Screen Processor")) {
                    Console.WriteLine("Type: Screen Processor");
                    this.AvailScreenProcessors.Add((GlowScreenProcessor)extension);
                }
                else if (extension.Type.Equals("Decorator")) {
                    Console.WriteLine("Type: Decorator");
                    this.AvailDecorators.Add((GlowDecorator)extension);
                }
                else if (extension.Type.Equals("Notifier")) {
                    Console.WriteLine("Type: Notifier");
                    this.AvailNotifiers.Add((GlowNotifier)extension);
                }
                else {
                    Console.WriteLine("Ignoring Extension - invalid type");
                }
            }
        }

        private void Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(this.path, "*.glow.dll"));
            catalog.Catalogs.Add(new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()));
            this.container = new CompositionContainer(catalog);
            this.container.ComposeParts(this);
            //this.container.SatisfyImportsOnce(this);//get rid of later on
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
